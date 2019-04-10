using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;

namespace cc_lab2
{
    public static class LanguageUtils
    {
        public static void RemoveUselessSymbols(Grammar grammar)
        {
            var unproductive = grammar.Rules.Select(rule => rule.Left).ToHashSet();
            var productive = grammar.Terminals.ToHashSet();
            
            bool placeHolder = true;

            while (placeHolder)
            {
                placeHolder = false;
                foreach (var rule in grammar.Rules)
                    if (rule.Right.All(ch => productive.Contains(ch)))
                        if (!productive.Contains(rule.Left))
                        {
                            productive.Add(rule.Left);
                            unproductive.Remove(rule.Left);
                            placeHolder = true;
                        }
            }

            Console.WriteLine("unproductive : " +  string.Join(" ", unproductive));
            Console.WriteLine("productive : " + string.Join(" ", productive));

            foreach (var rule in grammar.Rules.Where(rule => rule.Right.Any(ch => unproductive.Contains(ch))).ToList())
                grammar.Rules.Remove(rule);
            
            var reachableRules = grammar.Rules.Where(rule => rule.Left.Equals(grammar.Start)).ToHashSet();
            
            foreach (var grammarRule in grammar.Rules)
                if (reachableRules.Contains(grammarRule))
                    foreach (var ch in grammarRule.Right)
                        if (ch.All(Char.IsUpper))
                            reachableRules.UnionWith(grammar.Rules.Where(rule => rule.Left.Equals(ch)));

            grammar.Rules = reachableRules;

            foreach (var ch in grammar.Terminals.Union(grammar.NonTerminals).ToList())
            {
                if (!grammar.Rules.Any(rule => rule.Left.Equals(ch)))
                    grammar.NonTerminals.Remove(ch);

                if (!grammar.Rules.Any(rule => rule.Right.Any(d => d.Equals(ch))))
                    grammar.Terminals.Remove(ch);
            }

        }

        private static List<String> CommonPrefix(IEnumerable<Rule> rules)
        {
            if (rules == null || !rules.Any())
                return new List<string>();

            var s1 = rules.Min();
            var s2 = rules.Max();
            
            for (var i = 0; i < s1.Right.Count; i++)
                if (!s1.Right[i].Equals(s2.Right[i]))
                    return s1.Right.Take(i).ToList();

            return s1.Right;
        }
        
        public static void RemoveLeftFactoring(Grammar grammar)
        {
            var nonTerminals = grammar.NonTerminals.ToHashSet();
            var rules = grammar.Rules.ToHashSet();
            foreach (var nonTerminal in nonTerminals)
            {
                var nonTerminalRules = rules.Where((rule => rule.Left.Equals(nonTerminal) && !rule.Right[0].Equals(Grammar.Eps)));
                if (nonTerminalRules.Count() > 1)
                {
                    var prefix = CommonPrefix(nonTerminalRules);
                    if (prefix.Count != 0)
                    {
                        var newNonTerminal = nonTerminal + "~";
                        grammar.NonTerminals.Add(newNonTerminal);
                        grammar.Rules.Add(new Rule()
                        {
                            Left = nonTerminal,
                            Right = prefix.Append(newNonTerminal).ToList()
                        });
                        foreach (var nonTerminalRule in nonTerminalRules)
                        {
                            if (nonTerminalRule.Right.Take(prefix.Count).SequenceEqual(prefix))
                            {
                                var newRightPart =
                                    nonTerminalRule.Right.Skip(prefix.Count).ToList();
                                grammar.Rules.Remove(nonTerminalRule);
                                grammar.Rules.Add(new Rule()
                                {
                                    Left = newNonTerminal,
                                    Right = newRightPart.Count == 0 ? new List<string>{Grammar.Eps} : newRightPart
                                });
                            }
                        }
                    }
                }
            }
        }
        
        private static HashSet<string> FindEpsNonTerminals(Grammar grammar)
        {
            var rulesWithoutTerminals = grammar.Rules.Where(rule => !rule.HasTerminals(grammar.NonTerminals));
            
            var isEpsilon = new Dictionary<string,bool>();
            var concernedRules = new Dictionary<string, List<Rule>>();
            foreach (var grammarNonTerminal in grammar.NonTerminals)
            {
                isEpsilon.Add(grammarNonTerminal, false);
                concernedRules.Add(grammarNonTerminal, new List<Rule>());
            }
            
            var counter =  new Dictionary<Rule, int>();
            foreach (var ruleWithoutTerminals in rulesWithoutTerminals)
                counter.Add(ruleWithoutTerminals, ruleWithoutTerminals.NonTerminalsCount(grammar.NonTerminals));
            
            foreach (var ruleWithoutTerminals in rulesWithoutTerminals)
                foreach (var nt in ruleWithoutTerminals.Right)
                    if(!Grammar.Eps.Equals(nt))
                        concernedRules[nt].Add(ruleWithoutTerminals);
            
            var queue = new Queue<string>();

            foreach (var keyValuePair in counter)
                if (keyValuePair.Value == 0)
                {
                    queue.Enqueue(keyValuePair.Key.Left);
                    isEpsilon[keyValuePair.Key.Left] = true;
                }

            while (queue.Count >0)
            {
                var nonTerm = queue.Dequeue();
                foreach (var rule in concernedRules[nonTerm])
                {
                    var count = counter[rule];
                    counter[rule] = count - 1;

                    if (counter[rule] == 0 && !isEpsilon[rule.Left])
                    {
                        isEpsilon[rule.Left] = true;
                        queue.Enqueue(rule.Left);
                    }
                }
            }
            
            return isEpsilon.Where(pair => pair.Value).Select(pair => pair.Key).ToHashSet();
        }
        
        private static IEnumerable<IEnumerable<T>> 
            GetPermutationsWithRept<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutationsWithRept(list, length - 1)
                .SelectMany(t => list, 
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
        
        private static IEnumerable<IEnumerable<T>>
            GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(o => !t.Contains(o)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
        
        private static IEnumerable<IEnumerable<T>>
            GetCombinations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetCombinations(list, length - 1)
                .SelectMany(t => list, (t1, t2) => t1.Concat(new T[] { t2 }));
        }
        
        public static void RemoveEPS(ref Grammar grammar)
        {
            var ret = grammar.Copy();

            var epsNonTerms = FindEpsNonTerminals(grammar);

            foreach (var grammarRule in grammar.Rules)
            {
                var ruleEps = epsNonTerms.Intersect(grammarRule.Right).ToList();

                for (int i = 0; i < ruleEps.Count(); i++)
                {
                    foreach (var combo in GetCombinations(ruleEps, i+1))
                    {
                        var newRule = grammarRule.Copy();
                        foreach (var nt in combo)
                            newRule.Right.Remove(nt);

                        if (newRule.Right.Count > 0)
                            ret.Rules.Add(newRule);

                    }
                }
            }
            
            var epsRules = new List<Rule>();
            
            foreach (var retRule in ret.Rules)
                if(retRule.Right[0].Equals(Grammar.Eps))
                    epsRules.Add(retRule);
            
            foreach (var epsRule in epsRules)
                ret.Rules.Remove(epsRule);

            grammar = ret;
        }
        
    }
}
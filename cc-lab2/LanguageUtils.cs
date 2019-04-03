using System;
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
        
    }
}
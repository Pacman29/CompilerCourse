using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cc_lab2
{
    public static class LeftRecursionResolver
    {
        public static void ResolveImmediateRecursion(Grammar grammar)
        {
            for(var i = 0; i< grammar.NonTerminals.Count; ++i)
                ResolveImmediateRecursion(grammar, grammar.NonTerminals.ElementAtOrDefault(i));
        }
        
        public static void ResolveIndirectRecursion(Grammar grammar)
        {
            var nonTerms = grammar.NonTerminals.ToList();

            for (int i = 0; i < nonTerms.Count ; i++)
            {
                for (var j = 0; j < i; j++)
                    foreach (var rule in grammar.Rules
                        .Where(rule => rule.Left.Equals(nonTerms[i]) && rule.Right[0].Equals(nonTerms[j])).ToList())
                    {
                        grammar.Rules.Remove(rule);
                        foreach (var r in grammar.Rules.Where(rule1 => rule1.Left.Equals(rule.Right[0])).ToList())
                        {
                            var right = r.Right.ToList();
                            right.AddRange(rule.Right.GetRange(1, rule.Right.Count - 1));
                            grammar.Rules.Add(new Rule
                            {
                                Left = rule.Left,
                                Right = right
                            });
                        }
                    }
                
                ResolveImmediateRecursion(grammar, nonTerms[i]);
            }
        }

        private static void ResolveImmediateRecursion(Grammar grammar, String nonTerm)
        {
            var recursionRules = new List<Rule>();
            var nonRecursionRules = new List<Rule>();
            
            foreach (var grammarRule in grammar.Rules)
                if(grammarRule.Left.Equals(nonTerm))
                    (grammarRule.Right[0].Equals(nonTerm) ? recursionRules : nonRecursionRules).Add(grammarRule);

            if (recursionRules.Count != 0)
            {
                var newNonTerm = $"{nonTerm}'";
                grammar.NonTerminals.Add(newNonTerm);
                
                nonRecursionRules.ForEach(rule =>
                {
                    //grammar.Rules.Remove(rule);
                    grammar.Rules.Add(new Rule
                    {
                        Left = nonTerm,
                        Right = rule.Right.Append(newNonTerm).ToList()
                    });
                });
                
                recursionRules.ForEach(rule =>
                {
                    grammar.Rules.Remove(rule);
                    grammar.Rules.Add(new Rule
                    {
                        Left = newNonTerm,
                        Right = rule.Right.GetRange(1, rule.Right.Count-1).Append(newNonTerm).ToList()
                    });
                    grammar.Rules.Add(new Rule
                    {
                        Left = newNonTerm,
                        Right = rule.Right.Skip(1).ToList()
                    });
                });
            }
        }
    }
}
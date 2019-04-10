using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using cc_lab1;
using cc_lab2;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace cc_lab3
{
    public class LL1Parser
    {
        private Grammar Grammar { get; set; }

        public LL1Parser(Grammar grammar)
        {
            Grammar = grammar;
        }

        private class Iter
        {
            public int val { get; set; } = 0;

            public void Inc()
            {
                val++;
            }
        }

        private Tree _tree;

        public bool ProcessText(string text)
        {
            string[] separatingChars = { " ","\n","\t"};
            var input = text.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries).ToList();
            var iter = new Iter();
            _tree = new Tree()
            {
                Data = Grammar.Start
            };
            var check = CheckRulesForNonTerminal(Grammar.Start, input, iter, _tree);
            var checkI = iter.val == input.Count;
            return check && checkI;
        }
        
        private bool CheckRulesForNonTerminal(string nonTerm, List<string> input, Iter iter, Tree tree, int depth = 0)
        {
            foreach (var rule1 in Grammar.Rules.Where(rule => rule.Left.Equals(nonTerm)))
            {
                var prefix = new StringBuilder();
                for (int i = 0; i < depth; i++)
                    prefix.Append("\t");

                Console.WriteLine($"{prefix} Проверка {rule1}");
                if (CheckRule(rule1, input, iter, tree, depth + 1))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckRule(Rule rule, List<string> input, Iter iter, Tree tree, int depth = 0)
        {
            var origIter = iter.val;
            var prefix = new StringBuilder();
            for (int i = 0; i < depth; i++)
                prefix.Append("\t");
            
            foreach (var right in rule.Right)
            {
                if (iter.val == input.Count)
                    return true;
                
                var child = new Tree()
                {
                    Data = right
                };

                tree.Children.AddLast(child);
                
                if (Grammar.NonTerminals.Contains(right))
                {
                    if (!CheckRulesForNonTerminal(right, input, iter, child, depth))
                    {
                        iter.val = origIter;
                        tree.Children.Clear();
                        return false;
                    }
                } 
                else if (right.Equals(input[iter.val]))
                {
                    Console.WriteLine($"{prefix} Обработан символ {iter.val} : {input[iter.val]}");
                    iter.Inc();
                }
                else
                {
                    Console.WriteLine($"{prefix} Обнаружена ошибка {iter.val} : {input[iter.val]}");
                    Console.WriteLine($"{prefix} Правило {rule}");
                    iter.val = origIter;
                    tree.Children.Clear();
                    return false;
                }
            }
            Console.WriteLine($"{prefix} Подходит {rule}");
            return true;
        }

        public void PrintTree(string filename)
        {
            this._tree.PrintTree(filename);
        }
       
    }
}
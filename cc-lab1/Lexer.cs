using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace cc_lab1
{
    public static class Lexer
    {
        public static string AvailableSymbols = "abcdefghijklmnopqrstuvwxyz";
        public const char StartBracketOperand = '(';
        public const char EndBracketOperand = ')';
        public const char OneOrMoreOperand = '+';
        public const char ZeroOrMoreOperand = '*';
        public const char OrOperand = '|';
        public const char AndOperand = '.';
        public static string AvailableOperands = new string(new char[]
        {
            StartBracketOperand, EndBracketOperand, 
            OneOrMoreOperand, ZeroOrMoreOperand,
            OrOperand, AndOperand
        });
        public static char EndInputSymbol = '#';
        public static char EmptySymbol = (char) 1;
        
        public static string ConvertRegexpToPostfix(string regexp)
        {
            var result = new List<char>();
            var ops = new Stack<char>();

            var prevIcheck = 0;
            
            for (int i = 0; i<regexp.Length; ++i)
            {
                var ch = regexp[i];
                if (i > 0 && i != prevIcheck && 
                    (AvailableSymbols.Contains(regexp[i - 1]) 
                     || EndBracketOperand.Equals(regexp[i - 1])
                     || ZeroOrMoreOperand.Equals(regexp[i - 1])
                     || OneOrMoreOperand.Equals(regexp[i - 1])) 
                    && (AvailableSymbols.Contains(ch)  || StartBracketOperand.Equals(ch)))
                {
                    prevIcheck = i--;
                    ch = '.';
                }
                if(StartBracketOperand.Equals(ch))
                    ops.Push(ch);
                else if (EndBracketOperand.Equals(ch))
                {
                    while (ops.Count != 0 && !StartBracketOperand.Equals(ops.Peek()))
                        result.Add(ops.Pop());

                    if (ops.Count != 0 && StartBracketOperand.Equals(ops.Peek()))
                        ops.Pop();
                }
                else
                {
                    var priority = OperandPriority(ch);
                    if(priority == -1)
                        result.Add(ch);
                    else
                    {
                        if(ZeroOrMoreOperand.Equals(ch) || OneOrMoreOperand.Equals(ch))
                            result.Add(ch);
                        else if(ops.Count == 0)
                            ops.Push(ch);
                        else
                        {
                            while(ops.Count != 0 && StartBracketOperand.Equals(ops.Peek()) &&
                                  priority <= OperandPriority(ops.Peek()))
                                result.Add(ops.Pop());
                            ops.Push(ch);
                        }
                    }
                }
            }

            while (ops.Count != 0) 
            {
                result.Add(ops.Pop());
            }
            
            return new string(result.ToArray());
        }

        private static int OperandPriority(char op)
        {
            switch (op)
            {
                case OneOrMoreOperand:
                case ZeroOrMoreOperand:
                    return 4;
                case AndOperand:
                    return 3;
                case OrOperand:
                    return 2;
                default:
                    return -1;
            }
        }
    }
}
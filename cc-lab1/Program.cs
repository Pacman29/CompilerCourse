using System;

namespace cc_lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            var regexp = EnterRegexp();
            var testStr = "bcabbccccc";
            var postfix = RegexpToPostfix(regexp);
            var nfa = PostfixToNFA(postfix);
            //Console.WriteLine($"Тест NFA: {testStr} -> {nfa.Calculate(testStr)}");
            var dfa = NFAtoDFA(nfa);
            var minfa = DFAtoMinFA(dfa);
            Console.WriteLine($"Тест DFA: {testStr} -> {dfa.Calculate(testStr)}");
            Console.WriteLine($"Тест MinFA: {testStr} -> {minfa.Calculate(testStr)}");
            
        }

        private static DFA NFAtoDFA(NFA nfa)
        {
            Console.WriteLine("Построение DFA по NFA");
            var result = DFA.FromNFA(nfa,new TompsonDFABuildAlgorithm());
            result.PrintGraph("DFA.gv");
            return result;
        }
        
        private static MinFA DFAtoMinFA(DFA dfa)
        {
            Console.WriteLine("Построение MinFA по DFA");
            var result = MinFA.FromDFA(dfa, new TableMinimizingAlgorithm());
            result.PrintGraph("MinFA.gv");
            return result;
        }

        private static string RegexpToPostfix(string regexp)
        {
            Console.WriteLine("Построение постфикной нотации по регулярному выражению");
            var result = Lexer.ConvertRegexpToPostfix(regexp);
            Console.WriteLine(result);
            return result;
        }
        
        private static NFA PostfixToNFA(string postfix)
        {
            Console.WriteLine("Построение NFA по постфикной нотации ");
            var result = NFA.fromPostfix(postfix);
            result.PrintGraph("NFA.gv");
            return result;
        }

        private static string EnterRegexp()
        {
            //string regexp = "(((a|b)*abb)|c)+";
            string regexp = "(a|b)*aab";
            Console.WriteLine($"Введенное регулярное выражение: {regexp}");
            return regexp;
        }
    }
}
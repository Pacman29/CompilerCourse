using System.Text.RegularExpressions;
using cc_lab1;
using Xunit;

namespace TestCCLab1
{
    public class UnitTestMinFA
    {
        [Fact]
        public void Test1()
        {
            var test = "a";
            var testRegexp = "a";

            var postfix = Lexer.ConvertRegexpToPostfix(testRegexp);
            var nfa = NFA.fromPostfix(postfix);
            var dfa = DFA.FromNFA(nfa, new TompsonDFABuildAlgorithm());
            var minfa = MinFA.FromDFA(dfa, new TableMinimizingAlgorithm());

            var regexp = new Regex($"^{testRegexp}$");
            
            Assert.Equal(regexp.IsMatch(test),minfa.Calculate(test));
        }
        
        [Fact]
        public void Test2()
        {
            var test = "b";
            var testRegexp = "a";

            var postfix = Lexer.ConvertRegexpToPostfix(testRegexp);
            var nfa = NFA.fromPostfix(postfix);
            var dfa = DFA.FromNFA(nfa, new TompsonDFABuildAlgorithm());
            var minfa = MinFA.FromDFA(dfa, new TableMinimizingAlgorithm());

            var regexp = new Regex($"^{testRegexp}$");
            
            Assert.Equal(regexp.IsMatch(test),minfa.Calculate(test));
        }
        
        [Fact]
        public void Test4()
        {
            var test = "a";
            var testRegexp = "a*";

            var postfix = Lexer.ConvertRegexpToPostfix(testRegexp);
            var nfa = NFA.fromPostfix(postfix);
            var dfa = DFA.FromNFA(nfa, new TompsonDFABuildAlgorithm());
            var minfa = MinFA.FromDFA(dfa, new TableMinimizingAlgorithm());

            var regexp = new Regex($"^{testRegexp}$");
            
            Assert.Equal(regexp.IsMatch(test),minfa.Calculate(test));
        }
        
        [Fact]
        public void Test5()
        {
            var test = "b";
            var testRegexp = "a*";

            var postfix = Lexer.ConvertRegexpToPostfix(testRegexp);
            var nfa = NFA.fromPostfix(postfix);
            var dfa = DFA.FromNFA(nfa, new TompsonDFABuildAlgorithm());
            var minfa = MinFA.FromDFA(dfa, new TableMinimizingAlgorithm());

            var regexp = new Regex($"^{testRegexp}$");
            
            Assert.Equal(regexp.IsMatch(test),minfa.Calculate(test));
        }
        
        [Fact]
        public void Test6()
        {
            var test = "aaaa";
            var testRegexp = "a*";

            var postfix = Lexer.ConvertRegexpToPostfix(testRegexp);
            var nfa = NFA.fromPostfix(postfix);
            var dfa = DFA.FromNFA(nfa, new TompsonDFABuildAlgorithm());
            var minfa = MinFA.FromDFA(dfa, new TableMinimizingAlgorithm());

            var regexp = new Regex($"^{testRegexp}$");
            
            Assert.Equal(regexp.IsMatch(test),minfa.Calculate(test));
        }
        
        [Fact]
        public void Test7()
        {
            var test = "aabb";
            var testRegexp = "a*";

            var postfix = Lexer.ConvertRegexpToPostfix(testRegexp);
            var nfa = NFA.fromPostfix(postfix);
            var dfa = DFA.FromNFA(nfa, new TompsonDFABuildAlgorithm());
            var minfa = MinFA.FromDFA(dfa, new TableMinimizingAlgorithm());

            var regexp = new Regex($"^{testRegexp}$");
            
            Assert.Equal(regexp.IsMatch(test),minfa.Calculate(test));
        }
        
        [Fact]
        public void Test8()
        {
            var test = "bbaa";
            var testRegexp = "a*";

            var postfix = Lexer.ConvertRegexpToPostfix(testRegexp);
            var nfa = NFA.fromPostfix(postfix);
            var dfa = DFA.FromNFA(nfa, new TompsonDFABuildAlgorithm());
            var minfa = MinFA.FromDFA(dfa, new TableMinimizingAlgorithm());

            var regexp = new Regex($"^{testRegexp}$");
            
            Assert.Equal(regexp.IsMatch(test),minfa.Calculate(test));
        }
        
        [Fact]
        public void Test9()
        {
            var test = "ab";
            var testRegexp = "a*b";

            var postfix = Lexer.ConvertRegexpToPostfix(testRegexp);
            var nfa = NFA.fromPostfix(postfix);
            var dfa = DFA.FromNFA(nfa, new TompsonDFABuildAlgorithm());
            var minfa = MinFA.FromDFA(dfa, new TableMinimizingAlgorithm());

            var regexp = new Regex($"^{testRegexp}$");
            
            Assert.Equal(regexp.IsMatch(test),minfa.Calculate(test));
        }
        
        [Fact]
        public void Test10()
        {
            var test = "ba";
            var testRegexp = "a*b";

            var postfix = Lexer.ConvertRegexpToPostfix(testRegexp);
            var nfa = NFA.fromPostfix(postfix);
            var dfa = DFA.FromNFA(nfa, new TompsonDFABuildAlgorithm());
            var minfa = MinFA.FromDFA(dfa, new TableMinimizingAlgorithm());

            var regexp = new Regex($"^{testRegexp}$");
            
            Assert.Equal(regexp.IsMatch(test),minfa.Calculate(test));
        }
    }
}
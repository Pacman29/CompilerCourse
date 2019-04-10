using System;
using System.IO;
using cc_lab2;
using cc_lab3;
using Xunit;

namespace TestCClab3
{
    public class UnitTest1
    {
        private Grammar _grammar;

        public static string workingDirectory = Environment.CurrentDirectory;

        public readonly string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        
        public UnitTest1()
        {
            _grammar = LoadGrammar.LoadFromFile($@"{projectDirectory}/examples/ex2.json").ToGrammar();
            LeftRecursionResolver.ResolveIndirectRecursion(_grammar);
            LanguageUtils.RemoveLeftFactoring(_grammar);
            LanguageUtils.RemoveEPS(ref _grammar);
        }

        [Fact]
        public void Test_ok()
        {
            var parser = new LL1Parser(_grammar);
            Assert.True(parser.ProcessText(" a + b >= ( b + c )"));
        }
        
        [Fact]
        public void Test_fail()
        {
            var parser = new LL1Parser(_grammar);
            Assert.False(parser.ProcessText(" a + adasda ^ ( b + c )"));
        }
    }
}
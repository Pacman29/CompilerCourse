using System;
using cc_lab1;
using Xunit;

namespace TestCCLab1
{
    public class UnitTestPostfix
    {
        [Fact]
        public void Test1()
        {
            var input = "a";
            Assert.Equal("a",Lexer.ConvertRegexpToPostfix(input));
        }
        
        [Fact]
        public void Test2()
        {
            var input = "ab";
            Assert.Equal("ab.",Lexer.ConvertRegexpToPostfix(input));
        }
        
        [Fact]
        public void Test3()
        {
            var input = "a*";
            Assert.Equal("a*",Lexer.ConvertRegexpToPostfix(input));
        }
        
        [Fact]
        public void Test4()
        {
            var input = "a+";
            Assert.Equal("a+",Lexer.ConvertRegexpToPostfix(input));
        }
        
        [Fact]
        public void Test5()
        {
            var input = "a|b";
            Assert.Equal("ab|",Lexer.ConvertRegexpToPostfix(input));
        }
        
        [Fact]
        public void Test6()
        {
            var input = "a(g|f)c";
            Assert.Equal("agf|c..",Lexer.ConvertRegexpToPostfix(input));
        }
        
        [Fact]
        public void Test7()
        {
            var input = "a|b|c";
            Assert.Equal("abc||",Lexer.ConvertRegexpToPostfix(input));
        }
        
        [Fact]
        public void Test8()
        {
            var input = "(a|b)(c|d)";
            Assert.Equal("ab|cd|.",Lexer.ConvertRegexpToPostfix(input));
        }
    }
}
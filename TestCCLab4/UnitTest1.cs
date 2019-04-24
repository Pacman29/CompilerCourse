using System;
using System.Linq;
using cc_lab4;
using Xunit;

namespace TestCCLab4
{
    public class UnitTest1
    {
        private ShuntingYardSimpleMath SY;

        public UnitTest1()
        {
            SY = new ShuntingYardSimpleMath();
        }

        [Fact]
        public void Test1()
        {
            var res = SY.Execute(
                "( 3 + ( 4 * 2 ) ) * 2 / ( 6 - 5 )".Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(), null);
            Assert.Equal(22.0 , res);
        }
        
        [Fact]
        public void Test2()
        {
            var res = SY.Execute(
                " 2 ^ 2 + 2 ".Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(), null);
            Assert.Equal(6.0 , res);
        }
        
        [Fact]
        public void Test3()
        {
            Assert.Throws<Exception>(() => SY.Execute(
                " ( ( 3 + 4 ) ".Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(), null));
        }
        
        [Fact]
        public void Test4()
        {
            Assert.Throws<Exception>(() => SY.Execute(
                " ( 3 + 4 ) )".Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(), null));
        }
        
        [Fact]
        public void Test5()
        {
            Assert.Throws<Exception>(() => SY.Execute(
                "{ ( 3 + 4 ) )".Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(), null));
        }
    }
}
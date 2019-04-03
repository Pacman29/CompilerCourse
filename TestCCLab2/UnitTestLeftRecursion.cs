using System;
using cc_lab2;
using Xunit;
using Xunit.Abstractions;

namespace TestCCLab2
{
    public class UnitTestLeftRecursion
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTestLeftRecursion(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test1()
        {
            var grammar = Grammar.LoadFromFile(@"/home/pacman29/Рабочий стол/cc-lab1/TestCCLab2/testGrammars/immidiate.json");
            var expectedGrammar = Grammar.LoadFromFile(@"/home/pacman29/Рабочий стол/cc-lab1/TestCCLab2/testExpectedGrammars/immidiate.json");
            
            LeftRecursionResolver.ResolveImmediateRecursion(grammar);
            _testOutputHelper.WriteLine("Immidiate test:");
            _testOutputHelper.WriteLine(grammar.ToString());
            Assert.Equal(expectedGrammar,grammar);
        }
        
        [Fact]
        public void Test2()
        {
            var grammar = Grammar.LoadFromFile(@"/home/pacman29/Рабочий стол/cc-lab1/TestCCLab2/testGrammars/indirect.json");
            var expectedGrammar = Grammar.LoadFromFile(@"/home/pacman29/Рабочий стол/cc-lab1/TestCCLab2/testExpectedGrammars/indirect.json");

            LeftRecursionResolver.ResolveIndirectRecursion(grammar);
            _testOutputHelper.WriteLine("Indirect test:");
            _testOutputHelper.WriteLine(grammar.ToString());
            Assert.Equal(expectedGrammar,grammar);
        }
        
        [Fact]
        public void Test3()
        {
            var grammar = Grammar.LoadFromFile(@"/home/pacman29/Рабочий стол/cc-lab1/TestCCLab2/testGrammars/dragon.json");
            var expectedGrammar = Grammar.LoadFromFile(@"/home/pacman29/Рабочий стол/cc-lab1/TestCCLab2/testExpectedGrammars/dragon.json");
            
            LeftRecursionResolver.ResolveIndirectRecursion(grammar);
            _testOutputHelper.WriteLine("Dragon test:");
            _testOutputHelper.WriteLine(grammar.ToString());
            Assert.Equal(expectedGrammar,grammar);
        }
    }
}
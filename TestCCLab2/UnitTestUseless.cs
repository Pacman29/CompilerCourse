using cc_lab2;
using Xunit;
using Xunit.Abstractions;

namespace TestCCLab2
{
    public class UnitTestUseless
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTestUseless(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test1()
        {
            var grammar = Grammar.LoadFromFile(@"/home/pacman29/Рабочий стол/cc-lab1/TestCCLab2/testGrammars/useless.json");
            var expectedGrammar = Grammar.LoadFromFile(@"/home/pacman29/Рабочий стол/cc-lab1/TestCCLab2/testExpectedGrammars/useless.json");
            
            LanguageUtils.RemoveUselessSymbols(grammar);
            _testOutputHelper.WriteLine("Useless test:");
            _testOutputHelper.WriteLine(grammar.ToString());
            Assert.Equal(expectedGrammar,grammar);
        }
        
        [Fact]
        public void Test2()
        {
            var grammar = Grammar.LoadFromFile(@"/home/pacman29/Рабочий стол/cc-lab1/TestCCLab2/testGrammars/useless2.json");
            var expectedGrammar = Grammar.LoadFromFile(@"/home/pacman29/Рабочий стол/cc-lab1/TestCCLab2/testExpectedGrammars/useless2.json");
            
            LanguageUtils.RemoveUselessSymbols(grammar);
            _testOutputHelper.WriteLine("Useless test:");
            _testOutputHelper.WriteLine(grammar.ToString());
            Assert.Equal(expectedGrammar,grammar);
        }
    }
}
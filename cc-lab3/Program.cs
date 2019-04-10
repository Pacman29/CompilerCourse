using System;
using System.Linq;
using cc_lab2;

namespace cc_lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            var grammar = LoadGrammar.LoadFromFile(@"./examples/ex2.json").ToGrammar();
            Console.WriteLine(grammar.ToString());
            LeftRecursionResolver.ResolveIndirectRecursion(grammar);
            LanguageUtils.RemoveLeftFactoring(grammar);
            LanguageUtils.RemoveEPS(ref grammar);
            Console.WriteLine(grammar.ToString());
            var parser = new LL1Parser(grammar);
            Console.WriteLine(parser.ProcessText("a + b = ( b - c )"));
            parser.PrintTree(@"./tree.gv");
        }
    }
}
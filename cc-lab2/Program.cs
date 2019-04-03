using System;

namespace cc_lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = Grammar.LoadFromFile(@"./examples/useless2.json");
            LanguageUtils.RemoveUselessSymbols(g);
            //LeftRecursionResolver.ResolveIndirectRecursion(g);
            Console.WriteLine(g);
        }
    }
}
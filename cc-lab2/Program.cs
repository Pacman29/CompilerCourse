using System;

namespace cc_lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = Grammar.LoadFromFile(@"C:\Users\pacman29\Desktop\CompilerCourse\cc-lab2\examples\useless2.json");
            LanguageUtils.RemoveUselessSymbols(g);
            //LeftRecursionResolver.ResolveIndirectRecursion(g);
            Console.WriteLine(g);
        }
    }
}
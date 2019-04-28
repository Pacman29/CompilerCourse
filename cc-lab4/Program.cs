using System;
using System.Collections.Generic;
using cc_lab4.SyntaxAnalyzer;

namespace cc_lab4
{
    static class Program
        {
            static void Main(string[] args)
            {
                /*var SY = new ShuntingYardSimpleMath();
                var s = "( 3 + 5 )";
                Console.WriteLine($"input: {s}\n"); 
                var ss = s.Split(' ').ToList();
                SY.DebugRPNSteps += SY_DebugRPNSteps;
                SY.DebugResSteps += SY_DebugResSteps;
                try
                { 
                    var res = SY.Execute(ss, null);
                    Console.WriteLine($"input: {s} = {res}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR!!!");
                    Console.WriteLine(ex.Message);
                }*/
                
                string strExpr;
                while (true)
                {
                    Console.Write("Enter an arithmetic expression: ");                
                    strExpr = Console.ReadLine();

                    if (strExpr.Length == 0)
                        break;

                    var parser = new CParser();
                    if (parser.Parse(strExpr))
                        Console.WriteLine("The result is: {0}", parser.GetValue());                    

                    Console.WriteLine();
                }
            }
    
            static void SY_DebugRPNSteps(List<object> inter, List<char> opr)
            {
                Console.Write("RPN ");
                Console.WriteLine("\n inters:");
                foreach (object o in inter)
                    Console.Write("{0} ", o.ToString());
                Console.WriteLine("\n operators:");
                foreach (char o in opr)
                    Console.Write("{0} ", o.ToString());
                Console.WriteLine();
            }
    
            static void SY_DebugResSteps(List<object> res, List<double> var)
            {
                Console.Write("RPN ");
                foreach (object o in res)
                    Console.Write("{0} ", o.ToString());
                Console.Write("\n= ");
                foreach (double o in var)
                    Console.Write("{0} ", o.ToString());
                Console.WriteLine();
            }
            
        }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace cc_lab4
{
    static class Program
        {
            static void Main(string[] args)
            {
                {
                    ShuntingYardSimpleMath SY = new ShuntingYardSimpleMath();
                    String s = "( 3 + ( 4 * 2 ) ) * 2 / ( 6 - 5 )";
                    Console.WriteLine("input: {0}", s); Console.WriteLine();
                    List<String> ss = s.Split(' ').ToList();
                    SY.DebugRPNSteps += SY_DebugRPNSteps;
                    SY.DebugResSteps += SY_DebugResSteps;
                    try
                    { 
                        Double res = SY.Execute(ss, null);
                        Console.WriteLine("input: {0} = {1}", s, res);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR!!!");
                        Console.WriteLine(ex.Message);
                    }
                    Console.ReadKey();
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
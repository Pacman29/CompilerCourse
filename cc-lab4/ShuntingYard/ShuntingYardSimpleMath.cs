using System;
using System.Collections.Generic;
using System.Linq;
using cc_lab4.ShuntingYard;

namespace cc_lab4
{
    public class ShuntingYardSimpleMath : ShuntingYardBase<double, string>
    {
        Dictionary<char, PrecedensAssociativity> Oprs = new Dictionary<char, PrecedensAssociativity>() 
        {
            { '+', new PrecedensAssociativity(2,PrecedensAssociativity.Asso.Left, (d, d1) => d + d1)},
            { '-', new PrecedensAssociativity(2,PrecedensAssociativity.Asso.Left, (d, d1) => d - d1)},
            { '*', new PrecedensAssociativity(3,PrecedensAssociativity.Asso.Left, (d, d1) => d * d1)},
            { '/', new PrecedensAssociativity(3,PrecedensAssociativity.Asso.Left, (d, d1) => d / d1)},
            { '^', new PrecedensAssociativity(4,PrecedensAssociativity.Asso.Right, Math.Pow)},
        };

        public override double Evaluate(double result1, char opr, double result2)
        {
            KeyValuePair<char, PrecedensAssociativity> operation;
            try
            {
                operation = Oprs.First(pair => pair.Key == opr);
            }
            catch (InvalidOperationException e)
            {
                throw new Exception("Wrong operator!!");
            }

            return operation.Value.Execute(result1, result2);
        }

        public override double TypecastIdentifier(string input, object TagObj)
        {
            double result;
            if (double.TryParse(input, out result))
                return result;
            throw new Exception("Wrong identifier!!");
        }
        public override bool IsIdentifier(string input)
        {
            double result;
            return double.TryParse(input, out result);
        }
        public override bool IsOperator(char? opr)
        {
            if(opr == null) return false;
            return Oprs.ContainsKey((char)opr);
        }
        public override char? TypecastOperator(string input)
        {
            if (!Oprs.ContainsKey(input[0]) || input.Length > 1)
                return null;
            return (char?)input[0];
        }

        public override PrecedensAssociativity.Asso Association(char opr)
        {
            if (!Oprs.ContainsKey(opr))
                throw new Exception("Wrong operator!!");
            return Oprs[opr].Associativity;
        }

        public override int Precedence(char opr1, char opr2)
        {
            if (!Oprs.ContainsKey(opr1))
                throw new Exception("Wrong operator!!");
            if (!Oprs.ContainsKey(opr2))
                throw new Exception("Wrong operator!!");
            if (Oprs[opr1].Prec > Oprs[opr2].Prec) return 1;
            if (Oprs[opr1].Prec < Oprs[opr2].Prec) return -1;
            return 0;
        }

        public override bool IsNoise(string input)
        {
            return false;
        }
    }
}

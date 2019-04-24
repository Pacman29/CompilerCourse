using System;

namespace cc_lab4.ShuntingYard
{
    public class PrecedensAssociativity
    {
        public PrecedensAssociativity(int p, Asso a, Func<double, double, double> exec)
        {
            Prec = p;
            Associativity = a;
            Execute = exec;
        }
        public int Prec;
        public enum Asso { Left, Right };
        public Asso Associativity;
        public Func<double, double, double> Execute;
    }
}
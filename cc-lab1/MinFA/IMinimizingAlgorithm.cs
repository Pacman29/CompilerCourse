using System.Collections.Generic;

namespace cc_lab1
{
    public interface IMinimizingAlgorithm : IFAAlgorithm
    {
        void SetDFA(DFA dfa);
    }

}
using System.Collections.Generic;

namespace cc_lab1
{
    public interface IDFABuildAlgorithm : IFAAlgorithm
    {
        void SetNFA(NFA nfa);

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Algorithms.Search;

namespace cc_lab1
{
    public class DFA : FA<Vertex, BaseEdge<Vertex>>
    {
        
        public static DFA FromNFA(NFA nfa, IDFABuildAlgorithm dfaBuildAlgorithm)
        {
            var dfa = new DFA
            {
                Graph = new BidirectionalGraph<Vertex, BaseEdge<Vertex>>(), 
                Tokens = nfa.Tokens
            };

            dfaBuildAlgorithm.SetNFA(nfa);
            dfaBuildAlgorithm.Build();

            dfa.Graph.AddVertexRange(dfaBuildAlgorithm.States);
            dfa.Graph.AddEdgeRange(dfaBuildAlgorithm.Edges);
            
            return dfa;
        }
    }
}
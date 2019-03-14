using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using QuickGraph;

namespace cc_lab1
{
    public class MinFA : FA<Vertex,BaseEdge<Vertex>>
    {   
        public static MinFA FromDFA(DFA dfa, IMinimizingAlgorithm minimizingAlgorithm)
        {
            var minFA = new MinFA()
            {
                Graph = dfa.Graph.Clone(),
                Tokens = dfa.Tokens
            };

            minimizingAlgorithm.SetDFA(dfa);
            minimizingAlgorithm.Build();
            
            minFA.Graph = new BidirectionalGraph<Vertex, BaseEdge<Vertex>>();
            minFA.Graph.AddVertexRange(minimizingAlgorithm.States);
            minFA.Graph.AddEdgeRange(minimizingAlgorithm.Edges);
            
            return minFA;
        }
    }
}
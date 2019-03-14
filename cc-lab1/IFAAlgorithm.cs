using System.Collections.Generic;

namespace cc_lab1
{
    public interface IFAAlgorithm
    {
        List<Vertex> States { get; set; }
        List<BaseEdge<Vertex>> Edges { get; set; }
        
        HashSet<char> Tokens { get; set; }

        void Build();
    }
}
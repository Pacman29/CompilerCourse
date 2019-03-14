using System.Collections.Generic;
using System.Text;

namespace cc_lab1
{
    public class CombineVertex : Vertex
    {
        public HashSet<Vertex> Vertices { get; set; } 
            
        public CombineVertex(HashSet<Vertex> vertices)
        {
            Vertices = vertices;
            foreach (var v in Vertices)
            {
                if (v.IsStart)
                    IsStart = true;
                if (v.IsFinish)
                    IsFinish = true;
            }
                
            States = new HashSet<BaseVertex>();
            foreach (var v in Vertices)
            foreach (var state in v.States)
                States.Add(state);
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            foreach (var v in Vertices)
                res.Append(v + "\n");

            return res.ToString();
        }
    }
}
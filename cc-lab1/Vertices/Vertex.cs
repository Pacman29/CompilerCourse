using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cc_lab1
{
    public class Vertex : BaseVertex
    {
        public HashSet<BaseVertex> States { get; set; }

        public override bool Equals(object obj)
        {
            var v = obj as Vertex;
            return !Object.ReferenceEquals(null, v)
                   && Compare(v);
        }

        public bool Compare(Vertex v)
        {
            return this.IsStart == v.IsStart
                   && this.IsFinish == v.IsFinish
                   && CompareStates(v);
        }

        public bool CompareStates(Vertex v)
        {
            return States.Count == v.States.Count
                   && v.States.All(vstate => States.Contains(vstate));
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("{");
            foreach (var st in States)
                result.Append(st.Title).Append(", ");

            if(result.Length != 1)
                result.Remove(result.Length - 2, 2);
            result.Append("}");
            
            return result.ToString();
        }
    }
}
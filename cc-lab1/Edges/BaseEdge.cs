using System;
using QuickGraph;

namespace cc_lab1
{
    public class BaseEdge<TVertex> : Edge<TVertex> where TVertex : BaseVertex
    {
        public char Tag { get; set; }
            
        public BaseEdge(TVertex source, TVertex target, char tag) : base(source, target)
        {
            Tag = tag;
        }

        public bool Compare(BaseEdge<TVertex> edge)
        {
            return Source.Equals(edge.Source)
                   && Target.Equals(edge.Target)
                   && Tag.Equals(edge.Tag);
        }
    }
}
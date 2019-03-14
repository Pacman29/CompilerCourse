using System;

namespace cc_lab1
{
    public class BaseVertex
    {
        public string Title { get; set; }
            
        public bool IsStart { get; set; } = false;
        public bool IsFinish { get; set; } = false;
            
        public override string ToString()
        {
            return Title;
        }

        public override bool Equals(object obj)
        {
            var v = obj as BaseVertex;
            return !Object.ReferenceEquals(null, v)
                   && String.CompareOrdinal(Title,v.Title) == 0
                   && IsStart == v.IsStart
                   && IsFinish == v.IsFinish;
        }
    }
}
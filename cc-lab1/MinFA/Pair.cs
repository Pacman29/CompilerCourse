using System;

namespace cc_lab1
{
    public class Pair<T>
    {
        public T Value1 { get; set; }
        public T Value2 { get; set; }

        public Pair(T value1, T value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public override string ToString()
        {
            return Value1.ToString() + Value2.ToString();
        }

        public override bool Equals(object obj)
        {
            var v = obj as Pair<T>;
            return !Object.ReferenceEquals(null, v)
                   && Value1.Equals(v.Value1)
                   && Value2.Equals(v.Value2);
        }

        public bool SamePair(Pair<T> obj)
        {
            return Equals(obj) || 
                   (!Object.ReferenceEquals(null, obj)
                    && Value1.Equals(obj.Value2)
                    && Value2.Equals(obj.Value1));
        }
    }
}
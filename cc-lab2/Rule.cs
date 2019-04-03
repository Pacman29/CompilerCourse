using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace cc_lab2
{
    public class Rule : IEquatable<Rule>
    {
        [JsonProperty("right")]
        public List<String> Right { get; set; }
        
        [JsonProperty("left")]
        public String Left { get; set; }

        public bool Equals(Rule other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Right.SequenceEqual(other.Right) && string.Equals(Left, other.Left);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Rule) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Right != null ? Right.Aggregate(new int(), (i, s) => { i += s.GetHashCode();
                    return i;
                } ) : 0) * 397) ^ (Left != null ? Left.GetHashCode() : 0);
            }
        }
    }
}
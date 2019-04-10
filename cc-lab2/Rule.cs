using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace cc_lab2
{
    public class Rule : IEquatable<Rule>, IComparable 
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

        public int CompareTo(object obj)
        {
            return Right.Count - ((Rule) obj).Right.Count;
        }

        public bool HasTerminals(ISet<string> nonTerminals)
        {
            return Right.Any(t => !nonTerminals.Contains(t) && !Grammar.Eps.Equals(t));
        }

        public int NonTerminalsCount(ISet<string> nonTerminals)
        {
            int count = 0;
            foreach (var s in Right)
                if (nonTerminals.Contains(s))
                    count++;

            return count;
        }

        public Rule Copy()
        {
            return new Rule()
            {
                Left = Left,
                Right = Right.ToList()
            };
        }

        public override string ToString()
        {
            return $"{Left} -> {string.Join(" ", Right)}";
        }
    }
}
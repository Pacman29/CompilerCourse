using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace cc_lab2
{
    public class Grammar
    {
        public static String Eps = "eps";

        public Grammar()
        {
            Start = NonTerminals?.ElementAtOrDefault(0);
        }

        [JsonProperty("terminals")]
        public ISet<String> Terminals { get; set; }
        
        [JsonProperty("non_terminals")]
        public ISet<String> NonTerminals { get; set; }
        
        [JsonProperty("start")] 
        public String Start { get; set; }
       
        [JsonProperty("rules")]
        public ISet<Rule> Rules { get; set; }

        public static Grammar LoadFromFile(String path)
        {
            Grammar gr;
            using (StreamReader r = File.OpenText(path))
            {
                string json = r.ReadToEnd();
                gr = JsonConvert.DeserializeObject<Grammar>(json);
            }

            return gr;
        }
        
        public void CreateFile(String path)
        {
            using (StreamWriter w = File.CreateText(path))
            {
                var str = JsonConvert.SerializeObject(this, Formatting.Indented);
                w.Write(str);
                w.Flush();
            }
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            strBuilder.Append("Terminals:\n");
            foreach (var terminal in Terminals)
                strBuilder.Append(terminal + " ");
            
            strBuilder.Append("\n");
            
            strBuilder.Append("Non terminals:\n");
            foreach (var nonterminal in NonTerminals)
                strBuilder.Append(nonterminal + " ");
            
            strBuilder.Append("\n");

            strBuilder.Append("Rules: \n");
            foreach (var nonTerminal in NonTerminals)
                if (Rules.Any(rule => rule.Left.Equals(nonTerminal)))
                    strBuilder.Append(nonTerminal+" -> " + string.Join(" | ", 
                                          Rules.Where((rule => rule.Left.Equals(nonTerminal)))
                                              .Select(rule => String.Join(" ", rule.Right))) + "\n");

            return strBuilder.ToString();
        }

        protected bool Equals(Grammar other)
        {
            return Terminals.SequenceEqual(other.Terminals) 
                   && NonTerminals.SequenceEqual(other.NonTerminals) 
                   && string.Equals(Start, other.Start) 
                   && Rules.SequenceEqual(other.Rules);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Grammar) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Terminals != null ? Terminals.Aggregate(new int(), (i, s) => { i += s.GetHashCode();
                    return i;
                }) : 0);
                hashCode = (hashCode * 397) ^ (NonTerminals != null ? NonTerminals.Aggregate(new int(), (i, s) => { i += s.GetHashCode();
                    return i;
                }) : 0);
                hashCode = (hashCode * 397) ^ (Start != null ? Start.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Rules != null ? Rules.Aggregate(new int(), (i, s) => { i += s.GetHashCode();
                    return i;
                }) : 0);
                return hashCode;
            }
        }
    }
}
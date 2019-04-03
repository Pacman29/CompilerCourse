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
    }
}
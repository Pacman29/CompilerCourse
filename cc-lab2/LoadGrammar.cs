using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace cc_lab2
{
    public class LoadGrammar
    {
        [JsonProperty("terminals")]
        public ISet<String> Terminals { get; set; }
        
        [JsonProperty("non_terminals")]
        public ISet<String> NonTerminals { get; set; }
        
        [JsonProperty("start")] 
        public String Start { get; set; }
        
        [JsonProperty("rules")]
        public ISet<LoadRule> Rules { get; set; }

        public Grammar ToGrammar()
        {
            var grammar = new Grammar()
            {
                Start = Start,
                NonTerminals = NonTerminals,
                Terminals = Terminals,
                Rules = new HashSet<Rule>()
            };
            foreach (var loadRule in Rules)
                grammar.Rules.Add(loadRule.ToRule());

            return grammar;
        }
        
        public static LoadGrammar LoadFromFile(String path)
        {
            LoadGrammar gr;
            using (StreamReader r = File.OpenText(path))
            {
                string json = r.ReadToEnd();
                gr = JsonConvert.DeserializeObject<LoadGrammar>(json);
            }

            return gr;
        }
    }
}
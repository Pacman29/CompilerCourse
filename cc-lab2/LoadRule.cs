using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace cc_lab2
{
    public class LoadRule
    {
        [JsonProperty("right")]
        public String Right { get; set; }
        
        [JsonProperty("left")]
        public String Left { get; set; }

        public Rule ToRule()
        {
            return new Rule()
            {
                Left = Left,
                Right = Right.Split(" ").ToList()
            };
        }
    }
}
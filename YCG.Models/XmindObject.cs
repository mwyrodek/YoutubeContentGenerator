using System.Collections.Generic;
using Newtonsoft.Json;

namespace YCG.Tests.Generator
{
    
    public class Detached    {
        public string title { get; set; } 
        public string id { get; set; } 
        public Children children { get; set; }
    }
    public class Attached    {
        public string id { get; set; } 
        public string title { get; set; } 
        public Children children { get; set; }
    }
    
    public class Children    {
        public List<Detached> detached { get; set; } 
        public List<Attached> attached { get; set; } 
    }

    public class RootTopic    {
        public string id { get; set; } 
        [JsonProperty("class")]
        public string Class { get; set; } 
        public string title { get; set; } 
        public string structureClass { get; set; }
        public Children children { get; set; } 
    }
    
    public class XmindRoot    {
        public string id { get; set; }
        [JsonProperty("class")]
        public string Class { get; set; } 
        public string title { get; set; } 
        public RootTopic rootTopic { get; set; }
    }
    
}
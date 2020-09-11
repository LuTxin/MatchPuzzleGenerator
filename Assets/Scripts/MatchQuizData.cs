using System.Collections.Generic;
using Newtonsoft.Json;

namespace DefaultNamespace
{
    public class MatchQuizData
    {
        [JsonProperty("Matches")]
        public List<MatchButtonData> _matchesInfo;
        
        [JsonProperty("Type")]
        public string _type;

        [JsonProperty("Column")] 
        public int _column;
        
        [JsonProperty("Row")] 
        public int _row;

        [JsonProperty("MatchNum")] 
        public int _matchNumber;
        
        [JsonProperty("HandCapability")] 
        public int _handCapability;
    }
}
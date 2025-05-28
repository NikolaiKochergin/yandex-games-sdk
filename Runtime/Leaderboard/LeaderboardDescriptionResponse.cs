using System;
using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    [Serializable]
    public class LeaderboardDescriptionResponse
    {
        [field: Preserve] 
        public string appID;
        [field: Preserve] 
        public bool @default;
        [field: Preserve]
        public Description description;
        [field: Preserve] 
        public string technicalName;
        [field: Preserve]
        public Title[] title;
        
        [Serializable]
        public class Description
        {
            [field: Preserve]
            public bool invert_sort_order;
            [field: Preserve]
            public ScoreFormat score_format;
            [field: Preserve]
            public string type;
            
            [Serializable]
            public class ScoreFormat
            {
                [field: Preserve]
                public Options options;

                [Serializable]
                public class Options
                {
                    [field: Preserve]
                    public int decimal_offset;
                }
            }
        }
        
        [Serializable]
        public class Title
        {
            [field: Preserve]
            public string locale;
            [field: Preserve]
            public string name;
        }
    }
}
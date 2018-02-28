#r "Newtonsoft.Json"

using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class LUISKnowledgeBase
{
    private static IDictionary<string, string> kb = new Dictionary<string, string>();

	static LUISKnowledgeBase()
	{
        kb.Add("MissedGarbagePickup", "We're sorry to hear that your pickup was missed.  You can report problems with garbage pick up at https://ottawa.ca/en/residents/garbage-and-recycling/garbage#report-problem-garbage-collection");
	}

    public static string Lookup(JObject intents)
    {
        var top = intents["topScoringIntent"];
        float score = (float)top["score"];
        string intentTag = (string)top["intent"];
        if (score > 0.75)
        {
            string topic = null;
            if(kb.TryGetValue(intentTag, out topic))
            {
                return topic;
            }
        }
        return null;
    }
}

#load "GarbageDialog.csx"
#load "EchoDialog.csx"

using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class CustomDialogFactory
{
	static CustomDialogFactory()
	{
	}

    public static IDialog<string> CreateDialog(JObject intents)
    {
        var top = intents["topScoringIntent"];
        float score = (float)top["score"];
        string intentTag = (string)top["intent"];
        if (score < 0.80)
        {
            return null;
        }
        switch(intentTag)
        {
            case "GarbagePickup":
                return (IDialog<string>)(new GarbageDialog());
        }
        return null;
    }
}

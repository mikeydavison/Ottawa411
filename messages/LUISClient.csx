#r "System.Web"
#r "Newtonsoft.Json"

using System;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Bot.Builder.Azure;

public class LUISClient
{

    private static string luisAppId = "d5038322-7264-4ed7-bf6e-cf1bbed5f347";
    private static string subscriptionKey = "5d213e59ecee4c88b2ef5d71e3b14a31";

    static LUISClient()
    {
        luisAppId = Utils.GetAppSetting("LuisAppID");
        subscriptionKey = Utils.GetAppSetting("LuisSubscriptionKey");
    }

	public LUISClient()
	{
	}

    public string AskLuis(string phrase)
    {
        return "Hello, LUIS!";
    }

    public static async Task<JObject> AskLUIS(string phrase)
    {
        var client = new HttpClient();
        var queryString = HttpUtility.ParseQueryString(string.Empty);

        // The request header contains your subscription key
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

        // The "q" parameter contains the utterance to send to LUIS
        queryString["q"] = phrase;

        // These optional request parameters are set to their default values
        queryString["timezoneOffset"] = "0";
        queryString["verbose"] = "false";
        queryString["spellCheck"] = "false";
        queryString["staging"] = "false";

        var uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/" + luisAppId + "?" + queryString;
        var response = await client.GetAsync(uri);

        var strResponseContent = await response.Content.ReadAsStringAsync();
        return JObject.Parse(strResponseContent);
    }
}

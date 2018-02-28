#r "Newtonsoft.Json"
#r "System.Web"

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Bot.Builder.Azure;

public class QnAKnowledgeBase
{

    private static string knowledgebaseId = null;
    private static string qnamakerSubscriptionKey = null;

    static QnAKnowledgeBase()
    {
        qnamakerSubscriptionKey = Utils.GetAppSetting("QnASubscriptionKey");
        knowledgebaseId = Utils.GetAppSetting("QnAKnowledgebaseId");
    }


        private static string Answer(string question)
        {
            string responseString = string.Empty;

            //Build the URI
            Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");
            var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");

            //Add the question as part of the body
            var postBody = $"{{\"question\": \"{question}\"}}";

            //Send the POST request
            using (System.Net.WebClient client = new WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;

                //Add the subscription key header
                client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                client.Headers.Add("Content-Type", "application/json");
                responseString = client.UploadString(builder.Uri, postBody);
            }

            return responseString;
        }

        public static string BestAnswer(string question)
        {
            string answersJson = QnAKnowledgeBase.Answer(question);
            
            try
            {
                JObject o = JObject.Parse(answersJson);
                return HttpUtility.HtmlDecode(o.First.First.ToString());
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
   
}

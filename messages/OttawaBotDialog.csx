#load "LUISClient.csx"
#load "CustomDialogFactory.csx"
#load "LUISKnowledgeBase.csx"
#load "QnAKnowledgeBase.csx"
#r "Newtonsoft.Json"

using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;

// For more information about this template visit http://aka.ms/azurebots-csharp-basic
[Serializable]
public class OttawaBotDialog : IDialog<object>
{
    protected int count = 1;

    public Task StartAsync(IDialogContext context)
    {
        try
        {
            context.Wait(MessageReceivedAsync);
        }
        catch (OperationCanceledException error)
        {
            return Task.FromCanceled(error.CancellationToken);
        }
        catch (Exception error)
        {
            return Task.FromException(error);
        }

        return Task.CompletedTask;
    }

    public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {

        //get the user intent from LUIS.  Serialized as JObject for simplicity, probably should create a POCO
        var message = await argument;
        LUISClient lc = new LUISClient();
        JObject intent = await LUISClient.AskLUIS(message.Text);

        //see if a custom dialog has been created to handle the intent
        IDialog<object> dialog = CustomDialogFactory.CreateDialog(intent);
        if (dialog != null)
        {
            context.Call(dialog, this.AfterComplexDialog);
            return;
        }

        //check if a LUIS KB article exists for the intent
        string kbResponse = LUISKnowledgeBase.Lookup(intent);
        if (kbResponse != null)
        {
            await context.PostAsync(kbResponse);
            return;
        }

        //check if a QnA maker exists
        string qnaResponse = QnAKnowledgeBase.BestAnswer(message.Text);
        if (qnaResponse != null)
        {
            await context.PostAsync(qnaResponse);
            return;
        }


    }

    private async Task AfterComplexDialog(IDialogContext context, IAwaitable<object> argument)
    {
        await context.PostAsync("Is there anything else I can help you with?");
        context.Wait(this.MessageReceivedAsync);
    }

    public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
    {
        var confirm = await argument;
        if (confirm)
        {
            this.count = 1;
            await context.PostAsync("Reset count.");
        }
        else
        {
            await context.PostAsync("Did not reset count.");
        }
        context.Wait(MessageReceivedAsync);
    }
}
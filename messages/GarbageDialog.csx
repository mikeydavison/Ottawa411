using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

[Serializable]
public class GarbageDialog : IDialog<object>
{
	public GarbageDialog()
	{
	}

    public async Task StartAsync(IDialogContext context)
    {
        try
        {
            await context.PostAsync("Garbage pickup?  I can help with that.  Please tell me your postal code.");

            context.Wait(MessageReceivedAsync);
        }
        catch (OperationCanceledException error)
        {
            //return Task.FromCanceled(error.CancellationToken);
        }
        catch (Exception error)
        {
            //return Task.FromException(error);
        }

        //return Task.CompletedTask;
    }

    public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        await context.PostAsync("You can find your pickup schedule at https://ottawa.ca/en/residents/garbage-and-recycling/garbage#garbage-and-recycling-collection-calendar");

        context.Done<object>(null);
    }
}

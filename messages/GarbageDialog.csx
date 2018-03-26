using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading;

[Serializable]
public class GarbageDialog : IDialog<object>
{
	public GarbageDialog()
	{
	}

    public Task StartAsync(IDialogContext context)
    {
        var m = context.MakeMessage();
        m.Text = "Foo";
        context.PostAsync(m, CancellationToken.None);

        //context.PostAsync("Garbage pickup?  I can help with that.  Please tell me your postal code.", CancellationToken.None);

        context.Wait(MessageReceivedAsync);

        return Task.CompletedTask;
    }

    public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        await context.PostAsync("You can find your pickup schedule at https://ottawa.ca/en/residents/garbage-and-recycling/garbage#garbage-and-recycling-collection-calendar");

        context.Done<object>(null);
    }
}

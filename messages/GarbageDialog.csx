using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading;

[Serializable]
public class GarbageDialog : IDialog<string>
{
    public async Task StartAsync(IDialogContext context)
    {
        var m = context.MakeMessage();
        m.Text = "Foo";
        await context.PostAsync(m, CancellationToken.None);

        context.Wait(MessageReceivedAsync);
    }

    public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var m = await argument;

        await context.PostAsync("You can find your pickup schedule at https://ottawa.ca/en/residents/garbage-and-recycling/garbage#garbage-and-recycling-collection-calendar");

        context.Done<object>(null);
    }
}

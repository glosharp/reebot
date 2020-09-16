using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using Reebot.Services;

namespace Reebot.Events
{
    public class MessageEvents
    {

        public MessageEvents()
        {

        }

        public async Task MessageOnReceived(MessageCreateEventArgs e)
        {
            if (!e.Message.Author.IsBot)
            {

            }


        }
    }
}
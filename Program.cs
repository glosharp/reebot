using System;
using System.Threading.Tasks;
using DSharpPlus;

namespace Reebot
{
    class Program
    {
        static DiscordClient _discord;
        
        static void Main(string[] args)
        {
            var reebot = new Startup.Reebot();
            reebot.StartAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}

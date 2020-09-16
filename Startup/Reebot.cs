using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using Reebot.Events;

namespace Reebot.Startup
{
    /// <summary>
    /// Creates a new instance of Reebot
    /// </summary>
    public class Reebot
    { 
        /// <summary>
        /// Starts Reebot
        /// </summary>
        /// <param name="args">CLI arguments</param>
        /// <returns></returns>
        public async Task StartAsync(string[] args)
        {
            var configRegistration = new ConfigRegistration();
            var discord = configRegistration.DiscordClient;
            
            var commands = discord.UseCommandsNext(configRegistration.CommandsNextConfiguration);
            var commandRegistration = new CommandRegistration(commands);
            commandRegistration.RegisterCommands();
            
            var eventRegistration = new EventRegistration(discord, commands);

            await discord.ConnectAsync();
            
            // Set up Reddit Listener
            var redditEvents = new RedditEvents(discord, configRegistration.RedditClient);
            await redditEvents.MonitorMainSub();
            
            await Task.Delay(-1);
            
        }
    }
}
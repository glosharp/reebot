using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Reebot.Events;
using Reebot.Services;

namespace Reebot.Startup
{
    public class EventRegistration
    {
        private readonly DiscordClient _client;
        private readonly DiscordChannel _logChannel;
        private CommandsNextExtension Commands { get; set; }
        
        public EventRegistration(DiscordClient client, CommandsNextExtension commands, DiscordChannel logChannel = null)
        {
            _client = client;
            _logChannel = logChannel;
            Commands = commands;

            EventsInit();
        }

        private Task EventsInit()
        {

            // Set up listeners for Client Events
            var clientEvents = new ClientEvents(_client, _logChannel);
            
            // Set up listeners for Command Events
            var commandEvents = new CommandEvents();

            _client.Ready += clientEvents.ClientOnReady;
            _client.GuildAvailable += clientEvents.ClientOnGuildAvailable;
            _client.ClientErrored += clientEvents.ClientOnError;

            Commands.CommandExecuted += commandEvents.CommandOnExecuted;
            Commands.CommandErrored += commandEvents.CommandOnErrored;

            return Task.CompletedTask;
        }
    }
}
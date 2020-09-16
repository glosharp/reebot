using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Reebot.Events
{
    public class ClientEvents
    {
        private DiscordClient _client;
        private DiscordChannel _logChannel;
        
        public ClientEvents(DiscordClient client, DiscordChannel logChannel)
        {
            _client = client;
            _logChannel = logChannel;
        }

        /// <summary>
        /// Trigger when Client is ready.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public Task ClientOnReady(ReadyEventArgs e)
        {
            // let's log the fact that this event occured
            e.Client.DebugLogger.LogMessage(LogLevel.Info, nameof(Startup.Reebot), 
                "Client is ready to process events.", DateTime.Now);

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        /// <summary>
        /// Trigger when the client errors.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public Task ClientOnError(ClientErrorEventArgs e)
        {
            // let's log the details of the error that just 
            // occured in our client
            e.Client.DebugLogger.LogMessage(LogLevel.Error, nameof(Startup.Reebot), 
                $"Exception occured: {e.Exception.GetType()}: {e.Exception.Message}", DateTime.Now);

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        /// <summary>
        /// Trigger when the client login in to a new Guild.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public Task ClientOnGuildAvailable(GuildCreateEventArgs e)
        {
            // let's log the name of the guild that was just
            // sent to our client
            e.Client.DebugLogger.LogMessage(LogLevel.Info, nameof(Startup.Reebot), 
                $"Guild available: {e.Guild.Name}", DateTime.Now);

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }
    }
}
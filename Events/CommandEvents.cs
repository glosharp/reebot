using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;

namespace Reebot.Events
{
    public class CommandEvents
    {
        
        public CommandEvents()
        {

        }

        /// <summary>
        /// Triggered when a Command is executed.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public Task CommandOnExecuted(CommandExecutionEventArgs e)
        {
            // let's log the name of the command and user
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Info, nameof(Startup.Reebot), 
                $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'", DateTime.Now);

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        /// <summary>
        /// Triggered when a Command Fails
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task CommandOnErrored(CommandErrorEventArgs e)
        {
            // let's log the error details
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Error, nameof(Startup.Reebot), 
                $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' " +
                $"but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

            // let's check if the error is a result of lack
            // of required permissions
            if (e.Exception is ChecksFailedException ex)
            {
                // yes, the user lacks required permissions, 
                // let them know

                var emoji = DiscordEmoji.FromName(e.Context.Client, ":no_entry:");

                // let's wrap the response into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Access denied",
                    Description = $"{emoji} You do not have the permissions required to execute this command.",
                    Color = new DiscordColor(0xFF0000) // red
                    // there are also some pre-defined colors available
                    // as static members of the DiscordColor struct
                };
                await e.Context.RespondAsync("", embed: embed);
            }
        }
    }
}
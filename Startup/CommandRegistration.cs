using DSharpPlus.CommandsNext;
using Reebot.Commands;

namespace Reebot.Startup
{
    public class CommandRegistration
    {
        private CommandsNextExtension _commands;
        /// <summary>
        /// Creates a new instance of Command Registration
        /// </summary>
        public CommandRegistration(CommandsNextExtension commands)
        {
            _commands = commands;
        }

        /// <summary>
        /// Registers Commands
        /// </summary>
        public void RegisterCommands()
        {
            _commands.RegisterCommands<BasicCommands>();
            _commands.RegisterCommands<RedditCommands>();
            _commands.RegisterCommands<RoleCommands>();
        }
        
    }
}
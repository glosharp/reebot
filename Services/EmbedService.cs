using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace Reebot.Services
{
    public static class EmbedService
    {
        /// <summary>
        /// Get the Base Embed.
        /// </summary>
        /// <param name="discord"><see cref="DiscordClient"/></param>
        /// <param name="author">
        /// Optional. Specify this parameter if you want to manually set the Author.
        /// </param>
        /// <param name="ctx">
        /// Optional. Specify this parameter if you want to set the author to the user that
        /// triggered the command. If specified this will override all other author settings.
        /// </param>
        /// <returns></returns>
        public static DiscordEmbedBuilder GetBaseEmbed(DiscordClient discord, DiscordEmbedBuilder.EmbedAuthor author = null, CommandContext ctx = null)
        {
            author ??= new DiscordEmbedBuilder.EmbedAuthor
            {
                Name = discord.CurrentUser.Username,
                IconUrl = discord.CurrentUser.AvatarUrl
            };

            if (ctx != null)
            {
                author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = ctx.Member.Nickname,
                    IconUrl = ctx.Member.AvatarUrl
                };
            }
            
            var embed = new DiscordEmbedBuilder
            {
                Author = author,
                Color = new Optional<DiscordColor>(DiscordColor.Blue),
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "Powered by Glosharp"
                }
            };

            return embed;
        }
    }
}
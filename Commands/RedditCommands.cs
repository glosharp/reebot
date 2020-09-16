using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Reddit;
using Reddit.Inputs.Search;

namespace Reebot.Commands
{
    [Group("reddit")]
    [Description("You basic Reddit commands. This will allow you to reach and get basic information.")]
    public class RedditCommands : BaseCommandModule
    {
        private RedditClient Reddit { get; }
        private DiscordClient Discord { get; }
        
        public RedditCommands(RedditClient reddit, DiscordClient discord)
        {
            Reddit = reddit;
            Discord = discord;
        }

        [Command("ping")]
        [Description("Makes sure I can talk to Reddit.")]
        public async Task TestClient(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var check = await ctx.RespondAsync("Checking to see if I have a connection to Reddit...");
            await check.ModifyAsync($"Logged in as {Reddit.Account.Me.Name.ToUpper()}");

            var embed = new DiscordEmbedBuilder
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = Reddit.Account.Me.Name
                },
                Color = new Optional<DiscordColor>(DiscordColor.Blue)
            };

            embed.AddField("Cake Day!", Reddit.Account.Me.Created.ToString(CultureInfo.InvariantCulture));

            await ctx.RespondAsync(embed: embed);
        }

        [Command("about")]
        [Description("Gets the top hot posts from the sub.")]
        public async Task About(CommandContext ctx, [Description("What sub?")] string sub)
        {
            await ctx.TriggerTypingAsync();
            var interactivity = ctx.Client.GetInteractivity();
            
            var about = Reddit.Subreddit(sub).About();

            var embed = GetBaseEmbed();
            
            foreach (var post in about.Posts.GetHot().Where(post => embed.Fields.Count < 11))
            {
                embed.AddField($"{post.Title}", $"{Formatter.MaskedUrl("Read more...", new Uri($"https://reddit.com{post.Permalink}"))}");
            }

            await ctx.RespondAsync(embed: embed);

        }

        [Command("spy")]
        public async Task Spy(CommandContext ctx, [Description("What reddit user do you want to spy on?")]
            string user)
        {
            await ctx.RespondAsync("Ya creep..hold on getting that for you...");
            await ctx.TriggerTypingAsync();

            var spiedOnUser = Reddit.SearchUsers(new SearchGetSearchInput(user)).First();

            var embed = GetBaseEmbed();

            embed.Description = "Here is what I found.";

            embed.AddField("Name", spiedOnUser.Name);
            embed.AddField("Cake Day", spiedOnUser.Created.ToString("D"));
            embed.AddField("Comment Karma", spiedOnUser.CommentKarma.ToString());
            embed.AddField("Post Karma", spiedOnUser.LinkKarma.ToString());

            await ctx.RespondAsync(embed: embed);
        }

        [Command("creepon")]
        public async Task CreepOn(CommandContext ctx, [Description("What user do you want to creep on?")]
            string user)
        {
            await ctx.RespondAsync("Shit man..hold on...");
            await ctx.TriggerTypingAsync();

            var spiedOnUser = Reddit.SearchUsers(new SearchGetSearchInput(user)).First();

            await ctx.RespondAsync("You're one creepy person...but here");
            
            var embed = GetBaseEmbed();

            foreach (var comment in spiedOnUser.CommentHistory.Where(comment => embed.Fields.Count < 11))
            {
                embed.AddField($"Posted on {comment.Subreddit}", $"{comment.Body}");
            }

            await ctx.RespondAsync(embed: embed);
        }

        /// <summary>
        /// Gets the base embed platform for Reddit.
        /// </summary>
        /// <returns></returns>
        private DiscordEmbedBuilder GetBaseEmbed()
        {
            var embed = new DiscordEmbedBuilder
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = Reddit.Account.Me.Name,
                    IconUrl = Reddit.Account.Me.IconImg
                },
                Color = new Optional<DiscordColor>(DiscordColor.Blue),
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "Powered by glosharp"
                }
            };

            return embed;
        }
    }
}
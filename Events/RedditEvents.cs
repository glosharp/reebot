using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Reddit;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;

namespace Reebot.Events
{
    public class RedditEvents
    {
        private DiscordClient Discord { get; }
        private RedditClient Reddit { get; }

        public List<Comment> NewComments;
        
        public RedditEvents(DiscordClient discord, RedditClient reddit)
        {
            Discord = discord;
            Reddit = reddit;
        }

        public async Task MonitorMainSub()
        {
            Discord.DebugLogger.LogMessage(LogLevel.Debug, $"{nameof(Startup.Reebot)}", "Start monitor", DateTime.Now);
            var sub = Reddit.Subreddit("QuitYourShadowIT");

            sub.Posts.GetNew();
            sub.Posts.MonitorNew();
            sub.Posts.NewUpdated += PostsOnNewUpdated;

            while(true){}

            sub.Comments.MonitorNew();
            sub.Posts.NewUpdated -= PostsOnNewUpdated;

        }

        private async void PostsOnNewUpdated(object? sender, PostsUpdateEventArgs e)
        {
            foreach (var post in e.Added)
            {
                
                Discord.DebugLogger.LogMessage(LogLevel.Debug, $"{nameof(Startup.Reebot)}", $"Detected: {post.Title}", DateTime.Now);

                DiscordEmoji nsfw;

                if (post.NSFW)
                {
                    nsfw = DiscordEmoji.FromName(Discord, ":warning:");
                }
                else
                {
                    nsfw = DiscordEmoji.FromName(Discord, ":white_check_mark:");
                }
                
                var embed = new DiscordEmbedBuilder
                {
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        Name = Reddit.Account.Me.Name,
                        IconUrl = Reddit.Account.Me.IconImg
                    },
                    Description = $"{post.Author} has posted in r/{post.Subreddit.ToLower()}\n" +
                                  $"{Formatter.BlockCode(post.Title)}",
                    Color = new Optional<DiscordColor>(DiscordColor.Blue),
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = "Powered by glosharp"
                    }
                };
                embed.AddField("Wanna read more?", Formatter.MaskedUrl("See more...", new Uri($"https://reddit.com{post.Permalink}")));
                embed.AddField("Is it safe to view at work?", nsfw);

                var channel = await Discord.GetChannelAsync(747611805257302076);
                await Discord.SendMessageAsync(channel: channel, embed: embed);
            }
        }
    }
}
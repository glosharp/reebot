using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Reebot.Services;

namespace Reebot.Commands
{
    public class BasicCommands : BaseCommandModule
    {
        private DiscordClient Discord { get; }

        private int ReeLevel;
        
        public BasicCommands(DiscordClient discord)
        {
            Discord = discord;
        }
        
        [Command("ping")]
        [Description("See the latency of Reebot")]
        [Aliases("pong")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var emoji = DiscordEmoji.FromName(ctx.Client, ":KEKW:");

            await ctx.RespondAsync($"{emoji} Ping: {ctx.Client.Ping}ms");
        }

        [Command("greet"), Description("Say hey to someone."), Aliases("sayhi", "say_hi", "yo")]
        public async Task Greet(CommandContext ctx, [Description("Who do you want to say hey to?")]
            DiscordMember member)
        {
            // Delete the trigger message.
            await ctx.Message.DeleteAsync();
            
            // Show that Reebot is typing.
            await ctx.TriggerTypingAsync();

            // Grab the wave emoji.
            var emoji = DiscordEmoji.FromName(ctx.Client, ":wave:");
            
            // Ping the user.
            await ctx.RespondAsync($"{emoji} Yo, {member.Mention}!");
        }

        [Command("spy"), Description("Spy on what someone is doing."), Aliases("nosey")]
        public async Task Spy(CommandContext ctx, [Description("Who do you want to spy on?")]
            DiscordMember member)
        {
            await ctx.TriggerTypingAsync();

            try
            {
                var game = member.Presence.Activity.Name;
                await ctx.RespondAsync($"{member.DisplayName} is playing {game}");
            }
            catch (Exception ex)
            {
                await ctx.RespondAsync($"{member.DisplayName} is just vibin', not really doing anything.");
            }
        }


        [Command("ree"), Description("Ever get frustrated as shit? Well just reee it out!")]
        public async Task Ree(CommandContext ctx, [Description("Your REE level! Don't go over 100.")] int reeLevel = 0)
        {
            if (reeLevel == 0)
            {
                var rand = new Random().Next(2, 99);

                ReeLevel = rand;
                reeLevel = rand;
            }
            else
            {
                ReeLevel = reeLevel;   
            }
            await ctx.TriggerTypingAsync();

            if (reeLevel > 100)
            {
                await ctx.RespondAsync("Bruh I can't handle a Ree level like that.");
                return;
            }

            if (reeLevel < 1)
            {
                await ctx.RespondAsync("Why the fuck you playing with my emotions? :middle_finger: ");
                ReeLevel = 0;
                return;
            }
            
            var tableFlips = "";
            for (var i = 0; i < reeLevel; i++)
            {
                tableFlips += "\n(╯°□°）╯︵ ┻━┻ ";
            }

            await ctx.RespondAsync("REE LEVEL ACTIVATED\n" +
                                   $"{tableFlips}");
        }

        [Command("unree"), Description("Un-REE the last REE!")]
        public async Task UnRee(CommandContext ctx)
        {
            if (ReeLevel == 0)
            {
                await ctx.TriggerTypingAsync();

                await ctx.RespondAsync("So umm..there's no current Ree level..");
            }
            else
            {

                if (ReeLevel > 100)
                {
                    await ctx.TriggerTypingAsync();
                    await ctx.RespondAsync("What, am I some type of joke to you?!");
                    ReeLevel = 0;
                    return;
                }

                await ctx.TriggerTypingAsync();

                var tableFlips = "";
                for (var i = 0; i < ReeLevel; i++)
                {
                    tableFlips += "\n┬─┬ ノ( ゜-゜ノ) ";
                }

                ReeLevel = 0;

                await ctx.RespondAsync(tableFlips);
            }
        }

        [Command("clap"), Description("Clap a message for those awful fucking time.")]
        public async Task Clap(CommandContext ctx, [RemainingText, Description("What you finna clap?")]string message)
        {
            await ctx.Message.DeleteAsync();
            await ctx.TriggerTypingAsync();
            var clap = DiscordEmoji.FromName(Discord, ":clap:");
            var separators = new string[] {" "};

            var clappedMessage = $"{clap}";
            
            foreach (var word in message.Split(separators, StringSplitOptions.RemoveEmptyEntries))
            {
                clappedMessage = clappedMessage + " " + word + " " + clap;
            }
            
            await ctx.RespondAsync(clappedMessage);
            await ctx.RespondAsync(Formatter.InlineCode($"Per {ctx.Member.Nickname}"));
        }

        [Command("whoami")]
        public async Task WhoAmI(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var embed = EmbedService.GetBaseEmbed(Discord, ctx: ctx);

            var status = ctx.Member.Presence.Activity.CustomStatus != null 
                ? ctx.Member.Presence.Activity.CustomStatus.Name 
                : ctx.Member.Presence.Status.ToString();

            embed.Description = $"Status: {status}\n" +
                                $"ID: {ctx.User.Id.ToString()}\n";

            foreach (var role in ctx.Member.Roles)
            {
                embed.AddField("Role", $"{role.Name}");
            }
            await ctx.RespondAsync(embed: embed);

        }


        [Command("invite")]
        public async Task Invite(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var embed = EmbedService.GetBaseEmbed(Discord, ctx: ctx);

            embed.Title = "Awe, you want me to come with you?\n" +
                          "Let's go!";
            embed.WithThumbnail(new Uri("https://imgflip.com/s/meme/Happy-Guy-Rage-Face.jpg"));
            embed.WithUrl(
                "https://discord.com/oauth2/authorize?client_id=747611613569220660&scope=bot&permissions=268499969");
            await ctx.RespondAsync(embed: embed);
        }

    }
}
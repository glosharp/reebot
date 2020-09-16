using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Reebot.Services;

namespace Reebot.Commands
{
    [Group("role"), Aliases("r", "roles")]
    public class RoleCommands : BaseCommandModule
    {
        [Command("create-roles"), Aliases("init", "set-up")]
        [Description("Creates the base roles that will be used in this group.\n" +
                     "Requires the user and Reebot both have Manage Roles Permission.")]
        [RequirePermissions(Permissions.ManageRoles)]
        public async Task CreateRoles(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var guild = ctx.Guild;
            var createdRoles = new List<DiscordRole>();
            
            // Check to see if the Meeting Role is already created, if not, create it.
            var meetingRole = guild.Roles.Where(x => x.Value.Name == "MEETING");
            if (!meetingRole.Any())
            {
                var newRole = await ctx.Guild.CreateRoleAsync("MEETING", 
                    color: DiscordColor.Yellow, 
                    mentionable: false,
                    hoist: true,
                    reason: "Creating roles per command.");
                
                await newRole.ModifyPositionAsync(19,
                    "This Role should go on top of all others. No special permissions, just visual only.");
                
                createdRoles.Add(newRole);
            }

            // Check to see if the Focused Role is created, if not, create it.
            var focusedRole = guild.Roles.Where(x => x.Value.Name == "FOCUSED");
            if (!focusedRole.Any())
            {
                var newRole = await ctx.Guild.CreateRoleAsync($"FOCUSED" ,
                    color: DiscordColor.White,
                    mentionable: true,
                    hoist: true,
                    reason: "Creating roles per command.");

                await newRole.ModifyPositionAsync(18,
                    "This Role should go on top of all others. No special permissions, just visual only.");
                
                createdRoles.Add(newRole);
                
            }

            var afkRole = guild.Roles.Where(x => x.Value.Name == "AFK");
            if (!afkRole.Any())
            {
                var newRole = await ctx.Guild.CreateRoleAsync("AFK",
                    color: DiscordColor.Goldenrod,
                    mentionable: true,
                    hoist: true,
                    reason: "Going away from keyboard");
                
                await newRole.ModifyPositionAsync(18,
                    "This Role should go on top of all others. No special permissions, just visual only.");
                
                createdRoles.Add(newRole);
            }

            var embed = EmbedService.GetBaseEmbed(discord: ctx.Client, ctx: ctx);

            embed.Title = "Roles Created";
            embed.Description = "```";

            foreach (var role in createdRoles)
            {
                embed.Description += $"\n- {role.Name}";
            }

            embed.Description += "```";
            await ctx.RespondAsync(embed: embed);
        }

        [Command("delete-roles"), Aliases("pew-pew", "remove")]
        [Description("Removes the base roles that will be used in this group.\n" +
                     "Requires the user and Reebot both have Manage Roles Permission.")]
        [RequirePermissions(Permissions.ManageRoles)]
        public async Task RemoveRoles(CommandContext ctx)
        {
            var embed = EmbedService.GetBaseEmbed(ctx.Client, null, ctx);
            embed.Title = "Delete Reebot Roles";
            embed.Description = "Working on deleting the Reebot Roles..." +

            await ctx.RespondAsync(embed: embed);

            var meetingRole = 
                ctx.Guild.Roles.FirstOrDefault(x => x.Value.Name == "MEETING").Value;

            var focusedRole =
                ctx.Guild.Roles.FirstOrDefault(x => x.Value.Name == "FOCUSED").Value;
            
            var afkRole =
                ctx.Guild.Roles.FirstOrDefault(x => x.Value.Name == "AFK").Value;
                
            if (meetingRole != null)
            {
                await meetingRole.DeleteAsync();
            }

            if (focusedRole != null)
            {
                await focusedRole.DeleteAsync();
            }

            if (afkRole != null)
            {
                await afkRole.DeleteAsync();
            }

            embed.Description = "Roles deleted.";

            await ctx.RespondAsync(embed: embed);
        }

        [Command("focus"), Aliases("pingme", "f")]
        [Description("Ever just need to focus? Just run this to show people you need some time to yourself to get " +
                     "work done. To execute this with a short key, you can just use `;;r f` and will be " +
                     "awarded this role.'")]
        public async Task Focused(CommandContext ctx)
        {
            var focusRole = ctx.Guild.Roles.Values.FirstOrDefault(x => x.Name == "FOCUSED");
            var doesUserHaveRole = ctx.Member.Roles.FirstOrDefault(x => x.Id == focusRole?.Id);
            if (doesUserHaveRole != null)
            {
                await ctx.Member.RevokeRoleAsync(focusRole, "Done with the focus.");
                await ctx.RespondAsync("Welcome back!");
                return;
            }

            await ctx.Member.GrantRoleAsync(focusRole, "Needs to focus.");

            await ctx.RespondAsync("Alright, enjoy your focus!");
        }

        [Command("meeting"), Aliases("m")]
        [Description("Let others know that you are in a meeting and should not be pinged unless urgent. This role " +
                     "by default cannot be pinged.")]
        public async Task Meeting(CommandContext ctx)
        {
            var meetingRole = ctx.Guild.Roles.Values.FirstOrDefault(x => x.Name == "MEETING");
            var doesUserHaveRole = ctx.Member.Roles.FirstOrDefault(x => x.Id == meetingRole?.Id);

            if (doesUserHaveRole != null)
            {
                await ctx.Member.RevokeRoleAsync(meetingRole, "Meeting is over.");
                await ctx.RespondAsync("Welcome back, how was the meeting?");
                return;
            }

            await ctx.Member.GrantRoleAsync(meetingRole, "Member is in a meeting.");
            await ctx.RespondAsync("Good luck in your meeting!");
        }

        [Command("afk"), Aliases("away", "brb")]
        [Description("Let other know that you are stepping way from your computer or phone.")]
        public async Task Afk(CommandContext ctx)
        {
            var afkRole = ctx.Guild.Roles.Values.FirstOrDefault(x => x.Name == "AFK");
            var doesUserHaveRole = ctx.Member.Roles.FirstOrDefault(x => x.Id == afkRole?.Id);

            if (doesUserHaveRole != null)
            {
                await ctx.Member.RevokeRoleAsync(afkRole, "Coming back.");
                await ctx.RespondAsync($"{DiscordEmoji.FromName(ctx.Client, ":wave:")} Welcome back!");
                return;
            }

            await ctx.Member.GrantRoleAsync(afkRole, "Going away.");
            await ctx.RespondAsync("Just come back okay?");
        }
    }
}
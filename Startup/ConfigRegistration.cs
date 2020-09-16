using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Reddit;
using Reebot.Events;
using Reebot.Models;
using Reebot.Models.Entities;
using Reebot.Services;

namespace Reebot.Startup
{
    /// <summary>
    /// Creates base global configurations for all of Reebot. Will automatically load all environment variables
    /// that are needed to properly operate.
    /// </summary>
    public class ConfigRegistration
    {
        /// <summary>
        /// <see cref="Settings"/> Instance
        /// </summary>
        public Settings Settings { get; set; }
        
        /// <summary>
        /// <see cref="DiscordConfiguration"/> Instance
        /// </summary>
        public DiscordConfiguration DiscordConfiguration { get; set; }
        
        /// <summary>
        /// <see cref="RedditClient"/> Instance
        /// </summary>
        public RedditClient RedditClient { get; set; }
        
        /// <summary>
        /// <see cref="DiscordClient"/> Instance
        /// </summary>
        public DiscordClient DiscordClient { get; set; }
        
        /// <summary>
        /// <see cref="CommandsNextConfiguration"/> Instance
        /// </summary>
        public CommandsNextConfiguration CommandsNextConfiguration { get; set; }
        
        /// <summary>
        /// <see cref="ServiceCollection"/> Instance
        /// <remarks>
        /// This service collection holds all required service that are needed
        /// for dependency injection with Reebot.
        /// </remarks>
        /// </summary>
        public ServiceProvider ServiceCollection { get; set; }

        public ConfigRegistration()
        {
            LoadSettings();
            LoadDiscordConfiguration();
            LoadRedditConfiguration();
            LoadDiscordClient();
            LoadServiceCollection();
            LoadCommandsNextConfiguration();
        }

        /// <summary>
        /// Create an instance of <see cref="Settings"/>
        /// </summary>
        /// <returns></returns>
        private void LoadSettings()
        {
            Console.WriteLine("Loading Environment variables.");
            var settings = new Settings
            {
                Token = Environment.GetEnvironmentVariable("REEBOT_TOKEN"),
                CommandPrefix = Environment.GetEnvironmentVariable("REEBOT_COMMANDPREFIX"),
                RedditSettings = new RedditSettings
                {
                    AppId = Environment.GetEnvironmentVariable("REEBOT_REDDIT_APP_ID"),
                    RefreshToken = Environment.GetEnvironmentVariable("REEBOT_REDDIT_REFRESH_TOKEN")
                },
                MongoSettings = new MongoSettings
                {
                    Url = Environment.GetEnvironmentVariable("REEBOT_MONGO_URL"),
                    User = Environment.GetEnvironmentVariable("REEBOT_MONGO_USER"),
                    Password = Environment.GetEnvironmentVariable("REEBOT_MONGO_PASSWORD")
                }
            };
            
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("REEBOT_LOGCHANNELID")))
            {
                Console.WriteLine("No Log Channel was set. Not using one!");
            }
            else
            {
                settings.LogChannelId = Convert.ToUInt64(Environment.GetEnvironmentVariable("REEBOT_LOGCHANNELID"));
            }

            Settings = settings;
        }
        
        /// <summary>
        /// Create an instance of <see cref="DiscordConfiguration"/>
        /// </summary>
        /// <returns></returns>
        private void LoadDiscordConfiguration()
        {
            Console.WriteLine("Creating Discord Configuration");
            var config = new DiscordConfiguration
            {
                Token = Settings.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };
            DiscordConfiguration = config;
        }

        /// <summary>
        /// Create an instance of <see cref="RedditClient"/>
        /// </summary>
        /// <returns></returns>
        private void LoadRedditConfiguration()
        {
            Console.WriteLine("Creating Reddit Configuration");
            var reddit = new RedditClient(Settings.RedditSettings.AppId, Settings.RedditSettings.RefreshToken);

            RedditClient = reddit;
        }

        /// <summary>
        /// Create an instance of <see cref="DiscordClient"/>
        /// </summary>
        /// <returns></returns>
        private void LoadDiscordClient()
        {
            Console.WriteLine("Creating Discord Client");
            var discord = new DiscordClient(DiscordConfiguration);

            DiscordClient = discord;
        }
        

        /// <summary>
        /// Configures the Services needed for Dependency Injection.
        /// </summary>
        /// <returns></returns>
        private void LoadServiceCollection()
        {
            Console.WriteLine("Creating Service Collection");
            var services = new ServiceCollection()
                .AddSingleton(RedditClient)
                .AddSingleton(Settings)
                .AddSingleton(DiscordClient)
                .BuildServiceProvider();

            ServiceCollection = services;
        }

        /// <summary>
        /// Configures the <see cref="CommandsNextConfiguration"/> instance
        /// </summary>
        private void LoadCommandsNextConfiguration()
        {
            Console.WriteLine("Creating Command Next Configuration");
            var prefixList = new List<string> {Settings.CommandPrefix};

            CommandsNextConfiguration = new CommandsNextConfiguration
            {
                StringPrefixes = prefixList,
                EnableDms = true,
                EnableMentionPrefix = true,
                Services = ServiceCollection,
                DmHelp = true
            };
        }
        
    }
}
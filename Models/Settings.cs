using Newtonsoft.Json;

namespace Reebot.Models
{
    /// <summary>
    /// Holds data from environment variables.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Bot Token
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
        
        /// <summary>
        /// Command Prefix to trigger bot to listen
        /// </summary>
        public string CommandPrefix { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public ulong LogChannelId { get; set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public RedditSettings RedditSettings { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public MongoSettings MongoSettings { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class RedditSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string AppId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string RefreshToken { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MongoSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string User { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
    }
}
using DSharpPlus.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Reebot.Models.Entities
{
    /// <summary>
    /// Object for Todo
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Todo
    {
        /// <summary>
        /// ID for this object.
        /// </summary>
        [BsonId]
        public ObjectId Id { get; set; }
        
        /// <summary>
        /// Metadata for this object.
        /// </summary>
        public Metadata Metadata { get; set; }
        
        /// <summary>
        /// DiscordUser this belongs to. <see cref="DiscordUser"/>
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// Actual object.
        /// </summary>
        public string Title { get; set; }
        

        public override string ToString()
        {
            return Title;
        }
    }
}
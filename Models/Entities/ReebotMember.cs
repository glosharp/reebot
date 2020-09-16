using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Reebot.Models.Entities
{
    /// <summary>
    /// Represents a Reebot Member
    /// </summary>
    public class ReebotMember
    {
        /// <summary>
        /// ID for this object.
        /// </summary>
        [BsonId]
        public ObjectId Id { get; set; }
        
        /// <summary>
        /// Gets the member's username.
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// Gets the member's discriminator.
        /// </summary>
        public string Discriminator { get; set; }
        
        /// <summary>
        /// Gets the user's nickname.
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Discord User Id
        /// </summary>
        public ulong UserId { get; set; }
        
        /// <summary>
        /// Gets the user's Character Sheet
        /// </summary>
        public ReebotCharacterSheet CharacterSheet { get; set; }
        
        
        /// <summary>
        /// Represents Guilds this Member is in. These guilds are registered in Reebot.
        /// </summary>
        public List<ReebotGuild> Guilds { get; set; }

        /// <summary>
        /// Gets the member's display name.
        /// </summary>
        [BsonIgnore]
        public string DisplayName
            => this.Nickname ?? this.Username;

        /// <summary>
        /// Gets the member's Level.
        /// </summary>
        [BsonIgnore]
        public long Level
            => this.CharacterSheet.Level;
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Reebot.Models.Entities
{
    public class ReebotGuild
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        public long DiscordId { get; set; }
        
        public string Name { get; set; }
        
    }
}
namespace Reebot.Models.Entities
{
    /// <summary>
    /// Represents a member's Character Sheet.
    /// </summary>
    public class ReebotCharacterSheet
    {
        /// <summary>
        /// Member's experience points.
        /// </summary>
        public long XP { get; set; }
        
        /// <summary>
        /// Member's level.
        /// </summary>
        public long Level { get; set; }
    }
}
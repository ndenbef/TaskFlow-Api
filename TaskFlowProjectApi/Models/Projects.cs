using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace TaskFlowProjectApi.Models
{
    public class Projects
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.Empty;

        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime Deadline { get; set; }
        //[BsonRepresentation(BsonType.String)]
        public string TaskIds { get; set; } = string.Empty;
        public required string AllowedHosts { get; set; }
    }
}

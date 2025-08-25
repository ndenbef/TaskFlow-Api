using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TaskFlowTaskApi.Models
{
    public class Task
    {
        [BsonId]
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime Deadline { get; set; }
        public string? Tags { get; set; }
        public string? AllowedHosts { get; set; }
    }
}

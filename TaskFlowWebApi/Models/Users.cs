using AspNetCore.Identity.MongoDbCore.Models;

namespace TaskFlowWebApi.Models
{
    public class Users: MongoIdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        //public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastMod { get; set; } = DateTime.UtcNow;
        public string mode { get; set; } = string.Empty;
        public string? ProfileUrl { get; set; } = string.Empty;
        public string pushtoken { get; set; } = string.Empty;
        public string push { get; set; } = string.Empty;

        public Users(): base()
        {

        }

        public Users( string userName, string password): base(userName, password)
        {

        }
    }
    
}

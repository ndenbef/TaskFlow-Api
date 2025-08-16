using AspNetCore.Identity.MongoDbCore.Models;

namespace TaskFlowWebApi.Models
{
    public class UserRoles: MongoIdentityRole<Guid>
    {
        public UserRoles() : base()
        {

        }

        public UserRoles(string roleName): base(roleName)
        {

        }
    }
}

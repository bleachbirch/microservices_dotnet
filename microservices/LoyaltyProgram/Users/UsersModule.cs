using Nancy;
using Nancy.ModelBinding;

namespace LoyaltyProgram.Users
{
    public class UsersModule: NancyModule
    {
        private IDictionary<int, LoyaltyProgramUser> _registeredUsers = new Dictionary<int, LoyaltyProgramUser>();
        public UsersModule(): base("/users")
        {
            Post("/", _ =>
            {
                var newUser = this.Bind<LoyaltyProgramUser>();
                AddRegisteredUser(newUser);
                return CreatedResponse(newUser);
            });

            Put("/{userId:int}", parameters =>
            {
                int userId = parameters.userId;
                var updatedUser = this.Bind<LoyaltyProgramUser>();
                //store the updated user to a data store
                return updatedUser;
            });

            Get("/{userId:int}", parameters =>
            {
                int userId = parameters.userId;
                return _registeredUsers.ContainsKey(userId) ? _registeredUsers[userId] : HttpStatusCode.NotFound;
            });
        }

        private dynamic CreatedResponse(LoyaltyProgramUser newUser)
        {
            return Negotiate.WithStatusCode(HttpStatusCode.Created)
                .WithHeader("Location", Request.Url.SiteBase + "/users/" + newUser.Id)
                .WithModel(newUser);
        }

        private void AddRegisteredUser(LoyaltyProgramUser newUser)
        {
            //store thw newUSer to a data store
        }
    }
}

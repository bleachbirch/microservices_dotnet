using Nancy;
using Nancy.ModelBinding;

namespace LoyaltyProgram.Users
{
    public class UsersModule: NancyModule
    {
        private IDictionary<int, LoyaltyProgramUser> _registeredUsers = new Dictionary<int, LoyaltyProgramUser>();
        public UsersModule(): base("/users")
        {
            Post("/", parameters =>
            {
                var userId = parameters["userId"];
                if (!_registeredUsers.ContainsKey(userId))
                {
                    return HttpStatusCode.Forbidden;
                }
                var newUser = this.Bind<LoyaltyProgramUser>();
                AddRegisteredUser(newUser);
                return CreatedResponse(newUser);
            });

            Put("/{userId:int}", parameters =>
            {
                int.TryParse(Context.CurrentUser.Claims
                    .FirstOrDefault(claim => claim.Type.StartsWith("id"))?.Value.Split(':').Last() ?? string.Empty, 
                    out int loggedInUserId);
                int userId = parameters.userId;
                if(loggedInUserId != userId)
                {
                    return HttpStatusCode.Forbidden;
                }

                var updatedUser = this.Bind<LoyaltyProgramUser>();
                _registeredUsers[userId] = updatedUser;
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

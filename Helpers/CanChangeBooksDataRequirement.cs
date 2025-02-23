using Microsoft.AspNetCore.Authorization;

namespace BibleExplanationControllers.Helpers
{
    public class CanChangeBooksDataRequirement : IAuthorizationRequirement { }

    public class CanChangeBooksDataHandler : AuthorizationHandler<CanChangeBooksDataRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanChangeBooksDataRequirement requirement)
        {
            var user = context.User;

            if (user.IsInRole("Admin") ||
                (user.IsInRole("SubAdmin") && user.HasClaim("CanChangeBooksData", "true")) ||
                (user.IsInRole("Worker") && user.HasClaim("CanChangeBooksData", "true")))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

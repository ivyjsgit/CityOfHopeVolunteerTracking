using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHO.Security
{
    public class RequireAdminHandler :AuthorizationHandler<RequireAdmin>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireAdmin requirement)
        {
           // if (context. )
          //  {
                context.Succeed(requirement);
          //  }
            return Task.CompletedTask;
        }
    }
}

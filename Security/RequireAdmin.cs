using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHO.Security
{
    public class RequireAdmin :IAuthorizationRequirement
    {
    }
}

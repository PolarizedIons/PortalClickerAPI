using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PortalClickerApi.Exceptions;

namespace PortalClickerApi.Extentions
{
    public static class IdentityResultExtentions
    {
        public static async Task<IdentityResult> ThrowIfNotSucceeded(this Task<IdentityResult> identityResult, string message)
        {
            var result = await identityResult;
            if (!result.Succeeded)
            {
                throw new BadRequestException(message + ": " + string.Join(", ", result.Errors.Select(x => x.Description)));
            }

            return result;
        }
    }
}

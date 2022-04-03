using System.Security.Claims;

namespace EconomicManagementAPP.Services
{
    public class UserServices : IUserServices
    {
        private readonly HttpContext httpContext;
        public UserServices(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        public int GetUserId()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idClaim.Value);
                return id;
            }
            else
            {
                throw new ApplicationException("The User is not authentificated");
            }
        }
    }
}

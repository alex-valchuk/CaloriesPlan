using System.Threading.Tasks;

using Microsoft.Owin.Security;

namespace CaloriesPlan.BLL.Services
{
    public interface IOAuthService
    {
        Task<AuthenticationTicket> GetAuthenticationTicket(string userName, string password, string authType);
    }
}

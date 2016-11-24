using System.Threading.Tasks;

using Microsoft.Owin.Security;

namespace CaloriesPlan.BLL.Services
{
    public interface IOAuthService
    {
        Task<AuthenticationTicket> SignIn(string userName, string password, string authType);
    }
}

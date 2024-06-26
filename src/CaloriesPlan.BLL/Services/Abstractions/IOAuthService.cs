﻿using System.Threading.Tasks;

using Microsoft.Owin.Security;

namespace CaloriesPlan.BLL.Services.Abstractions
{
    public interface IOAuthService
    {
        Task<AuthenticationTicket> SignInAsync(string userName, string password, string authType);
    }
}

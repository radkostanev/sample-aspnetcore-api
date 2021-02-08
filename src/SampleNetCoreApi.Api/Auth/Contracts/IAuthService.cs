using SampleNetCoreApi.Data.Entities;
using SampleNetCoreApi.Api.Models;
using System.Collections.Generic;

namespace SampleNetCoreApi.Api.Auth.Contracts
{
    public interface IAuthService
    {
        AuthenticateResponse Register(RegisterRequest model);
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}

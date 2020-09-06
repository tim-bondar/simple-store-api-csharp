using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleStore.DB;
using SimpleStore.Models;

namespace SimpleStore.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(Guid id);
        Task<User> GetCurrent();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExoWebApi.DAL.Contracts
{
    public interface IUserRepository
    {
        Task<bool> Authentificate(string mail, String password);
        Task<bool> Confidentials(string mail);
    }
}

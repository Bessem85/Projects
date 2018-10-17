using ExoWebApi.DAL.Contracts;
using ExoWebApi.DAL.Data;
using ExoWebApi.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExoWebApi.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static List<User> _users;
        public UserRepository()
        {
            _users = UserData.Users;
        }

        /// <summary>
        /// Authentificate by mail and password
        /// </summary>
        /// <param name="email">Required user email</param>
        /// <param name="password">Required user password</param>
        /// <returns>bool</returns>
        public async Task<bool> Authentificate(string email, string password)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(password))
            {
                var user = _users.FirstOrDefault(_ => _.Email.Equals(email) && _.Password.Equals(password));
                if (user != null)
                    result = true;
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        ///  Check Confidential by mail
        /// </summary>
        /// <param name="email">Required user email</param>
        /// <returns></returns>
        public async Task<bool> Confidentials(string email)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(email))
            {
                var user = _users.FirstOrDefault(_ => _.Email.Equals(email));
                if (user != null)
                    result = true;
            }
            return await Task.FromResult(result);
        }
    }
}

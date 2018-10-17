using ExoWebApi.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExoWebApi.DAL.Data
{
    public static class UserData
    {
        public static List<User> Users => new List<User>
            {
                new User {Login="user1",Password="azerty111",Email="user1@webapi.fr" },
                new User {Login="user2",Password="azerty222",Email="user2@webapi.fr" },
                new User {Login="user3",Password="azerty333",Email="user3@webapi.fr" },
                new User {Login="user4",Password="azerty444",Email="user4@webapi.fr" },
             };

    }
}


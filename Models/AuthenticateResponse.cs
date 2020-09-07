using System;
using System.Text.Json.Serialization;
using SimpleStore.DB;

namespace SimpleStore.Models
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(User user, string token)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.UserName;
            IsAdmin = user.IsAdmin;
            Token = token;
        }

        public string FirstName { get;  }
        public string LastName { get;  }
        public string Username { get;  }
        public bool IsAdmin { get;  }
        public string Token { get;  }
    }
}

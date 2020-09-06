using System;
using SimpleStore.DB;

namespace SimpleStore.Models
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.UserName;
            IsAdmin = user.IsAdmin;
            Token = token;
        }

        public Guid Id { get; }
        public string FirstName { get;  }
        public string LastName { get;  }
        public string Username { get;  }
        public bool IsAdmin { get;  }
        public string Token { get;  }
    }
}

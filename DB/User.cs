using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SimpleStore.DB
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("lastname")]
        public string LastName { get; set; }
        [Column("username")]
        public string UserName { get; set; }
        public string FullName => FirstName + " " + LastName;
        [Column("isadmin")]
        public bool IsAdmin { get; set; }

        [JsonIgnore]
        [Column("passwordhash")]
        public string PasswordHash { get; set; }
    }
}

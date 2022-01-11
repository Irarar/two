using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }
    }
}
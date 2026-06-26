using Microsoft.AspNetCore.Identity;

namespace PetCare.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }  
        public string LastName { get; set; }  
        public string FullName => $"{FirstName} {LastName}";
    }
}
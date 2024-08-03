using System.ComponentModel.DataAnnotations;

namespace P2Api.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [MinLength(3)]
        public string username { get; set; } = String.Empty;
        [Required]
        [MinLength(3)]
        public string password { get; set; } = String.Empty;
    }
}

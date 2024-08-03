using System.ComponentModel.DataAnnotations;

namespace P2Api.ViewModels
{
    public class TarefaCreateViewModel
    {
        [Required]
        [MinLength(5)]
        public string title { get; set; } = String.Empty;
        public string description { get; set; } = String.Empty;
        
    }
    public class TarefaStatusViewModel
    {
        [Required]
        public string status { get; set; } = String.Empty;

    }
}

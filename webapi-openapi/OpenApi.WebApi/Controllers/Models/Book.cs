using System.ComponentModel.DataAnnotations;

namespace OpenApi.WebApi.Controllers.Models;

public class Book
{
    [Required(ErrorMessage = "The book name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "The number of pages is required")]
    [Range(1, int.MaxValue, ErrorMessage = "The number of pages must be greater than 0")]
    public int Pages { get; set; }
}



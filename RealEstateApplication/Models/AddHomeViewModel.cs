using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BSR;

public class AddHomeViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Address field is required.")]
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }   
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int GarageSpots { get; set; }

    [Required(ErrorMessage = "The Price field is required.")]
    [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "The Area field is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Area must be a positive value.")]
    public int Area { get; set; }

    [ValidateNever]
    public string ImageUrl { get; set; }
    public List<string> States { get; set; }
    public List<string> Cities { get; set; }
}

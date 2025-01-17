using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.BL.VM.Trainer;
public class TrainerUpdateVM
{
    [Required(ErrorMessage = "Fullname is required"), MaxLength(64, ErrorMessage = "Trainer Fullname length must be less than 64 charachters")]
    public string Fullname { get; set; }
    [Required(ErrorMessage = "Description is required"), MaxLength(64, ErrorMessage = "Trainer Description length must be less than 256 charachters")]
    public string Description { get; set; }
    public string ExistingImageUrl { get; set; }
    public IFormFile? Image { get; set; }
    public int CategoryId { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.BL.VM.Category;
public class CategoryCrateVM
{
    [Required(ErrorMessage = "Name is required"), MaxLength(64, ErrorMessage = "Catregory Name length must be less than 64 charachters")]
    public string Name { get; set; }    
}

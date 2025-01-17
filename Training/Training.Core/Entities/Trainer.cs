using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Core.Entities.Base;

namespace Training.Core.Entities;
public class Trainer : BaseEntity
{
    public string Fullname { get; set; }    
    public string Description { get; set; }
    public string ImageUrl { get; set; }    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}

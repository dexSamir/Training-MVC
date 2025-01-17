using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Core.Entities.Base;

namespace Training.Core.Entities;
public class Category : BaseEntity
{
    public string Name { get; set; }    
    public ICollection<Trainer> Trainers { get; set;} = new HashSet<Trainer>();
}

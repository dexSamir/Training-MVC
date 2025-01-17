using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Core.Entities;

namespace Training.DAL.Context;
public class AppdbContext : IdentityDbContext<User>
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public AppdbContext(DbContextOptions opt) : base(opt)
    {

    }
}
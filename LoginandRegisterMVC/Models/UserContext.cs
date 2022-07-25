using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoginandRegisterMVC.Models
{
    public class UserContext:DbContext
    {
        public UserContext() : base("Name=Dbconfig")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<VotedUser> VotedUsers { get; set; }

        public DbSet<CheckBox> CheckBoxes { get; set; }


    }
}
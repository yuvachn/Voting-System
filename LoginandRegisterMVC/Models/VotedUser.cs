using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginandRegisterMVC.Models
{
    public class VotedUser
    {
        [Key]
        public int EmpId { get; set; }

        public int ElectionId { get; set; }
    }
}
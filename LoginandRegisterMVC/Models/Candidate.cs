using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginandRegisterMVC.Models
{
    public class Candidate
    {
        public int CandidateId { get; set; }
        public int ElectionId { get; set; }
        public Election Election { get; set; }
        public int EmployeeId { get; set; }
        public User User { get; set; }

        public int Votes { get; set; } = 0;

    }
}
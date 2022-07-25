using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Security;

namespace LoginandRegisterMVC.Models
{
    public class Election
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name = "Election Id")]
        public int ElectionId { get; set; }

        [Required]
        [Display(Name = "Election Title")]
        public string ElectionTitle { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Service Line")]
        public string ServiceLine { get; set; }

        public bool ADM { get; set; }

        public bool QEA { get; set; }

        public bool MDU { get; set; }

        public bool CSD { get; set; }

        public ICollection<Candidate> Candidates { get; set; }

        public List<CheckBox> ServiceLines { get; set; }

    }

    public class CheckBox
    {
        [Key]
        public int Value { get; set; }

        public string Text { get; set; }

        public bool IsChecked { get; set; }
    }



}
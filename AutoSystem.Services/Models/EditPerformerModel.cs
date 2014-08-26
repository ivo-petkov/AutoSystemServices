using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSystem.Services.Models
{
    public class EditPerformerModel
    {
        public int PerformerId { get; set; }
        public string Username { get; set; }
        public string OldAuthCode { get; set; }
        public string NewAuthCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
    }
}
using System;
using System.Linq;

namespace AutoSystem.Services.Models
{
    public class PerformerModel
    {
        public int PerformerId { get; set; }
        public string Username { get; set; }
        public string AuthCode { get; set; }
        public string SessionKey { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
    }
}
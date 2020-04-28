using AsqMe.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsqMe.Data.Models
{
    public class Earning : IEntity
    {
        public enum EarningType
        {
            Sent,
            Received
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public EarningType EarningsType { get; set; }
        public decimal Amount { get; set; }
        public string SenderId { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}

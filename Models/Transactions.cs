using System;

namespace WebAPIServiceII.Models
{
    public class Transactions
    {
       // public int Tid { get; set; }
        public int AccountNumber_Debited { get; set; }
        public int AccountNumber_Credited { get; set; }
        public int Amount { get; set; }
        public string TransactionNumber { get; set; }
        public string Date { get; set; }
    }
}
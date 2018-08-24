using System;

namespace WebAPIServiceII.Models
{
    public class AmountTransfer
    {
        public string Amount { get; set; }
        public string Account_No { get; set; }
        public string IFSC { get; set; }
        public string DOT { get; set; }
    }
}
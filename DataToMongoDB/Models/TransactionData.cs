﻿namespace DataToMongoDB.Models
{
    public class TransactionData
    {
        public string from { get; set; }
        public string to { get; set; }
        public string hash { get; set; }
        public string value { get; set; }
        public string gas { get; set; }
        public string gasPrice { get; set; }
        public string input { get; set; }
    }
}

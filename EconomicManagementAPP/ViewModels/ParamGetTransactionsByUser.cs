﻿namespace EconomicManagementAPP.Models
{
    public class ParamGetTransactionsByUser
    {
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

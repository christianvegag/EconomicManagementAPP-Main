namespace EconomicManagementAPP.Models
{
    public class ReportTransactionsDetails
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<OperationType> OperationTypes { get; set; }

        public IEnumerable<TransactionsByDate> TransactionsGrouped { get; set; }

        public decimal BalanceDeposits => TransactionsGrouped.Sum(x=> x.BalanceDeposits);

        public decimal BalanceWithdrawals => TransactionsGrouped.Sum(x=> x.BalanceWithdrawals);

        public decimal Total => BalanceDeposits - BalanceWithdrawals;

        public class TransactionsByDate
        {
            public DateTime TransactionDate { get; set; }
            public IEnumerable<Transaction> Transactions { get; set; }

            public decimal BalanceDeposits =>
                Transactions.Where(x => x.OperationType == "Income")
                .Sum(x => x.Total);

            public decimal BalanceWithdrawals =>
                Transactions.Where(x => x.OperationType == "Expense" )
                .Sum(x => x.Total);
        }
    }
}

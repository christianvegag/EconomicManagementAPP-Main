using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public class ReportServices : IReportServices
    {
        private readonly IRepositorieTransactions repositorieTransactions;
        private readonly HttpContext httpContext;

        public ReportServices(IRepositorieTransactions repositorieTransactions, IHttpContextAccessor httpContextAccessor)
        {
            this.repositorieTransactions = repositorieTransactions;
            this.httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<ReportTransactionsDetails> GetReportTransactionsDetailed(int userId, int month, int year, dynamic ViewBag)
        {
            (DateTime startDate, DateTime endDate) = GenerateDateStartAndEnd(month, year);

            var param = new ParamGetTransactionsByUser()
            {
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate
            };

            var transactions = await repositorieTransactions.GetByUserId(param);
            var model = await GenerateReportTransactionsDetailed(startDate, endDate, transactions);

            AssignValuesToViewbag(ViewBag, startDate);
            return model;
        }
        public async Task<ReportTransactionsDetails> GetReportTransactionsDetailedByAccount(int userId, int accountId, int month, int year, dynamic ViewBag)
        {
            (DateTime startDate, DateTime endDate) = GenerateDateStartAndEnd(month, year);

            var getTransactionsByAccount = new ParamGetTransactionsByAccount()
            {
                AccountId = accountId,
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate,
            };

            var transactions = await repositorieTransactions.GetByAccountId(getTransactionsByAccount);

            var model = await GenerateReportTransactionsDetailed(startDate, endDate, transactions);

            AssignValuesToViewbag(ViewBag, startDate);

            return model;

        }

        private void AssignValuesToViewbag(dynamic ViewBag, DateTime startDate)
        {
            ViewBag.previousMonth = startDate.AddMonths(-1).Month;
            ViewBag.previousYear = startDate.AddMonths(-1).Year;
            ViewBag.laterMonth = startDate.AddMonths(1).Month;
            ViewBag.laterYear = startDate.AddMonths(1).Year;
            ViewBag.urlReturn = httpContext.Request.Path + httpContext.Request.QueryString;
        }

        private async Task<ReportTransactionsDetails> GenerateReportTransactionsDetailed(DateTime startDate, DateTime endDate, IEnumerable<Transaction> transactions)
        {

            var model = new ReportTransactionsDetails();

            var transactionsByDate = transactions.OrderByDescending(x => x.TransactionDate)
                .GroupBy(x => x.TransactionDate)
                .Select(group => new ReportTransactionsDetails.TransactionsByDate()
                {
                    TransactionDate = group.Key,
                    Transactions = group.AsEnumerable()
                });

            model.TransactionsGrouped = transactionsByDate;
            model.StartDate = startDate;
            model.EndDate = endDate;
            return model;
        }

        private (DateTime startDate, DateTime endDate) GenerateDateStartAndEnd(int month, int year)
        {
            DateTime startDate;
            DateTime endDate;

            if (month <= 0 || month > 12 || year <= 1900)
            {
                var today = DateTime.Today;
                startDate = new DateTime(today.Year, today.Month, 1);
            }
            else
            {
                startDate = new DateTime(year, month, 1);
            }

            endDate = startDate.AddMonths(1).AddDays(-1);

            return (startDate, endDate);
        }
    }
}

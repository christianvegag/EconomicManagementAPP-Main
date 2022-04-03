using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public interface IReportServices
    {
        Task<ReportTransactionsDetails> GetReportTransactionsDetailed(int userId, int month, int year, dynamic ViewBag);
        Task<ReportTransactionsDetails> GetReportTransactionsDetailedByAccount(int userId, int accountId, int month, int year, dynamic ViewBag);
    }
}
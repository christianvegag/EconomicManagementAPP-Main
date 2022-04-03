namespace EconomicManagementAPP.Models
{
    public class TransactionUpdateViewModel : TransactionViewModel
    {
        public int PreviousAccountId { get; set; }
        public decimal PreviousTotal { get; set; }
        public string UrlReturn { get; set; }
    }
}

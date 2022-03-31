namespace EconomicManagementAPP.Models
{
    public class IndexAccountViewModel
    {
        public string AccountTypes { get; set; }
        public IEnumerable<Account> Accounts { get; set; }

        public decimal Balance => Accounts.Sum(x => x.Balance);
    }
}

using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Transaction Date")]
        [DataType(DataType.DateTime)]
        public DateTime TransactionDate { get; set; } = DateTime.Parse(DateTime.Now.ToString("g"));
        public decimal Total { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage = "The description cannot exceed {1} characters")]
        public string Description { get; set; }

        [Range(1, maximum:int.MaxValue, ErrorMessage = "Select at least one category")]
        [Display(Name = "Account")]
        public int AccountId { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Select at least one category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "OperationType")]
        public int OperationTypeId { get; set; } = 1;

        public string OperationType { get; set; }

        public string Account {get; set; }

        public string Category { get; set; }
    }
}

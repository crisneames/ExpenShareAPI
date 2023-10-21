using System;
namespace ExpenShareAPI.Models
{
	public class Expense
	{
		public int Id {get; set;}
		public string Name { get; set; }
		public decimal Amount { get; set; }
		public string Comment { get; set; }
		public int EventId { get; set; }
	}
}


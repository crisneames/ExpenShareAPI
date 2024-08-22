using System;
namespace ExpenShareAPI.Models
{
	public class Event
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public string? Comment { get; set; }
        public List<User> User { get; set; }
		public List<Expense> Expense { get; set; }
    }


	public class UserEvent
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int EventId { get; set; }
	}

	public class UserEventExpenseDto
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public int EventId { get; set; }
		public string EventName { get; set; }
		public decimal TotalExpenseAmount { get; set; }
		public int NumberOfUsers { get; set; }
		public decimal UserExpensePortion { get; set; }
	}

	public class EventWithUsersDto
	{
		public int EventId { get; set; }
		public string EventName { get; set; }
		public DateTime EventDate { get; set; }
		public string EventComment { get; set; }
		public List<User> Users { get; set; }
	}
}


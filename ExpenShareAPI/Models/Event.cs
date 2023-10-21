using System;
namespace ExpenShareAPI.Models
{
	public class Event
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public string Comment { get; set; }
		public int UserId { get; set; }
	}
}


namespace data.raspberry.awards.Entities.Base
{
	using System;
	public class BaseEntity
	{
		public DateTime? InsertDate { get; set; }
		public long? UserInsert { get; set; }
		public DateTime? UpdateDate { get; set; }
		public long? UserUpdate { get; set; }
	}
}

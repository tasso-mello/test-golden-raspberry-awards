namespace common.raspberry.awards.Models.Base
{
    using System;

    public class BaseModel
    {
        public DateTime? InsertDate { get; set; }
        public long? UserInsert { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? UserUpdate { get; set; }
    }
}

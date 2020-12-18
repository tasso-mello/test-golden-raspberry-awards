namespace data.raspberry.awards.Entities
{
    using Base;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Movie")]
    public class Movie : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Studio { get; set; }
        public string Producers { get; set; }
        public bool Winner { get; set; }
    }
}

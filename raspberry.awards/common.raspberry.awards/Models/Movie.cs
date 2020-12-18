namespace common.raspberry.awards.Models
{
    using common.raspberry.awards.Models.Base;

    public class Movie: BaseModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Studio { get; set; }
        public string Producers { get; set; }
        public bool Winner { get; set; }
    }
}

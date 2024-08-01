namespace ServiceShared.Models
{
    public class InsertFeedbackRequestModel
    {
        public int ParentId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Content { get; set; }
    }
}

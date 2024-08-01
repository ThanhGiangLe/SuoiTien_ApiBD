namespace ServiceShared.Models
{
    public class GetCommentDB
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Content { get; set; }
        public DateTime Time { get; set; }
        public int ParentID { get; set; }
        public int LikeC { get; set; }
        public int DislikeC { get; set; }
    }
}

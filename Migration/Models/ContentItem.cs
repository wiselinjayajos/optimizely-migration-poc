namespace CMSDemo.Migration.Models
{
    public class ContentItem
    {
        public int ContentId { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string ContentData { get; set; }
        public int ParentContentId { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string TeaserImageUrl { get; set; }
    }
}

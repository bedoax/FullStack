namespace Models.DTOs.Topic
{
    public class TopicUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

}

namespace Models.DTOs.Option
{
    public class OptionUpdateDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}

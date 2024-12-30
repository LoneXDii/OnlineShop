namespace SupportService.Application.DTO;

public class MessageDTO
{
    public string SenderId { get; set; }
    public string Text { get; set; }
    public DateTime DateTime { get; set; }
    public string ChatOwnerId { get; set; }
}

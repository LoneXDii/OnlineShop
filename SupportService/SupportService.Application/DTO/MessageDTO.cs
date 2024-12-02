namespace SupportService.Application.DTO;

internal class MessageDTO
{
    public int SenderId { get; set; }
    public string Text { get; set; }
    public DateTime DateTime { get; set; }
}

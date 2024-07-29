namespace Iot.Database.FileManager;

public class FileAccessLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FileId { get; set; }
    public string UserName { get; set; }
    public DateTime AccessTime { get; set; }
    public FileOperation Operation { get; set; }
}

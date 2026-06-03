namespace API.Services.Options;
public class EmailOptions
{
    public required string SmtpServer { get; set; }
    public int Port { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
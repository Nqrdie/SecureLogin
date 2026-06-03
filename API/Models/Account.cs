namespace API.Models;

public class Account
{
    public int Id {get; set;}
    public required string Name {get; set;}
    public required string Email {get; set;}
    public required string Password {get; set;}

    public bool EmailVerified {get; set;} = false;
    public string? EmailVerificationToken {get; set;}
}

public class LoginRequest
{
    public required string Email {get; set;}
    public required string Password {get; set;}
}

public class RegisterRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
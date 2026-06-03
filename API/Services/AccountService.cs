using API.Data;
using API.Models;


namespace API.Services;

public class AccountService
{
    private readonly AppDbContext _context;

    public AccountService(AppDbContext context)
    {
        _context = context;
    }

    public List<Account> GetAll()
    {
        return _context.Users.ToList();
    }

    public Account? Get(int id)
    {
        return _context.Users.FirstOrDefault(a => a.Id == id);
    }

    public void AddAccount(Account account)
    {
        _context.Users.Add(account);

        _context.SaveChanges();
    }

    public void DeleteAccount(int id)
    {
        var account = Get(id);

        if (account is null)
            return;

        _context.Users.Remove(account);

        _context.SaveChanges();
    }

    public void UpdateAccount(Account updatedAccount)
    {
        var existingAccount = Get(updatedAccount.Id);

        if (existingAccount is null)
            return;

        existingAccount.Name = updatedAccount.Name;
        existingAccount.Email = updatedAccount.Email;
        existingAccount.Password = updatedAccount.Password;
        existingAccount.EmailVerified = updatedAccount.EmailVerified;
        existingAccount.EmailVerificationToken = updatedAccount.EmailVerificationToken;

        _context.SaveChanges();
    }
}
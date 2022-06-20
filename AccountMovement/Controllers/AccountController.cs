using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AccountMovement.Entities;
using AccountMovement.Interfaces;

namespace AccountMovement.Controllers;
[ApiController]
[Route("[controller]")]
public class AccountController : Controller
{
    private IApiDatabase _context;
    public AccountController(IApiDatabase context)
    {
        _context = context;
    }

    [HttpGet("{AccountNumber}")]
    public async Task<IActionResult> Get(int AccountNumber)
    {
        var res = await _context.Account.Where(w => w.AccountNumber == AccountNumber).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        return Ok(res);
    }
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var res = await _context.Account.Where(w => w.AccountState == true).ToListAsync();
        if (res == null) return NotFound();
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Account account)
    {
        var cli = await _context.Client.Where(w => w.ClientID == account.ClientID).AsNoTracking().FirstOrDefaultAsync();
        if (cli == null) return NotFound($"El cliente no existe: {account.ClientID}");
        _context.Account.Add(account);
        await _context.SaveChangesAsync();
        return Ok(account);
    }
    [HttpPut("{AccountNumber}")]
    public async Task<IActionResult> Put(Account account, int AccountNumber)
    {
        if (AccountNumber != account.AccountNumber) throw new Exception();
        var res = await _context.Account.Where(w => w.AccountNumber == AccountNumber).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        res.AccountType = account.AccountType;
        res.AccountBalance = account.AccountBalance;
        res.AccountState = account.AccountState;
        await _context.SaveChangesAsync();
        return Ok(res);
    }
    [HttpDelete("{AccountNumber}")]
    public async Task<IActionResult> Delete(int AccountNumber)
    {
        var res = await _context.Account.Where(w => w.AccountNumber == AccountNumber).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        _context.Account.Remove(res);
        await _context.SaveChangesAsync();
        return Ok(res);
    }
}


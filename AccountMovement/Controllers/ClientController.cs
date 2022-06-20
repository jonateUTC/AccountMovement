using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AccountMovement.Entities;
using AccountMovement.Interfaces;

namespace AccountMovement.Controllers;
[ApiController]
[Route("[controller]")]
public class ClientController : Controller
{
    private IApiDatabase _context;
    public ClientController(IApiDatabase context)
    {
        _context = context;
    }

    [HttpGet("{PersonIdentification}")]
    public async Task<IActionResult> Get(String PersonIdentification)
    {
        var res = await _context.Client.Where(w => w.PersonIdentification == PersonIdentification).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        return Ok(res);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var res = await _context.Client.Where(w => w.ClientState == true).ToListAsync();
        if (res == null) return NotFound();
        return Ok(res);
    }

    [HttpPost]
    public async Task<Client> Post(Client client)
    {
        _context.Client.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }
    [HttpPut("{PersonIdentification}")]
    public async Task<IActionResult> Put(Client client, String PersonIdentification)
    {
        if (PersonIdentification != client.PersonIdentification) throw new Exception();
        var res = await _context.Client.Where(w => w.PersonIdentification == PersonIdentification).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        res.PersonName = client.PersonName;
        res.PersonGender = client.PersonGender;
        res.PersonBirthday = client.PersonBirthday;
        res.PersonAge = client.PersonAge;
        res.PersonAddress = client.PersonAddress;
        res.PersonPhone = client.PersonPhone;
        res.ClientPassword = client.ClientPassword;
        await _context.SaveChangesAsync();
        return Ok(res);
    }
    [HttpDelete("{PersonIdentification}")]
    public async Task<IActionResult> Delete(String PersonIdentification)
    {
        var res = await _context.Client.Where(w => w.PersonIdentification == PersonIdentification).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        _context.Client.Remove(res);
        await _context.SaveChangesAsync();
        return Ok(res);
    }
}


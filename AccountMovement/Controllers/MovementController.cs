using AccountMovement.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AccountMovement.Entities;
using System.Net;
using System.Web;

namespace AccountMovement.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MovementController : Controller
{
    private IApiDatabase _context;

    public MovementController(IApiDatabase context)
    {
        _context = context;
    }

    [HttpGet("{MovementID}")]
    public async Task<IActionResult> Get(int MovementID)
    {
        var res = await _context.Movement.Where(w => w.MovementID == MovementID).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        return Ok(res);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(Movement movement)
    {
        var acc = await _context.Account.Where(w => w.AccountID == movement.AccountID).FirstOrDefaultAsync();
        if (acc == null) return NotFound($"El cliente no existe: {movement.AccountID}");
        if (acc.AccountBalance < movement.MovementValue && (movement.MovementType.ToUpper() == "DEBITO" || movement.MovementType.ToUpper() == "DÉBITO"))
        {
            return NotFound("Saldo no disponible");
        }
        if (movement.MovementValue == 0) return NotFound("El Valor no puede ser Cero");
        if (acc.AccountBalance < movement.MovementValue && (movement.MovementType.ToUpper() == "DEBITO" || movement.MovementType.ToUpper() == "DÉBITO"))
        {
            return NotFound("Saldo no disponible");
        }
        if (movement.MovementValue == 0) return NotFound("El Valor no puede ser Cero");
        if (movement.MovementType.ToUpper() == "DEBITO" || movement.MovementType.ToUpper() == "DÉBITO")
        {
            movement.MovementType = "debito";
            if (movement.MovementValue > 0)
            {
                movement.MovementValue = movement.MovementValue * -1;
            }
            else
            {
                movement.MovementValue = movement.MovementValue;
            }

        }
        else
        {
            movement.MovementType = "credito";
            if (movement.MovementValue > 0)
            {
                movement.MovementValue = movement.MovementValue;
            }
            else
            {
                movement.MovementValue = movement.MovementValue * -1;
            }
        }
        DateTime dateNow = DateTime.UtcNow;
        movement.MovementBalance = acc.AccountBalance + movement.MovementValue;
        movement.MovementValueIni = acc.AccountBalance;
        acc.AccountBalance = acc.AccountBalance + movement.MovementValue;
        var accday = await _context.Movement.Where(w => w.AccountID == movement.AccountID &&
                            w.MovementDate.Date == movement.MovementDate).ToListAsync();
        Decimal total = 0;
        for (int i = 0; i < accday.Count; i++)
        {
            if(accday[i].MovementType == "debito")
            {
                total += accday[i].MovementValue;
            }            
        }
        total = total + movement.MovementValue;
        if ((total*-1) > 1000) return NotFound("Cupo diario excedido.");
        _context.Movement.Add(movement);
        await _context.SaveChangesAsync();
        return Ok(movement);
    }

    [HttpPut("{MovementID}")]
    public async Task<IActionResult> Put(Movement movement, int MovementID)
    {
        if (MovementID != movement.MovementID) throw new Exception();
        var res = await _context.Movement.Where(w => w.MovementID == MovementID).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        res.MovementDate = movement.MovementDate;
        res.MovementBalance = movement.MovementBalance;
        res.MovementValue = movement.MovementValue;
        res.MovementType = movement.MovementType;
        await _context.SaveChangesAsync();
        return Ok(res);
    }
    [HttpDelete("{MovementID}")]
    public async Task<IActionResult> Delete(int MovementID)
    {
        var mov = await _context.Movement.Where(w => w.MovementID == MovementID).FirstOrDefaultAsync();
        if (mov == null) return NotFound();
        _context.Movement.Remove(mov);
        await _context.SaveChangesAsync();
        return Ok(mov);
    }
    [HttpGet()]
    public async Task<IActionResult> GetReport(string date, int clientID)
    {
        var parameter = date.Split(',');
        if (parameter.Length != 2)
        {
            return NotFound("Debe ingresar rango de fecha separado por comas Ej. 12/05/2022,25/05/2022");
        }
        DateTime datefrom = DateTime.ParseExact(parameter[0], "dd/MM/yyyy", null);
        DateTime dateto = DateTime.ParseExact(parameter[1], "dd/MM/yyyy", null);
        var res = (from mo in _context.Movement
                   join acc in _context.Account on mo.AccountID equals acc.AccountID
                   join cli in _context.Client on acc.ClientID equals cli.ClientID
                   where mo.MovementDate.Date >= datefrom.Date && mo.MovementDate.Date <= dateto.Date &&
                            cli.ClientID == clientID
                   orderby mo.AccountID, mo.MovementDate descending
                   select new { mo.MovementDate.Date, cli.PersonName, acc.AccountNumber, acc.AccountType, mo.MovementValueIni, acc.AccountState, mo.MovementValue, mo.MovementBalance });
        if (res == null) return NotFound();
        return Ok(res);
    }

}


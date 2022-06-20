using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AccountMovement.Context;
using AccountMovement.Entities;
using AccountMovement.Interfaces;

namespace AccountMovement.Controllers;

public class MovementsController : Controller
{
    private IApiDatabase _context;

    public MovementsController(IApiDatabase context)
    {
        _context = context;
    }

    /*[HttpGet("{MovementID}")]
    public async Task<IActionResult> Get(int MovementID)
    {
        var res = await _context.Movement.Where(w => w.MovementID == MovementID).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        return Ok(res);
    }
    [HttpGet("{AccountID}")]
    public async Task<IActionResult> GetAll(int AccountID)
    {
        var res = await _context.Movement.Where(w => w.AccountID == AccountID).ToListAsync();
        if (res == null) return NotFound();
        return Ok(res);
    }
    */
    [HttpPost]
    public async Task<IActionResult> Post(Movement movement)
    {
        var cli = await _context.Account.Where(w => w.AccountID == movement.AccountID).AsNoTracking().FirstOrDefaultAsync();
        if (cli == null) return NotFound($"El cliente no existe: {movement.AccountID}");
        _context.Movement.Add(movement);
        await _context.SaveChangesAsync();
        return Ok(movement);
    }

    /*[HttpPut("{MovementID}")]
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
    }*/
}


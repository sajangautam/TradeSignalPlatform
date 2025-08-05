using Microsoft.AspNetCore.Mvc;
using TradeSignalManager.Core.Interfaces;
using TradeSignalManager.Core.Entities;

namespace TradeSignalManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TradeSignalController(ITradeSignalRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await repository.GetAllAsync());
        
    [HttpPost]
    public async Task<IActionResult> Create(TradeSignal signal)
        => CreatedAtAction(nameof(GetAll), await repository.AddAsync(signal));

}
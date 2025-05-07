using Kolokwium1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitsController : ControllerBase
{
    private readonly IVisitsService _visitsService;

    public VisitsController(IVisitsService visitsService)
    {
        _visitsService = visitsService;
    }


    [HttpGet("{visitId}")]
    public async Task<IActionResult> Get(int visitId)
    {
        var result = _visitsService.GetVisits(visitId);

        if (result == null)
        {
            return NotFound($"Nie znaleziono vizyty o ID:{visitId}");
        }
        
        return Ok(result.Result);
    }
}
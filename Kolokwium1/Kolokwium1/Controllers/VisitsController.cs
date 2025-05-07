using Kolokwium1.Models_DTOs;
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
        try
        {
            var result = _visitsService.GetVisits(visitId);
            return Ok(result.Result);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }



    [HttpPost]
    public async Task<IActionResult> PostVisit([FromBody] CreateVisitDTO createVisitDto)
    {
        var resultat = _visitsService.PostVisits(createVisitDto);

        return Created();
    }
}
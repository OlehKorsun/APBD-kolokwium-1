using Kolokwium1.Models_DTOs;

namespace Kolokwium1.Services;

public interface IVisitsService
{
    Task<VisitsDTO> GetVisits(int id);
    
    Task<bool> PostVisits(CreateVisitDTO visits);
}
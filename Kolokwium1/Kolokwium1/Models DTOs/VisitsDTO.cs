namespace Kolokwium1.Models_DTOs;

public class VisitsDTO
{
    public DateTime Date { get; set; }
    public ClientDTO Client { get; set; }
    public MechanicDTO Mechanic { get; set; }
    public List<VisitServicesDTO> VisitServices { get; set; }
}
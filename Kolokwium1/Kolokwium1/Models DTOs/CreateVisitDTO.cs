namespace Kolokwium1.Models_DTOs;

public class CreateVisitDTO
{
    public int VisitId { get; set; }
    public int ClientId { get; set; }
    public string MechanicLicenceNumber { get; set; }
    public List<CreateServiceDTO> Services { get; set; }
}

public class CreateServiceDTO
{
    public string ServiceName { get; set; }
    public decimal ServiceFee { get; set; } 
}
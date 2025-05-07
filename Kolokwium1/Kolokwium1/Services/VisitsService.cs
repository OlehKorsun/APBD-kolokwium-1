using Kolokwium1.Models_DTOs;
using Microsoft.Data.SqlClient;

namespace Kolokwium1.Services;

public class VisitsService : IVisitsService
{
    private string connectionString;
    public VisitsService(IConfiguration configuration)
    {
        connectionString = configuration["DefaultConnection"];
    }

    public async Task<VisitsDTO> GetVisits(int visitId)
    {
        
        VisitsDTO? visitsDTO = null;

        string visitQuery =
            @"Select date, Client.first_name, Client.last_name, date_of_birth, Mechanic.mechanic_id, licence_number, name, base_fee 
                from Visit
                join Client on Client.client_id = Visit.client_id
                join Mechanic on Mechanic.mechanic_id = Visit.mechanic_id
                join Visit_Service on Visit.visit_id = Visit_Service.visit_id
                join Service on Service.service_id = Visit_Service.service_id
                Where Visit.visit_id = @visitId;";


        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(visitQuery, conn))
        {
            await conn.OpenAsync();
            cmd.Parameters.AddWithValue("@visitId", visitId);

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (visitsDTO == null)
                    {
                        visitsDTO = new VisitsDTO()
                        {
                            Date = reader.GetDateTime(0),
                            Client = new ClientDTO()
                            {
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                DateOfBirth = reader.GetDateTime(3),
                            },
                            Mechanic = new MechanicDTO()
                            {
                                mechanicId = reader.GetInt32(4),
                                licenceNumber = reader.GetString(5),
                            },
                            VisitServices = new List<VisitServicesDTO>()
                            {
                                new VisitServicesDTO()
                                {
                                    name = reader.GetString(6),
                                    serviceFee = reader.GetInt32(7)
                                }
                            }
                        };
                    }
                    else
                    {
                        visitsDTO.VisitServices.Add(new VisitServicesDTO()
                        {
                            name = reader.GetString(1),
                            serviceFee = reader.GetInt32(2)
                        });
                    }
                }
            }
        }
        return visitsDTO;
    }
}
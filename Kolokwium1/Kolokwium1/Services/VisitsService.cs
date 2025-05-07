using Kolokwium1.Models_DTOs;
using Microsoft.Data.SqlClient;

namespace Kolokwium1.Services;

public class VisitsService : IVisitsService
{
    // private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=apbd;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
    // public VisitsService(IConfiguration configuration)
    // {
    //     connectionString = configuration["DefaultConnection"];
    // }

    public async Task<VisitsDTO> GetVisits(int visitId)
    {
        string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=apbd;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        
        VisitsDTO? visitsDTO = null;

        string visitQuery =
            @"Select date, Client.first_name, Client.last_name, date_of_birth, Mechanic.mechanic_id, licence_number, name, base_fee 
                from Visit
                join Client on Client.client_id = Visit.client_id
                join Mechanic on Mechanic.mechanic_id = Visit.mechanic_id
                join Visit_Service on Visit.visit_id = Visit_Service.visit_id
                join Service on Service.service_id = Visit_Service.service_id
                Where Visit.visit_id = @visitId;";


        using (SqlConnection conn = new SqlConnection(_connectionString))
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
                                    serviceFee = reader.GetDecimal(7)
                                }
                            }
                        };
                    }
                    else
                    {
                        visitsDTO.VisitServices.Add(new VisitServicesDTO()
                        {
                            name = reader.GetString(6),
                            serviceFee = reader.GetDecimal(7)
                        });
                    }
                }
            }
        }

        if (visitsDTO == null)
        {
            throw new Exception($"Nie znaleziono vizyty z ID: {visitId}");
        }
        
        return visitsDTO;
    }




    public async Task<bool> PostVisits(CreateVisitDTO visitsDTO)
    {

        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=apbd;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        // sprawdzam czy juz istnieje wizyta
        string checkVisitQuery = @"Select 1 from visit where visit_id = @visitId;";
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(checkVisitQuery, conn))
        {
            await conn.OpenAsync();
            cmd.Parameters.AddWithValue("@visitId", visitsDTO.VisitId);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                bool checkRes = false;
                while (await reader.ReadAsync())
                {
                    checkRes = reader.IsDBNull(0);
                }
                
                if (!checkRes)
                    throw new Exception($"Już istnieje wizyta o ID: {visitsDTO.VisitId}");
            }
        }
        
        
        // sprawdzam czy istnieje client
        string checkClientQuery = @"Select 1 from Client where client_id = @clientId;";
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(checkClientQuery, conn))
        {
            await conn.OpenAsync();
            cmd.Parameters.AddWithValue("@clientId", visitsDTO.ClientId);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                bool checkRes = false;
                while (await reader.ReadAsync())
                {
                    checkRes = reader.IsDBNull(0);
                }
                
                if (checkRes)
                    throw new Exception($"Nie istnieje klienta o ID: {visitsDTO.ClientId}");
            }
        }
        
        
        // sprawdzam czy istnieje mechanik o podanym numerze licencji
        string checkMechanicQuery = @"Select 1 from Mechanic where licence_number = @licenceNumber;";
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(checkClientQuery, conn))
        {
            await conn.OpenAsync();
            cmd.Parameters.AddWithValue("@licenceNumber", visitsDTO.MechanicLicenceNumber);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                bool checkRes = false;
                while (await reader.ReadAsync())
                {
                    checkRes = reader.IsDBNull(0);
                }
                
                if (checkRes)
                    throw new Exception($"Nie istnieje mechanika o numerze licencji: {visitsDTO.MechanicLicenceNumber}");
            }
        }
        
        
        // sprawdzam czy istnieja servisy o podanej nazwie
        foreach (CreateServiceDTO csdto in visitsDTO.Services)
        {
            
        }


        return false;
    }
}
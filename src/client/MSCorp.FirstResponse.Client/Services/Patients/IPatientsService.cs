using MSCorp.FirstResponse.Client.Models;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Patients
{
    public interface IPatientsService
    {
        Task<PatientModel> GetPatientAsync(string patientId);

        Task<bool> AddReportAsync(string patientId, EpcrModel ePcr);
    }
}

using System.Threading.Tasks;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.Services.Patients;
using MSCorp.FirstResponse.Client.Data;
using System.Linq;

namespace MSCorp.FirstResponse.Client.Services.Mock.Patients
{
    public class MockPatientsService : IPatientsService
    {
        public async Task<PatientModel> GetPatientAsync(string patientId)
        {
            await Task.Delay(500);
            return DataRepository.LoadPatientsData().FirstOrDefault(p => p.Id == patientId);
        }

        public async Task<bool> AddReportAsync(string patientId, EpcrModel ePcr)
        {
            await Task.Delay(500);
            return true;
        }
    }
}

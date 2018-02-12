namespace FirstResponse.WebAPI.Mock.Controllers
{
    using MSCorp.FirstResponse.WebApiDemo.Models;
    using MSCorp.FirstResponse.WebApiDemo.Repositories;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Linq;

    [RoutePrefix("patient")]
    public class PatientController : ApiController
    {

        private GenericDocumentDbRepository<PatientModel> _patientRepository;

        public PatientController()
        {
            _patientRepository = new GenericDocumentDbRepository<PatientModel>("FirstResponse", "Patient");
        }

        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetPatients()
        {
            var patients = await _patientRepository.GetItemsAsync();

            return Ok(patients);
        }

        [HttpGet]
        [Route("{patientId}")]
        public async Task<IHttpActionResult> GetPatientInfo(string patientId)
        {
            var patient = await _patientRepository.GetItemAsync(patientId);

            return Ok(patient);
        }


        [HttpGet]
        [Route("search/{patientName}")]
        public async Task<IHttpActionResult> SearchPatientInfo(string patientName)
        {
            patientName = patientName.ToLower();
            var patient = await _patientRepository.GetItemsAsync(p => p.FirstName.ToLower().Contains(patientName) || p.LastName.ToLower().Contains(patientName));

            return Ok(patient);
        }

        [HttpPost]
        [Route("{patientId}/epcr")]
        public async Task<IHttpActionResult> InsertEpcr(string patientId, [FromBody] ePCRModel epcr)
        {
            if (epcr == null)
                return BadRequest("ePCR can't be null");

            var patient = await _patientRepository.GetItemAsync(patientId);
            patient.ePCRs.Add(epcr);
            var documentCreated = await _patientRepository.UpdateItemAsync(patientId, patient);

            return Created(documentCreated.SelfLink, patient);
        }
    }
}
namespace MSCorp.FirstResponse.WebApiDemo.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class PatientModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string HairColor { get; set; }

        public string EyeColor { get; set; }

        public string SkinColor { get; set; }

        public string Sex { get; set; }

        public string SuspectSearchImage { get; set; }

        public string MedicationAllergies { get; set; }

        public string CurrentMedication { get; set; }

        public string HealthIssues { get; set; }

        public string BloodType { get; set; }

        public List<ePCRModel> ePCRs { get; set; }
    }
}

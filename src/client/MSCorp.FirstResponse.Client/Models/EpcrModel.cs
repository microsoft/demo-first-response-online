using MSCorp.FirstResponse.Client.Helpers;
using System;

namespace MSCorp.FirstResponse.Client.Models
{
    public class EpcrModel : ExtendedBindableObject
    {
        public string InjuryDescription { get; set; }

        public string PainLocation { get; set; }

        public string InitialAssesment { get; set; }

        public int HeartRate { get; set; } // bpm

        public float Temperature { get; set; } // °F

        public int SystolicPressure { get; set; } // mmHg

        public int DiastolicPressure { get; set; } // mmHg

        public int EcgPrInterval { get; set; } // milliseconds

        public int EcgQtInterval { get; set; } // milliseconds

        public DateTime ReportDate { get; set; }
    }
}

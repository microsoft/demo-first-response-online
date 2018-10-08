namespace MSCorp.FirstResponse.Client.Models
{
    public class IncidentInvolvedNavigationParameter
    {
        public IncidentInvolvedNavigationParameter(SuspectModel person, IncidentModel incident)
        {
            Person = person;
            Incident = incident;
        }

        public SuspectModel Person { get; private set; }

        public IncidentModel Incident { get; private set; }
    }
}

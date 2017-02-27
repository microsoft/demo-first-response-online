using Microsoft.PowerBI.Api.V1.Models;

namespace MSCorp.FirstResponse.WebApiDemo.Models
{
    public class ReportViewModel
    {
        public Report Report { get; set; }

        public string AccessToken { get; set; }
    }
}
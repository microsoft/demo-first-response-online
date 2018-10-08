using MSCorp.FirstResponse.Client.Helpers;
using Newtonsoft.Json;

namespace MSCorp.FirstResponse.Client.Models
{
    public class SuspectModel : ExtendedBindableObject
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public string SkinColor { get; set; }
        public string Sex { get; set; }
        public string SuspectSearchImage { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            SuspectModel objAsSuspect = obj as SuspectModel;

            if (objAsSuspect == null) return false;

            if (!string.IsNullOrEmpty(this.Id))
            {
                return this.Id == objAsSuspect.Id;
            }
            else if (!string.IsNullOrEmpty(this.Name))
            {
                return this.Name == objAsSuspect.Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return !string.IsNullOrEmpty(this.Name)
                ? this.Name.GetHashCode()
                : base.GetHashCode();
        }
    }
}

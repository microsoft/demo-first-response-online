using MSCorp.FirstResponse.Client.Helpers;

namespace MSCorp.FirstResponse.Client.Models
{
    public class SuspectModel : ExtendedBindableObject
    {
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
            return (this.Name.Equals(objAsSuspect.Name));
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

    }
}

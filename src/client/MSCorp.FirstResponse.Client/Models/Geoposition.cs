namespace MSCorp.FirstResponse.Client.Models
{
    public class Geoposition
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override bool Equals(object obj)
        {
            Geoposition position = obj as Geoposition;
            if (position == null)
                return false;

            return Latitude == position.Latitude && Longitude == position.Longitude;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + Latitude.GetHashCode();
            hash = hash * 23 + Longitude.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return $"({Latitude}, {Longitude})";
        }
    }
}

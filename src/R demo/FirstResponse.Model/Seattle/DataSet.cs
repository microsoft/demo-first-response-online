namespace FirstResponse.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DataSet")]
    public partial class DataSet
    {
        public int Id { get; set; }

        public int ZoneId { get; set; }

        public int Distance1000 { get; set; }

        public int Distance3000 { get; set; }

        public int Distance6000 { get; set; }

        public int DistancePlus6000 { get; set; }

        public int Temperature { get; set; }

        public int SeaLvlPress { get; set; }

        public int Windspeed { get; set; }

        public decimal Rain { get; set; }

        public int EventCount { get; set; }
    }
}

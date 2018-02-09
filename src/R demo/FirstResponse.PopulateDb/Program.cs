using FirstResponse.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstResponse.PopulateDb
{
    class Program
    {
        static void Main(string[] args)
        {
            var information = File.ReadAllLines(@"Data/911Dataset.csv").Skip(1);
            var datas = new List<Model.DataSet>();

            foreach (var line in information)
            {
                var columns = line.Split(',');

                var dataset = new Model.DataSet()
                {
                    ZoneId = int.Parse(columns[0]),
                    Distance1000 = int.Parse(columns[1]),
                    Distance3000 = int.Parse(columns[2]),
                    Distance6000 = int.Parse(columns[3]),
                    DistancePlus6000 = int.Parse(columns[4]),
                    Temperature = int.Parse(columns[5]),
                    SeaLvlPress = int.Parse(columns[6]),
                    Windspeed = int.Parse(columns[7]),
                    Rain = decimal.Parse(columns[8].Replace(".", ",")),
                    EventCount = int.Parse(columns[9])
                };

                datas.Add(dataset);
            }

            BulkInsert(datas);
        }

        private static void BulkInsert(IEnumerable<Model.DataSet> data)
        {
            var sbCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["SeattleContext"].ConnectionString);
            sbCopy.DestinationTableName = "dbo.Dataset";

            DataTable dt = new DataTable("dbo.Dataset");

            //define columns
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("ZoneId", typeof(int));
            dt.Columns.Add("Distance1000", typeof(int));
            dt.Columns.Add("Distance3000", typeof(int));
            dt.Columns.Add("Distance6000", typeof(int));
            dt.Columns.Add("DistancePlus6000", typeof(int));
            dt.Columns.Add("Temperature", typeof(int));
            dt.Columns.Add("SeaLvlPress", typeof(int));
            dt.Columns.Add("Windspeed", typeof(int));
            dt.Columns.Add("Rain", typeof(decimal));
            dt.Columns.Add("EventCount", typeof(int));

            //fill rows
            foreach (var item in data)
            {
                dt.Rows.Add(
                    item.Id,
                    item.ZoneId,
                    item.Distance1000,
                    item.Distance3000,
                    item.Distance6000,
                    item.DistancePlus6000,
                    item.Temperature,
                    item.SeaLvlPress,
                    item.Windspeed,
                    item.Rain,
                    item.EventCount);
            }

            sbCopy.BulkCopyTimeout = 0;
            sbCopy.WriteToServer(dt);
            sbCopy.Close();
        }
    }
}

using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using RRDNet.Core;

namespace alive
{
    public class FillController : BaseController
    {
        [Route("{machineId}")]
        [HttpGet]
        public string Action(string machineId = "machine", string time = "2023-10-01 20:00")
        {
            RrdDb db = null;
            DateTimeOffset t = DateTimeOffset.Parse(time, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);

            try
            {
                db = RRD.GetDb(machineId);
                var sample = db.CreateSample();
                for (var i = 0; i < 100; i++)
                    sample.SetAndUpdate($"{t.Add(t.Offset).ToUnixTimeSeconds() + 1 + i * 5 * 60}:{1}");
            }
            finally
            {
                db?.Close();
            }

            return "pong";
        }
    }
}

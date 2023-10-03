using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using RRDNet.Core;

namespace alive
{
    public class PingController : BaseController
    {
        [Route("{machineId}")]
        [HttpGet]
        public string Action(string machineId = "machine", string m = "m")
        {
            RrdDb db = null;

            if (!DateTimeOffset.TryParse(m, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var time))
                time = DateTimeOffset.Now;

            try
            {
                db = RRD.GetDb(machineId);
                var sample = db.CreateSample();
                for (var i = 0; i < 2; i++)
                    sample.SetAndUpdate($"{time.Add(time.Offset).ToUnixTimeSeconds() + 1 + RRD.Step * i}:1");
            }
            finally
            {
                db?.Close();
            }

            return "pong";
        }
    }
}

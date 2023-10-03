using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using RRDNet.Graph;

namespace alive
{
    public class ViewController : BaseController
    {
        [HttpGet]
        [Route("{machineId}")]
        public IActionResult Action(
            string machineId = "machine", string from = "2023-10-01 07:00", string to = "2023-10-01 10:00")
        {
            var dbPath = Path.Join(Locations.DataRoot, machineId);
            if (!System.IO.File.Exists(dbPath))
            {
                return NotFound();
            }

            var f = DateTimeOffset.Parse(from, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            var t = DateTimeOffset.Parse(to, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);

            RrdGraphDef graphDef = new();

            graphDef.SetTimePeriod(f.Add(f.Offset).ToUnixTimeSeconds(), t.Add(f.Offset).ToUnixTimeSeconds());
            graphDef.Title = $"Pings from: {machineId}";
            graphDef.Datasource("alive", dbPath, "alive", "AVERAGE");
            graphDef.Area("alive", Color.Green, "ping");
            graphDef.SetValueAxis(1, 1);

            RrdGraph graph = new(graphDef);
            var bytes = graph.GetPNGBytes(800, 200);
            var stream = new MemoryStream(bytes);

            return new FileStreamResult(stream, "image/png");
        }
    }
}

using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace alive
{
    public class AliveController : BaseController
    {
        [HttpGet]
        public string Action()
        {
            var sb = new StringBuilder();
            sb.AppendLine("alive");
            sb.AppendLine($"root: {Locations.Root}");
            sb.AppendLine($"data: {Locations.DataRoot}");

            return sb.ToString();
        }

        [HttpGet]
        [Route("List")]
        public string List()
        {
            var sb = new StringBuilder();
            foreach (var db in Directory.GetFiles(Locations.DataRoot))
                sb.AppendLine($"+ {Path.GetFileName(db)}");

            return sb.ToString();
        }
    }
}

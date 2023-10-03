using RRDNet.Core;

namespace alive
{
    public class RRD
    {
        public const int Step = 5 * 60; // 5min

        public static RrdDb GetDb(string dbName)
        {
            var dbPath = Path.Join(Locations.DataRoot, dbName);
            if (!File.Exists(dbPath))
                return CreateDb(dbPath);

            return OpenDb(dbPath);
        }

        public static RrdDb CreateDb(string dbPath)
        {
            var now = DateTimeOffset.Now;

            RrdDef rrdDef = new(dbPath)
            {
                StartTime = now.Add(now.Offset).ToUnixTimeSeconds(),
                Step = Step, // 5m
            };

            rrdDef.AddDatasource("alive", "GAUGE", Step, 0, Double.NaN);
            rrdDef.AddArchive("AVERAGE", 0.5, 1, 3600/Step * 24 * 30);

            RrdDb rrdDb = new(rrdDef);

            return rrdDb;
        }

        public static RrdDb OpenDb(string dbPath) =>
            new RrdDb(dbPath, false);
    }
}

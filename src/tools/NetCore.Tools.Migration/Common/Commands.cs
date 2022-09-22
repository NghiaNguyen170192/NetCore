using CommandLine;

namespace NetCore.Tools.Migration.Common
{
    public class Command
    {
        [Option('m', "migration", Required = true, HelpText = "Run Migration.")]
        public bool RunMigration { get; set; }

        [Option('s', "seed-data", Required = false, HelpText = "Run Seed.")]
        public bool RunSeeds { get; set; }

        [Option('t', "test-data", Required = false, HelpText = "Run Seed Data.")]
        public bool RunSeedsTestData { get; set; }

        [Option('d', "delete-database", Required = false, HelpText = "Delete Database.")]
        public bool RunDeleteDatabase { get; set; }
    }
}

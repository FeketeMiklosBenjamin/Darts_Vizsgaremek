namespace VizsgaBackend.Models
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
        public string UsersTournamentStatsCollectionName { get; set; } = null!;
        public string UsersFriendlyStatsCollectionName { get; set; } = null!;
        public string MessagesCollectionName { get; set; } = null!;
    }
}

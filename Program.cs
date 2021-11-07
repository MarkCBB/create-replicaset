using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace csharp_replicaset_config
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Waiting 20 seconds to let mongo instances be ready");
            Task.WaitAll(Task.Delay(20000));
            var mongo1 = ConnectMongoInstance("mongodb://mongo1:27017/?connect=direct");
            if (mongo1 == null)
            {
                Console.WriteLine("Couldn't connect to the main instance");
                return;
            }
            Console.WriteLine("Connected to mongodb1 instance");
            try
            {
                var db = mongo1.GetDatabase("admin");
                var command = GetReplicasetCommand();
                var result = db.RunCommand<BsonDocument>(command);
                Console.WriteLine("Replicaset created");
            }
            catch
            {
                Console.WriteLine("Couldn't create the replicaset");
            }
        }

        static MongoClient ConnectMongoInstance(string server)
        {
            Console.WriteLine("Trying to connect to instance: " + server);
            MongoClient client = null;
            var retries = 0;
            const int MAX_RETRIES = 15;
            while (retries < MAX_RETRIES)
            {
                try
                {
                    client = new MongoClient(server);
                    return client;
                }
                catch
                {
                    Task.WaitAll(Task.Delay(2000));
                    retries++;
                }
            }
            return null;
        }

        private static BsonDocument GetReplicasetCommand()
        {
            var host1 = new BsonDocument { { "_id", 0 }, { "host", "mongo1:27017" } };
            var host2 = new BsonDocument { { "_id", 1 }, { "host", "mongo2:27017" } };
            var host3 = new BsonDocument { { "_id", 2 }, { "host", "mongo3:27017" } };
            var hosts = new BsonArray() { host1, host2, host3 };
            var members = new BsonElement("members", hosts);
            var replicaId = new BsonElement("_id", "Replica_UOC");
            var cfg = new BsonDocument() { replicaId, members };
            return new BsonDocument() { new BsonElement("replSetInitiate", cfg) };
        }
    }
}

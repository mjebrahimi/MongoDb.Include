using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MongoDb.Include
{
    internal static class CommandExtractor
    {
        private static readonly Regex regex = new Regex(@"^aggregate\((?<Commands>.+)\)$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        internal static IEnumerable<PipelineStageDefinition<TEntity, TEntity>> ExtractStages<TEntity>(IQueryable query)
        {
            var str = query.ToString();
            return ExtractStages<TEntity>(str);
        }

        internal static IEnumerable<PipelineStageDefinition<TEntity, TEntity>> ExtractStages<TEntity>(string query)
        {
            var commands = regex.Match(query).Groups["Commands"].Value;

            using var jsonReader = new JsonReader(commands);
            var bsonArray = BsonArraySerializer.Instance.Deserialize(BsonDeserializationContext.CreateRoot(jsonReader));

            foreach (var item in bsonArray)
                yield return item.ToString();
        }
    }
}

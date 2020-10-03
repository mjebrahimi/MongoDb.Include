//using MongoDB.Bson.Serialization;
//using MongoDB.Bson.Serialization.Conventions;
//using System;

//namespace MongoDb.Include
//{
//    #region NavigationPropertySerializer
//    //var pack = new ConventionPack { new NavigationPropertySerializer() };
//    //ConventionRegistry.Register("NavigationPropertySerializer", pack, _ => true);

//    public class NavigationPropertySerializer : IBsonSerializer //BsonClassMapSerializer
//    {
//        private readonly IBsonSerializer serializer;

//        public NavigationPropertySerializer(IBsonSerializer serializer)
//        {
//            this.serializer = serializer.NotNull(nameof(serializer));
//        }

//        public Type ValueType => serializer.ValueType;

//        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
//        {
//            var result = serializer.Deserialize(context, args);
//            return result;
//        }

//        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
//        {
//            serializer.Serialize(context, args, value);
//        }
//    }

//    public class IgnoreEmptyObjectConvention : ConventionBase, IMemberMapConvention// where TValue : class
//    {
//        public IgnoreEmptyObjectConvention() : base("NavigationPropertySerializer")
//        {
//        }

//        public void Apply(BsonMemberMap memberMap)
//        {
//            if (memberMap.MemberType.IsCustomType())
//                memberMap.SetSerializer(new NavigationPropertySerializer(memberMap.GetSerializer()));
//        }
//    }
//    #endregion
//}

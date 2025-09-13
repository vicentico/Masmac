using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Infrastructure.Persistence.Configurations;

public static class MongoDbConfiguration
{
    public static void Configure()
    {
        // Convenciones globales
        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreIfNullConvention(true)
        };
        ConventionRegistry.Register("VetUberAppConventions", pack, t => true);

        // Serialización de tipos comunes
        BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Utc));
        BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));

        // Mapeo de entidades
        if (!BsonClassMap.IsClassMapRegistered(typeof(Address)))
        {
            BsonClassMap.RegisterClassMap<Address>(cm =>
            {
                cm.AutoMap();
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
        {
            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapMember(x => x.Address);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Veterinarian)))
        {
            BsonClassMap.RegisterClassMap<Veterinarian>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapMember(x => x.CurrentLocation);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Pet)))
        {
            BsonClassMap.RegisterClassMap<Pet>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Appointment)))
        {
            BsonClassMap.RegisterClassMap<Appointment>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapMember(x => x.Location);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Review)))
        {
            BsonClassMap.RegisterClassMap<Review>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Payment)))
        {
            BsonClassMap.RegisterClassMap<Payment>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
using Xunit;

namespace VetUberApp.IntegrationTests;

public sealed class MongoDbSkipCondition : IDisposable
{
    private static bool? _isMongoDbAvailable;
    
    public static bool IsMongoDbAvailable
    {
        get
        {
            if (_isMongoDbAvailable.HasValue)
                return _isMongoDbAvailable.Value;

            try
            {
                // Intentar crear una instancia del TestDatabaseFixture
                var fixture = new TestDatabaseFixture();
                fixture.Dispose();
                _isMongoDbAvailable = true;
                return true;
            }
            catch
            {
                _isMongoDbAvailable = false;
                return false;
            }
        }
    }

    public void Dispose()
    {
        // No hay recursos que liberar
    }
}

public class MongoDbSkipAttribute : FactAttribute
{
    public MongoDbSkipAttribute()
    {
        if (!MongoDbSkipCondition.IsMongoDbAvailable)
        {
            Skip = "MongoDB no está disponible";
        }
    }
}

public class MongoDbTheoryAttribute : TheoryAttribute
{
    public MongoDbTheoryAttribute()
    {
        if (!MongoDbSkipCondition.IsMongoDbAvailable)
        {
            Skip = "MongoDB no está disponible";
        }
    }
}
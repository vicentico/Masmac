using Xunit;

namespace VetUberApp.IntegrationTests;

/// <summary>
/// Configura las pruebas de integración para ejecutarse de forma secuencial
/// para evitar problemas de concurrencia con MongoDB
/// </summary>
[CollectionDefinition("IntegrationTests", DisableParallelization = true)]
public class TestCollectionDefinition : ICollectionFixture<TestDatabaseFixture>
{
    // Esta clase existe solo para definir la colección de pruebas
    // El atributo DisableParallelization evita que las pruebas se ejecuten en paralelo
    // ICollectionFixture<TestDatabaseFixture> asegura que todas las pruebas compartan la misma instancia del fixture
}
using MongoDB.Bson;

namespace VetUberApp.Infrastructure.Common;

/// <summary>
/// Clase de utilidad para operaciones comunes con MongoDB
/// </summary>
public static class MongoDbHelper
{
    /// <summary>
    /// Valida si un string es un ObjectId válido de MongoDB
    /// </summary>
    /// <param name="id">ID a validar</param>
    /// <returns>True si es un ObjectId válido, false en caso contrario</returns>
    public static bool IsValidObjectId(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return false;
            
        return ObjectId.TryParse(id, out _);
    }

    /// <summary>
    /// Valida un ObjectId y lanza una excepción si no es válido
    /// </summary>
    /// <param name="id">ID a validar</param>
    /// <param name="parameterName">Nombre del parámetro para la excepción</param>
    /// <exception cref="ArgumentException">Si el ID no es válido</exception>
    public static void ValidateObjectId(string? id, string parameterName = "id")
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("El ID no puede ser null o vacío", parameterName);
            
        if (!IsValidObjectId(id))
            throw new ArgumentException($"'{id}' no es un ObjectId válido de MongoDB. Debe ser una cadena hexadecimal de 24 caracteres.", parameterName);
    }

    /// <summary>
    /// Convierte un string a ObjectId si es válido
    /// </summary>
    /// <param name="id">ID a convertir</param>
    /// <returns>ObjectId si es válido, null si no</returns>
    public static ObjectId? ToObjectId(string? id)
    {
        if (!IsValidObjectId(id))
            return null;
            
        return ObjectId.Parse(id!);
    }
}
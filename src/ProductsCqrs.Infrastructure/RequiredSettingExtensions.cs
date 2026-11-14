using Microsoft.Extensions.Configuration;

namespace ProductsCqrs.Infrastructure;

public static class RequiredSettingExtensions
{
    public static string GetRequiredValue(this IConfiguration configuration, string name)
    {
        var value = configuration[name];

        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"Falta la variable de entorno '{name}'. Revise el archivo .env.");

        return value;
    }

    public static int GetRequiredNumber(this IConfiguration configuration, string name)
    {
        var value = configuration.GetRequiredValue(name);

        if (!int.TryParse(value, out var number))
            throw new InvalidOperationException($"La variable de entorno '{name}' debe ser un número entero. Revise el archivo .env.");

        return number;
    }
}

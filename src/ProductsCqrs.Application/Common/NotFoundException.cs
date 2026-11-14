namespace ProductsCqrs.Application.Common;

public class NotFoundException(string message) : Exception(message)
{
    public static NotFoundException Product(int id) => new($"No existe un producto con el id {id}.");
}

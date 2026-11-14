namespace ProductsCqrs.Application.Abstractions.Messaging;

public readonly record struct Unit
{
    public static Unit Value => default;
}

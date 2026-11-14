namespace ProductsCqrs.Application.Abstractions.Messaging;

public interface IRequest<out TResponse>;

public interface ICommand<out TResponse> : IRequest<TResponse>;

public interface ICommand : ICommand<Unit>;

public interface IQuery<out TResponse> : IRequest<TResponse>;

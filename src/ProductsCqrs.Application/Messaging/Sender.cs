using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using ProductsCqrs.Application.Abstractions.Messaging;

namespace ProductsCqrs.Application.Messaging;

internal sealed class Sender(IServiceProvider serviceProvider) : ISender
{
    private static readonly ConcurrentDictionary<Type, object> PipelinesByRequestType = new();

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var pipeline = (RequestPipeline<TResponse>)PipelinesByRequestType.GetOrAdd(
            request.GetType(),
            requestType => CreatePipeline(requestType, typeof(TResponse)));

        return pipeline.Run(request, serviceProvider, ct);
    }

    private static object CreatePipeline(Type requestType, Type responseType)
    {
        var pipelineType = typeof(TypedRequestPipeline<,>).MakeGenericType(requestType, responseType);
        return Activator.CreateInstance(pipelineType)!;
    }

    private abstract class RequestPipeline<TResponse>
    {
        public abstract Task<TResponse> Run(IRequest<TResponse> request, IServiceProvider services, CancellationToken ct);
    }

    private sealed class TypedRequestPipeline<TRequest, TResponse> : RequestPipeline<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public override Task<TResponse> Run(IRequest<TResponse> request, IServiceProvider services, CancellationToken ct)
        {
            var typedRequest = (TRequest)request;
            var handler = services.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
            var behaviors = services.GetServices<IPipelineBehavior<TRequest, TResponse>>().Reverse();

            RequestHandlerDelegate<TResponse> pipeline = () => handler.Handle(typedRequest, ct);

            foreach (var behavior in behaviors)
            {
                var next = pipeline;
                pipeline = () => behavior.Handle(typedRequest, next, ct);
            }

            return pipeline();
        }
    }
}

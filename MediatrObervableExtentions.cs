using MediatR;
using System;
using System.Reactive.Linq;
using System.Threading;

namespace Mediatr.ObservableExtentions
{
    public static class MediatrObservableExtentions
    {
        public static IObservable<TMediatorResult> Mediate<TInput, TMediatorRequest, TMediatorResult>
            (this IObservable<TInput> observable, IMediator mediatr, Func<TInput, TMediatorRequest> requestBuilder, CancellationToken cancellationToken = default)
            where TMediatorRequest : IRequest<TMediatorResult>
            where TMediatorResult : class, new()
        {
            if (observable == null)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            return Observable.Create<TMediatorResult>(o =>
            {
                return observable.Subscribe(input =>
                {
                    TMediatorRequest req = requestBuilder(input);
                    TMediatorResult result = mediatr.Send(req, cancellationToken).Result;
                    o.OnNext(result);
                });
            });
        }

        public static IDisposable Mediate<TInput, TMediatorNotification>
            (this IObservable<TInput> observable, IMediator mediatr, Func<TInput, TMediatorNotification> requestBuilder, CancellationToken cancellationToken = default)
            where TMediatorNotification : INotification
        {
            if (observable == null)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            return observable.Subscribe(input =>
            {
                TMediatorNotification req = requestBuilder(input);
                mediatr.Publish(req, cancellationToken);
            });
        }
    }
}

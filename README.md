# Mediatr.ObservableExtensions

This is a simple extensions that allow to bind an observable pipeline with MediatR handler (INotification or IRequest)

```csharp
MyObservable.Mediate(_mediatr, (observableType) => new mediatrNotification
{
    Id = observableType.Id
}, ct);
```

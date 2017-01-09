using System;
using System.Threading.Tasks;

namespace MMU.BoerseDownloader.WpfUI.Handlers.ExceptionHandling
{
    public interface IExceptionHandler
    {
        void HandledAction(Action action, Action finallyAction = null);

        Task HandledActionAsync(Func<Task> action, Action finallyAction = null);

        T HandledFunction<T>(Func<T> func);

        Task<T> HandledFunctionAsync<T>(Func<Task<T>> func);
    }
}
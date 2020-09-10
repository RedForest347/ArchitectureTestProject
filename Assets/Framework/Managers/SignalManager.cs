using UnityEngine;

namespace RangerV
{
    /// сделать автоматическое добавление процессинга сюда (мб)
    public class SignalManager<T> where T : ISignal, new()
    {
        public static SignalManager<T> Instance = new SignalManager<T>();

        delegate void SignalHandler(T arg);
        event SignalHandler signalHandler;

        public void AddReceiver(IReceive<T> receiver)
        {
            signalHandler += receiver.SignalHandler;
        }

        public void RemoveReceiver(IReceive<T> receiver)
        {
            signalHandler -= receiver.SignalHandler;
        }

        public void SendSignal(T arg)
        {
            signalHandler?.Invoke(arg);
            /*if (signalHandler != null)
                signalHandler(arg);
            else
                Debug.LogWarning("SignalHandler of " + typeof(T).Name + " is null");*/
        }

    }
}

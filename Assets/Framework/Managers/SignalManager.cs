using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RangerV
{
    public class SignalManager<T> where T : ISignal, new()
    {
        public static SignalManager<T> SignalManagerInstance = new SignalManager<T>();

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
            if (signalHandler != null)
                signalHandler(arg);
            else
                Debug.LogWarning("SignalHandler of " + typeof(T) + " is null");
        }

    }
}

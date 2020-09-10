using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RangerV
{
    public interface ICustomAwake
    {
        void OnAwake();
    }

    public interface ICustomStart
    {
        void OnStart();
    }

    public interface ICustomUpdate
    {
        void CustomUpdate();
    }

    public interface ICustomFixedUpdate
    {
        void CustomFixedUpdate();
    }

    public interface ICustomLateUpdate
    {
        void CustomLateUpdate();
    }

    public interface ICustomDisable
    {
        void OnCustomDisable();
    }

    public interface IStopStartProc
    {
        void Stop();

        void Start();
    }

    public interface IReceive<T> where T : ISignal, new() // убрать new
    {
        void SignalHandler(T arg);
    }

    public interface IComponent { }

    public interface ISignal { }
}

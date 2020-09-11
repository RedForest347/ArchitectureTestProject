using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;
//using NUnit.Framework;

namespace RangerV
{
    /// <summary>
    /// вопрос, как на время отключить процессинг?
    /// (просто так исключить его из Processings не вариант, т.к. исключение-включение в Dictionary имеет неприятные последствия)
    /// </summary>

    public class GlobalSystemStorage : MonoBehaviour
    {
        public static GlobalSystemStorage Instance { get => Singleton<GlobalSystemStorage>.Instance; }
        public bool debug_mod = false;

        Dictionary<Type, ProcessingBase> Processings;


        public static void Init()
        {
            Instance.Processings = new Dictionary<Type, ProcessingBase>();
        }

        public static T Add<T>() where T : ProcessingBase, new()
        {
            if (Instance.Processings.ContainsKey(typeof(T)))
            {
                Debug.LogError("Компонент " + typeof(T).Name + " уже добавлен в GlobalSystemStorage. он не может быть добавлен повторно");
                return null;
            }


            T processing = new T();
            Instance.Processings.Add(typeof(T), processing);

            if (processing is ICustomAwake)
                (processing as ICustomAwake).OnAwake();
            ManagerUpdate.Instance.AddTo(processing);

            if (Starter.initialized)
                if (processing is ICustomStart)
                    (processing as ICustomStart).OnStart();

            return processing;
        }

        public static T Get<T>() where T : ProcessingBase
        {
            ProcessingBase resolve;
            Instance.Processings.TryGetValue(typeof(T), out resolve);
            return (T)resolve;
        }

        public static void StartProcessings()
        {
            ProcessingBase[] prcessings = new ProcessingBase[Instance.Processings.Count];
            Instance.Processings.Values.CopyTo(prcessings, 0);

            for (int i = 0; i < Instance.Processings.Count; i++)
            {
                if (prcessings[i] is ICustomStart)
                    (prcessings[i] as ICustomStart).OnStart();
            }
        }

        public static void DisableProcessings()
        {
            Dictionary<Type, ProcessingBase> processings = Instance.Processings;
            ProcessingBase[] values = new ProcessingBase[processings.Count];
            processings.Values.CopyTo(values, 0);

            for (int i = 0; i < values.Length; i++)
                if (values[i] is ICustomDisable)
                    (values[i] as ICustomDisable).OnCustomDisable();

            processings = new Dictionary<Type, ProcessingBase>();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace RangerV
{
    public class GlobalSystemStorage
    {
        public static GlobalSystemStorage InstanceGSS = new GlobalSystemStorage();
        Dictionary<Type, ProcessingBase> InstanceProcessings = new Dictionary<Type, ProcessingBase>();


        public static T Add<T>() where T : ProcessingBase, new()
        {
            T processing = new T();
            InstanceGSS.InstanceProcessings.Add(typeof(T), processing);

            if (processing is ICustomAwake)
                (processing as ICustomAwake).OnAwake();   
            ManagerUpdate.InstanceManagerUpdate.AddTo(processing);

            return processing;
        }

        public static T Get<T>() where T : ProcessingBase
        {
            ProcessingBase resolve;
            InstanceGSS.InstanceProcessings.TryGetValue(typeof(T), out resolve);
            return (T)resolve;
        }

        //public static bool TryGet(Type t, out object resolve)
        //{
        //    bool b;
        //    b = InstanceGSS.InstanceProcessings.TryGetValue(t, out resolve);
        //    return b;
        //}

        public void StartProcessings()
        {
            ProcessingBase[] values = new ProcessingBase[InstanceProcessings.Count];
            InstanceProcessings.Values.CopyTo(values, 0);
            for (int i = 0; i < InstanceProcessings.Count; i++)
            {
                if(values[i] is ICustomStart)
                    (values[i] as ICustomStart).OnStart();
            }
        }

        public void ClearScene()
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RangerV
{
    public class ManagerUpdate : MonoBehaviour
    {
        private List<ICustomUpdate> updates = new List<ICustomUpdate>();
        private List<ICustomFixedUpdate> fixedupdates = new List<ICustomFixedUpdate>();
        private List<ICustomLateUpdate> lateupdates = new List<ICustomLateUpdate>();


        public static ManagerUpdate Instance { get => Singleton<ManagerUpdate>.Instance; }

        public static void Init()
        {

        }


        public void AddTo(object updateble)      //посылаем сюда object унаследованный от ICustomUpdate/ICustomFixedUpdate/ICustomLateUpdate
        {
            ManagerUpdate mngUpdate = Instance;

            if (updateble is ICustomUpdate)
                mngUpdate.updates.Add(updateble as ICustomUpdate);

            if (updateble is ICustomFixedUpdate)
                mngUpdate.fixedupdates.Add(updateble as ICustomFixedUpdate);

            if (updateble is ICustomLateUpdate)
                mngUpdate.lateupdates.Add(updateble as ICustomLateUpdate);

        }

        public void RemoveFrom(object updateble)     //посылаем сюда object унаследованный от ICustomUpdate/ICustomFixedUpdate/ICustomLateUpdate
        {
            ManagerUpdate mngUpdate = Instance;

            if (updateble is ICustomUpdate)
                mngUpdate.updates.Remove(updateble as ICustomUpdate);

            if (updateble is ICustomFixedUpdate)
                mngUpdate.fixedupdates.Remove(updateble as ICustomFixedUpdate);

            if (updateble is ICustomLateUpdate)
                mngUpdate.lateupdates.Remove(updateble as ICustomLateUpdate);
        }


        private void Update()
        {
            for (var i = 0; i < updates.Count; i++)
                updates[i].CustomUpdate();
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < fixedupdates.Count; i++)
                fixedupdates[i].CustomFixedUpdate();
        }

        private void LateUpdate()
        {
            for (var i = 0; i < lateupdates.Count; i++)
                lateupdates[i].CustomLateUpdate();
        }
    }
}

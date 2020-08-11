using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RangerV
{
    // В Starter выполняется главный Awake сцены. В Awake сначала создается ManagerUpdate, после происходит добавление processing'ов в GSS словарь
    // (происходит это в StarterSetup, переопределенном в специальном классе унаследованном от Starter). 
    // При добавлении в GSS словарь, на processing"е выполняется метод OnAwake (при наличии интерфейса IAwake).
    //
    // 1) создается ManagerUpdate
    // 2) происходит StarterSetup отвечающий за добавление processing'ов
    // 3) при добавлении на processing"е выполняется метод OnAwake
    //
    //


    public class Starter : MonoBehaviour        
    {
        public static bool initialized;

        private void Awake()
        {
            //Storage.CleanStorage();

            ManagerUpdate.Create();
            StarterSetup();
            initialized = true;
            Debug.Log("initialized:   " + initialized);

            EntitiesInitializing();
            GlobalSystemStorage.InstanceGSS.StartProcessings();
        }

        void EntitiesInitializing()
        {
            EntityBase[] objs = FindObjectsOfType<EntityBase>();
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].state.requireStarter)
                    objs[i].SetupAfterStarter();
            }
        }

        /// <summary>
        /// Setup стартера уровня. В стартере уровня выполняется добавление processing'ов в GSS
        /// </summary>
        public virtual void StarterSetup() { }

        protected virtual void OnDestroy()
        {
            initialized = false; 
            Debug.Log("initialized:   " + initialized);
        }
    }
}

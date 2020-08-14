using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace RangerV
{
    class Some2
    {
        static Some2()
        {
            Debug.Log("static конструктор 2");
        }
        public Some2()
        {
            //new GameObject("DDD");
            Debug.LogWarning(" isEditor = " + Application.isEditor/* + " genuine? = " + Application.genuine + "  " */);
            Debug.Log("конструктор 2");
        }
    }

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

        Some2 some2 = new Some2();

        static Starter()
        {
            Debug.LogWarning(/*"isPlaying = " + Application.isPlaying + */" isEditor = " + Application.isEditor/* + " genuine? = " + Application.genuine + "  " */);
            Debug.Log("static starter");
        }

        private void Awake()
        {
            ManagerUpdate.Init();
            GlobalSystemStorage.Init();
            StarterSetup();
            initialized = true;
            Debug.Log("initialized:   " + initialized);

            EntitiesInitializing();
            GlobalSystemStorage.Instance.StartProcessings();
        }

        private void OnEnable()
        {
            Debug.Log("Starter Enable");
            if (!initialized)
            {
                Debug.LogWarning("EntitiesInitializing повторно");
                ReInitialisation();
            }

            
        }

        void ReInitialisation() // происходит при реблде
        {
            ManagerUpdate.Init();
            GlobalSystemStorage.Init();
            StarterSetup();
            initialized = true;
            Group.UpdateAllGroups();
            Debug.LogWarning("Ребилд еще не доделан, некоторые функции могут не работать/работать некорректно");
            //Debug.Log("Re initialized:   " + initialized);
        }

        
        private void Update() // потом убрать
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("Was Update");

                for (int i = 0; i < EntityBase.entity_count; i++)
                {
                    if (EntityBase.EntityExists(i))
                        Debug.Log("сущность " + i + " существует");
                }

                Debug.Log("InstanceProcessings.Count = " + GlobalSystemStorage.Instance.InstanceProcessings.Count);
                Debug.Log("groups.Count = " + Group.groups.Count);
                //Debug.Log("groups.hachcode = " + Group.groups[0].hash_code_components);
            }
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

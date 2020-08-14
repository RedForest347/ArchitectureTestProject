using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RangerV
{
    abstract class Storage
    {
        /// <summary>
        /// по сути, является массивом и используется как массив (подробнее смотри в описании класса EntityData)
        /// </summary>
        protected EntityData entityData = new EntityData();

        public virtual event Action<int> OnAdd;
        public virtual event Action<int> OnRemove;

        protected static Dictionary<Type, Storage> StorageDictionary = new Dictionary<Type, Storage>();

        /// <summary>
        ///костыльная функция, которая вызывает инициализацию Storage<ComponentType>
        ///крайне не рекомендуется к использованию и надлежит ее заменить
        ///вызывается в случае если вызывается функция TryAddGroup(Type componentType, GroupBase group),
        ///где componentType - тип, который некода ранее не использовался и не инициализировался в Storage,
        ///т.е.если через код вызвать Storage.TryAddGroup(typeof(какойто левый компонент, который используется впервые и нигде не объявлялся))
        ///при нормальных условиях вызываться не должна
        /// </summary>
        /// <param name="ComponentType"></param>
        static void InitStorage(Type ComponentType)
        {
            Activator.CreateInstance(Type.GetType("RangerV.Storage`1[" + ComponentType + "]"));
        }

        public static T GetComponent<T>(int entity) where T : ComponentBase, IComponent, new()
        {
            return (T)Storage<T>.Instance.entityData[entity].component;
        }

        public static ComponentBase GetComponent(Type type, int entity)
        {
            return StorageDictionary.ContainsKey(type) ? StorageDictionary[type].GetComponent(entity) : null;
        }

        public static Storage GetStorage(Type ComponentType)
        {
            if (!StorageDictionary.ContainsKey(ComponentType))
                InitStorage(ComponentType);
            return StorageDictionary[ComponentType];
        }

        public static bool ContainsComponent<T>(int entity) where T : ComponentBase, IComponent, new()
        {
            return Storage<T>.Instance.entityData[entity].have_component;
        }

        public static bool ContainsComponent(Type componentType, int entity)
        {
            if (StorageDictionary.ContainsKey(componentType))
                return StorageDictionary[componentType].entityData[entity].have_component;
            return false;
        }

        public static void RemoveComponent(Type componentType, int entity)
        {
            if (StorageDictionary.ContainsKey(componentType))
                StorageDictionary[componentType].Remove(entity);
        }

        public static void RemoveComponent<T>(int entity) where T : ComponentBase, IComponent, new()
        {
            Storage<T>.Instance.Remove(entity);
        }

        public static bool AddComponent<T>(T componentBase, int entity) where T : ComponentBase, IComponent, new()
        {
            //Debug.Log(" componentBase = " + componentBase.GetType());
            if (!StorageDictionary.ContainsKey(componentBase.GetType()))///
                InitStorage(componentBase.GetType());

            return StorageDictionary[componentBase.GetType()].Add(componentBase, entity);
        }

        public static void Init<T>() where T : ComponentBase, IComponent, new()
        {
            //Storage<T>.Nothing();
            // если возникнут проблемы, следует раскомментировать, но вроде все норм

        }

        public static void RemoveFromAllStorages(int entity)
        {
            foreach(Storage storage in StorageDictionary.Values)
                storage.Remove(entity);
        }

        protected abstract ComponentBase GetComponent(int entity);

        public abstract void Remove(int entity);

        public abstract bool Add(ComponentBase component, int entity);


        /// <summary>
        /// данный класс является попыткой сделать свойство для entityData.
        /// нужня для динамческого расширения массива хранения данных о сущности (have_component, component)
        /// при это, обращение к нему такое же как если бы entityData был массивом
        /// </summary>
        protected class EntityData
        {
            public entityData[] _entityData = new entityData[50];

            public entityData this[int index]
            {
                get
                {
                    if (_entityData.Length <= index || _entityData[index] == null)
                        return new entityData();
                    return _entityData[index];
                }
                set
                {
                    if (_entityData.Length <= index)
                        Array.Resize(ref _entityData, index + 10);
                    _entityData[index] = value;
                }
            }

            public class entityData
            {
                public bool have_component;
                public ComponentBase component;

                public entityData() : this(false, null) { }

                public entityData(bool have_component, ComponentBase component)
                {
                    this.have_component = have_component;
                    this.component = component;
                }

                public void SetDefault()
                {
                    have_component = false;
                    component = null;
                }
            }
        }
    }


    class Storage<T> : Storage where T : ComponentBase, IComponent, new()
    {
        public static Storage<T> Instance;

        public override event Action<int> OnAdd;
        public override event Action<int> OnRemove;

        static Storage()
        {
            if (typeof(T) == typeof(ComponentBase))
                Debug.LogError("создан Storage с типом ComponentBase");

            Storage<T> storage = new Storage<T>();
            StorageDictionary.Add(typeof(T), storage);
            Instance = storage;
        }

        /// <summary>
        /// функция - мем
        /// </summary>
        public static void Nothing()
        {
            /*нужна для инициализации Storage если ее еще не было (например, в случае добавления исключений)*/
        }

        protected override ComponentBase GetComponent(int entity)
        {
            return entityData[entity].have_component ? entityData[entity].component : null;
        }

        public override bool Add(ComponentBase component, int entity)
        {
            if (entityData[entity].have_component)
                Debug.LogWarning("сущность " + entity + " уже имеет компонент " + (T)component + ". он будет перезаписан");


            entityData[entity] = new EntityData.entityData(true, (T)component);

            //Debug.Log("к сущности " + entity + " добавлен компонент " + (T)component/* + " в Storage " + typeof(T)*/);
            OnAdd?.Invoke(entity);
            //Debug.Log("у сущности " + entity + " добавился компонент " + typeof(T));

            //Group.AddToGroups(entity);
            return true;
        }

        public override void Remove(int entity)
        {
            if (!entityData[entity].have_component)
                return;

            //UnityEngine.Object.Destroy(entityData[entity].component);
            entityData[entity].SetDefault();

            OnRemove?.Invoke(entity);
        }
    }
}
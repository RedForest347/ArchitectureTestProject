using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace RangerV
{
    /// <summary>
    /// Базовый класс Entity
    /// </summary>
    public abstract class EntityBase : MonoBehaviour
    {
        public static event Action<int> OnCreateEntity;
        public static event Action<int> OnCreateEntityID;

        public static event Action<int> OnDestroyEntity;
        


        /// <summary>
        /// нулевой элемент не должен быть занят
        /// </summary>
        static EntityBase[] Entities = new EntityBase[50];
        static Stack<int> freeID = new Stack<int>(25);
        static int nextMax = 1;
        public static int entity_count { get => nextMax; }

        public int entity;

        [SerializeField]
        public List<ComponentBase> Components = new List<ComponentBase>();
        public EntityState state;

        public static EntityBase GetEntity(int entity)
        {
            return Entities[entity];
        }

        //разобрать причинно-следственные связи
        /// <summary>
        /// ----------до----------
        /// 
        /// попытаюсь расписать хронологию событий:
        /// 
        /// 1) Awake
        ///     CreateEntity(this); // - присваевается номер сущности
        ///     SetupAfterStarter()
        ///         state.requireStarter = false;
        ///         Setup();
        ///             добавление компонентов
        ///         
        ///  !!!!!!!!!с этого момента с сущностью могут работать процессинги!!!!!!!!!
        ///  
        ///         OnEnable();
        ///             state.enabled = true;
        ///             ManagerUpdate.InstanceManagerUpdate.AddTo(this); // - добавление себя в ManagerUpdate
        ///             Storage.AddToAllStorages(Components, entity); // нужно при повторном включении 
        ///         state.initialized = true;
        ///      
        /// 2) OnEnable
        ///     state.enabled = true;
        ///     ManagerUpdate.InstanceManagerUpdate.AddTo(this); // - добавление себя в ManagerUpdate (повторно)
        ///     Storage.AddToAllStorages(Components, entity); // нужно при повторном включении 
        /// 
        /// 3) Start
        ///     отсутствует
        ///     
        /// 4) OnDisable
        ///     OnDeactivate();
        ///         state.enabled = false;
        ///         ManagerUpdate.InstanceManagerUpdate.RemoveFrom(this); // - удаление себя из ManagerUpdate
        ///         Storage.RemoveFromAllStorages(entity);
        ///         
        /// 5) OnEnable
        ///     state.enabled = true;
        ///     ManagerUpdate.InstanceManagerUpdate.AddTo(this); // - добавление себя в ManagerUpdate
        ///     Storage.AddToAllStorages(Components, entity); // нужно при повторном включении 
        /// 
        /// 6) OnDisable
        ///     OnDeactivate();
        ///         state.enabled = false;
        ///         ManagerUpdate.InstanceManagerUpdate.RemoveFrom(this); // - удаление себя из ManagerUpdate
        ///         Storage.RemoveFromAllStorages(entity);
        /// 
        /// 7) OnDestroy
        ///     state.in_game = false;
        ///     OnDeactivate();
        ///         state.enabled = false;
        ///         ManagerUpdate.InstanceManagerUpdate.RemoveFrom(this);
        ///         Storage.RemoveFromAllStorages(entity);
        ///     freeID.Push(entity);
        ///     Entities[entity] = null;
        /// 
        /// </summary>
        public void Awake()
        {
            state.runtime = true;
            CreateEntityID(this);
            OnCreateEntity?.Invoke(entity);

            if (!Starter.initialized)
                state.requireStarter = true;
            else
                SetupAfterStarter();
        }

        private void OnEnable()
        {
            
            if (state.requireStarter)
                return;
            if (state.enabled)
                return;

            //Debug.Log("Enable " + entity);

            state.enabled = true;
            OnActivate();
            Debug.Log("entity " + entity + " --active--");
        }

        private void OnDisable()
        {
            OnDeactivate();
        }

        private void OnDestroy()
        {
            state.runtime = false;
            freeID.Push(entity);
            Entities[entity] = null;
            entity = -1;
        }

        void OnActivate()
        {
            for (int i = 0; i < Components.Count; i++)
                Storage.AddComponent(Components[i], entity);
            //Group.UpdateInGroups(entity);
            //OnCreateEntity(entity);
        }

        public void OnDeactivate()
        {
            OnDestroyEntity?.Invoke(entity);
            state.enabled = false;
            ManagerUpdate.InstanceManagerUpdate.RemoveFrom(this);
            Storage.RemoveFromAllStorages(entity);
            //Group.RemoveFromAllGroups(entity);
        }

        


        #region MAIN


        public void SetupAfterStarter()
        {
            state.requireStarter = false;
            /*for (int i = 0; i < Components.Count; i++)
                AddFromStartList(Components[i]);*/
            
            
            OnEnable();
            Setup();
            state.initialized = true;
        }


        void CreateEntityID(EntityBase entityBase)
        {
            if (Entities.Length <= nextMax)
                Array.Resize(ref Entities, Entities.Length + 10);

            if (freeID.Count > 0)
                entityBase.entity = freeID.Pop();
            else
                entityBase.entity = nextMax++;

            Entities[entityBase.entity] = entityBase;
            OnCreateEntityID?.Invoke(entityBase.entity);
        }

        #endregion MAIN

        #region ADD/REMOVE

        public T Add<T>() where T : ComponentBase, IComponent, new()
        {
            return (T)Add(typeof(T));
        }

        public ComponentBase Add(Type componentType)
        {
            if (componentType == typeof(ComponentBase))
            {
                Debug.LogError("Попытка добавить ComponentBase");
                return null;
            }

            if (!state.runtime)
                return Add_InEditorMode(componentType);

            if (Storage.ContainsComponent(componentType, entity))
            {
                Debug.LogError("попытка добавить уже существующий компонент " + componentType + " к сущности " + entity);
                return null; // или компонент, который уже существует
            }

            ComponentBase component = (ComponentBase)gameObject.AddComponent(componentType);

            if ((component as ICustomAwake) != null)
                (component as ICustomAwake).OnAwake();

            Components.Add(component);
            Storage.AddComponent(component, entity);
            //Group.UpdateInGroups(entity);
            return component;
        }

        /// <summary>
        /// общая чать для добавление компонента в EditorMode
        /// </summary>
        /// <param name="componentType">тип компонента, который требуется добавить</param>
        /// <returns>компонент, если он был добавлен, null если не был</returns>
        ComponentBase Add_InEditorMode(Type componentType)
        {
            if (Components.Any(comp => comp.GetType() == componentType))
            {
                Debug.LogError("попытка добывления повторяющегося компонента " + componentType + " на сущность. Компонент добавлен не будет");
                return null;
            }

            ComponentBase _component = (ComponentBase)gameObject.AddComponent(componentType);
            Components.Add(_component);
            return _component;
        }


        void AddFromStartList(ComponentBase component)
        {
            if (Storage.ContainsComponent(component.GetType(), entity))
                return;

            if ((component as ICustomAwake) != null)
                (component as ICustomAwake).OnAwake();

            Storage.AddComponent(component, entity);
        }

        public bool RemoveComponent<T>() where T : ComponentBase, IComponent, new()
        {
            if (!state.runtime)
                return RemoveInEditorMode(typeof(T));

            if (!Storage.ContainsComponent<T>(entity))
                return false;

            Destroy(GetEntityComponent<T>());
            //Storage<T>.StorageForType.Remove(entity);
            Storage.RemoveComponent<T>(entity); //багs
            //Group.UpdateInGroups(entity);
            RemoveComponentFromLists(typeof(T));
            return true;
        }

        public bool RemoveComponent(Type componentType)
        {
            if (!state.runtime)
                return RemoveInEditorMode(componentType);

            if (!Storage.ContainsComponent(componentType, entity))
                return false;

            Destroy(GetEntityComponent(componentType));
            Storage.RemoveComponent(componentType, entity);
            //Group.UpdateInGroups(entity);
            RemoveComponentFromLists(componentType);
            return true;
        }

        bool RemoveComponentFromLists(Type componentType)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].GetType() == componentType)
                {
                    Components.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        bool RemoveInEditorMode(Type componentType)
        {
            int index = IndexOf(componentType);

            if (index == -1)
                return false;

            DestroyImmediate(Components[index]);
            Components.RemoveAt(index);
            return true;
        }

        int IndexOf(Type ComponentType)
        {
            for (int i = 0; i < Components.Count; i++)
                if (Components[i].GetType() == ComponentType)
                    return i;
            return -1;
        }

        #endregion

        #region GetComponent

        public List<ComponentBase> GetAllComponents()
        {
            List<ComponentBase>  componentBases = new List<ComponentBase>(Components.Count);

            for (int i = 0; i < Components.Count; i++)
                componentBases.Add(Components[i]);

            return componentBases;
        }

        public T GetEntityComponent<T>() where T : ComponentBase, IComponent, new()
        {
            return Storage.GetComponent<T>(entity);
        }

        public ComponentBase GetEntityComponent(Type type)
        {
            return Storage.GetComponent(type, entity);
        }

        public bool ContainsEntityComponent<T>() where T : ComponentBase, IComponent, new()
        {
            return Storage.ContainsComponent<T>(entity) ? true : false;
        }

        public bool ContainsEntityComponent(Type type)
        {
            return Storage.ContainsComponent(type, entity) ? true : false;
        }

        #endregion GetComponent

        public virtual void Setup() { }

    }
}

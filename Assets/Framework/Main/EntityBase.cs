﻿using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace RangerV
{

    /// <summary>
    /// костыль. его суть - хранить статическую дату для  EntityBase в нестатическом виде для нормального
    /// восстановления после ребилдинга. хорошо бы переделать в более правильный вид
    /// </summary>
    public class EntityBaseStatic : MonoBehaviour
    {
        protected static EntityBaseStatic Instance { get => Singleton<EntityBaseStatic>.Instance; }

        public EntityBase[] Entities; //сделать расширение массива при ребилде
        public Stack<int> freeID = new Stack<int>(25);
        public int nextMax = 1;
    }

    /// <summary>
    /// Базовый класс Entity
    /// </summary>
    public abstract class EntityBase : EntityBaseStatic
    {
        public static event Action<int> OnCreateEntityID;
        public static event Action<int> OnDestroyEntity;
        public static event Action<int> OnActivateEntity;

        new static EntityBase[] Entities { get => Instance.Entities; }
        new static Stack<int> freeID { get => Instance.freeID; }
        new static int nextMax { get => Instance.nextMax; set => Instance.nextMax = value; } 

        /// <summary>
        /// нулевой элемент не должен быть занят
        /// </summary>

        public static int entity_count { get => Instance.nextMax; }

        public int entity;

        [SerializeField]
        public List<ComponentBase> Components = new List<ComponentBase>();
        public EntityState state;

        public static EntityBase GetEntity(int entity)
        {
            return Entities[entity];
        }

        public static bool EntityExists(int entity)
        {
            return Entities[entity] != null;
        }

        //разобрать причинно-следственные связи //устарело
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
            //Debug.Log("Awake");
            state.runtime = true;
            CreateEntityID();

            if (!Starter.initialized)
                state.requireStarter = true;
            else
                SetupAfterStarter();
        }

        private void OnEnable()
        {
            Debug.Log("EntityBase Enable");
            //Debug.Log("requireStarter = " + state.requireStarter + " enabled = " + state.enabled);
            if (state.requireStarter)
                return;
            if (state.enabled)
                return;

            state.enabled = true;

            Entities[entity] = this;

            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is ICustomAwake)
                    ((ICustomAwake)Components[i]).OnAwake();

                ManagerUpdate.Instance.AddTo(Components[i]);
                Storage.AddComponent(Components[i], entity);
            }

            OnActivateEntity?.Invoke(entity);

            //Debug.Log("entity " + entity + " (" + gameObject.GetInstanceID() + ") --Activate--"); 
        }

        private void OnDisable()
        {
            //Debug.Log("Disable");
            OnDeactivate();
        }

        private void OnDestroy()
        {
            state.runtime = false;
            freeID.Push(entity);
            Entities[entity] = null;
            entity = -1;
        }

        public void OnDeactivate()
        {
            OnDestroyEntity?.Invoke(entity);
            state.enabled = false;
            ManagerUpdate.Instance.RemoveFrom(this);
            Storage.RemoveFromAllStorages(entity);
            Entities[entity] = null;
            //Debug.Log("entity " + entity + " (" + gameObject.GetInstanceID() + ") --Deactivate--");
        }


        #region MAIN


        public void SetupAfterStarter()
        {
            state.requireStarter = false;
            OnEnable();
            Setup();
            state.initialized = true;
        }

        void CreateEntityID()
        {
            if (Entities.Length <= nextMax)
                Array.Resize(ref Instance.Entities, Entities.Length + 10);

            if (freeID.Count > 0)
                entity = freeID.Pop();
            else
                entity = nextMax++;

            Entities[entity] = this;
            OnCreateEntityID?.Invoke(entity);
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
                Debug.LogWarning("попытка добавить уже существующий компонент " + componentType + " к сущности " + entity + " компонент добавлен не будет");
                return null;
            }

            ComponentBase component = (ComponentBase)gameObject.AddComponent(componentType);

            if (component is ICustomAwake)
                ((ICustomAwake)component).OnAwake();

            ManagerUpdate.Instance.AddTo(component);

            Components.Add(component);
            Storage.AddComponent(component, entity);
            
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

        public bool RemoveComponent<T>() where T : ComponentBase, IComponent, new()
        {
            if (!state.runtime)
                return RemoveInEditorMode(typeof(T));

            if (!Storage.ContainsComponent<T>(entity))
                return false;

            Destroy(GetEntityComponent<T>());
            Storage.RemoveComponent<T>(entity);
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

        #endregion ADD/REMOVE

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

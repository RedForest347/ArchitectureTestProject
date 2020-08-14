using System;
using System.Collections;
using System.Collections.Generic;
using Stopwatch = System.Diagnostics.Stopwatch;
using System.Linq;
using UnityEngine;

namespace RangerV
{
    /// <summary>
    /// создание группы:
    /// Group group = Group.Create(new ComponentsList<SomeComp1, SomeComp2>() -- компоненты, new ComponentsList<SomeComp3, SomeComp4> -- исключения);
    /// </summary>
    /// <ploblem>
    /// нет удаления группы, т.е. группа всегда останется в листе групп, не будет отписки от событий и т.д.
    /// </ploblem>
    public class Group
    {

        #region Static Func

        public static List<Group> groups = new List<Group>(); // убрать public

        public static Group Create(ComponentsList Components, Action<int> OnAdd = null)
        {
            return Create(Components, null, OnAdd);
        }


        /// <summary>
        /// подразумевается, что Components и Exceptions будут созданы с помощью new ComponentsList<>()
        /// в противном случае, могут возникнуть ошибки
        /// </summary>
        /// <param name="Components">компоненты, которые обязаны быть в группе</param>
        /// <param name="Exceptions">компоненты, которых обязано не быть в группе</param>
        /// <returns>возвращает ссылку на созданную/существующую группу</returns>
        public static Group Create(ComponentsList Components, ComponentsList Exceptions, Action<int> OnAdd = null)
        {
            if (Components == null)
                Debug.LogError("Components == null");

            if (Components.types.GroupBy(g => g).Where(w => w.Count() > 1).Count() != 0)
                Debug.LogError("повторяющиеся компоненты при создании группы. Группа будет создана");

            if (Exceptions != null && Exceptions.types.GroupBy(g => g).Where(w => w.Count() > 1).Count() != 0)
                Debug.LogError("повторяющиеся исключения при создании группы. Группа будет создана");

            if (Exceptions == null)
                Exceptions = new ComponentsList();

            Group group = new Group(Components.types, Exceptions.types);

            Group exist_group = GetExistGroup(group);

            if (exist_group != null)
                return exist_group;

            group.OnAddEntity += OnAdd;

            group.InitDictionary();
            group.InitEvents();

            groups.Add(group);

            return group;
        }

        public static void UpdateAllGroups()
        {
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].InitDictionary();
            }
        }

        static Group GetExistGroup(Group group)
        {
            for (int i = 0; i < groups.Count; i++)
                if (groups[i] == group)
                    return groups[i];

            return null;
        }

        public static bool operator == (Group group1, Group group2)
        {
            if (group1 is null || group2 is null)
                return ReferenceEquals(group1, group2);

            return group1.hash_code_components == group2.hash_code_components && group1.hash_code_exceptions == group2.hash_code_exceptions;
        }

        public static bool operator != (Group group1, Group group2)
        {
            return !(group1 == group2);
        }


        #endregion Static Func


        public int entities_count { get; private set; }

        Dictionary<int, EntContainer> EntitiesDictionary;

        List<Type> Components;
        List<Type> Exceptions;

        long hash_code_components;
        long hash_code_exceptions;

        public event Action<int> OnAddEntity;
        public event Action<int> OnRemoveEntity;

        private Group(List<Type> Components) : this(Components, new List<Type>(0)) { }

        private Group(List<Type> Components, List<Type> Exceptions)
        {
            if (Components == null) 
                Debug.LogError("при создании группы, лист Components пуст");
            if (Exceptions == null)
                Debug.LogError("при создании группы, лист Exceptions пуст");

            //EntitiesDictionary = new Dictionary<int, EntContainer>();
            entities_count = 0;
            this.Components = Components;
            this.Exceptions = Exceptions;

            for (int i = 0; i < Components.Count; i++)
                hash_code_components += Components[i].GetHashCode();
            for (int i = 0; i < Exceptions.Count; i++)
                hash_code_exceptions += Exceptions[i].GetHashCode();
        }

        void AddEntity(int entity)
        {
            EntitiesDictionary[entity].was_added = true;
            entities_count++;
            OnAddEntity?.Invoke(entity);
        }

        void RemoveEntity(int entity)
        {
            EntitiesDictionary[entity].was_added = false;
            entities_count--;
            OnRemoveEntity?.Invoke(entity);
        }

        public List<Type> GetComponentTypes()
        {
            List<Type> componentsTypes = new List<Type>(Components.Count);
            for (int i = 0; i < Components.Count; i++)
                componentsTypes.Add(Components[i]);
            return componentsTypes;
        }

        public List<Type> GetExceptionTypes()
        {
            List<Type> exceptionsTypes = new List<Type>(Exceptions.Count);
            for (int i = 0; i < Exceptions.Count; i++)
                exceptionsTypes.Add(Exceptions[i]);
            return exceptionsTypes;
        }

        void InitDictionary()
        {
            int length = EntityBase.entity_count;

            //Debug.Log("entity_count = " + EntityBase.entity_count);

            EntitiesDictionary = new Dictionary<int, EntContainer>();

            for (int ent = 0; ent < length; ent++)
            {
                EntitiesDictionary.Add(ent, EntContainer.Empty);

                if (EntityBase.GetEntity(ent) != null)
                {
                    EntContainer entContainer = new EntContainer(ent, Components.Count);

                    for (int comp = 0; comp < Components.Count; comp++)
                        if (Storage.ContainsComponent(Components[comp], ent))
                            entContainer.remains_components--;

                    for (int exc = 0; exc < Exceptions.Count; exc++)
                        if (Storage.ContainsComponent(Exceptions[exc], ent))
                            entContainer.remains_exceptions++;

                    EntitiesDictionary[ent] = entContainer;

                    if (entContainer.ShouldJoin())
                        AddEntity(ent);
                }
            }
        }

        void InitEvents()
        {
            EntityBase.OnBeforeAddComponents += OnCreateNewEntityID;


            for (int comp = 0; comp < Components.Count; comp++)
            {
                Storage.GetStorage(Components[comp]).OnAdd += OnAddComponent;
                Storage.GetStorage(Components[comp]).OnRemove += OnRemoveComponent;
            }

            for (int exceptions = 0; exceptions < Exceptions.Count; exceptions++)
            {
                Storage.GetStorage(Exceptions[exceptions]).OnAdd += OnAddException;
                Storage.GetStorage(Exceptions[exceptions]).OnRemove += OnRemoveException;
            }
        }

        void FinalEvents() // куда нибудь запихнуть
        {
            if (Components == null)
                return;

            EntityBase.OnCreateEntityID -= OnCreateNewEntityID;

            for (int comp = 0; comp < Components.Count; comp++)
            {
                Storage.GetStorage(Components[comp]).OnAdd -= OnAddComponent;
                Storage.GetStorage(Components[comp]).OnRemove -= OnRemoveComponent;
            }

            for (int exceptions = 0; exceptions < Exceptions.Count; exceptions++)
            {
                Storage.GetStorage(Exceptions[exceptions]).OnAdd -= OnAddException;
                Storage.GetStorage(Exceptions[exceptions]).OnRemove -= OnRemoveException;
            }
        }

        void OnCreateNewEntityID(int entity)
        {
            if (!EntitiesDictionary.ContainsKey(entity))
            {
                EntitiesDictionary.Add(entity, null);
            }

            EntContainer entContainer = new EntContainer(entity, Components.Count);

            for (int comp = 0; comp < Components.Count; comp++)
                if (Storage.ContainsComponent(Components[comp], entity))
                    entContainer.remains_components--;

            for (int exc = 0; exc < Exceptions.Count; exc++)
                if (Storage.ContainsComponent(Exceptions[exc], entity))
                    entContainer.remains_exceptions++;

            EntitiesDictionary[entity] = entContainer;

            if (EntitiesDictionary[entity].ShouldJoin())
                AddEntity(entity);
        }

        void OnAddComponent(int entity)
        {
            if (!EntitiesDictionary.ContainsKey(entity))
                Debug.LogError("в библиотеке " + GetHashCode() + " не найдена сущность " + entity);

            EntitiesDictionary[entity].remains_components--;

            if (EntitiesDictionary[entity].ShouldJoin())
                AddEntity(entity);
        }
        void OnRemoveComponent(int ent)
        {
            /*Debug.Log("EntitiesDictionary (" + (hash_code_components % 256) + ") contains: ");
            foreach (var item in EntitiesDictionary.Values)
            {
                Debug.Log(item.entity);
            }

            Debug.Log("end");*/

            EntitiesDictionary[ent].remains_components++;

            if (EntitiesDictionary[ent].was_added)
                RemoveEntity(ent);
        }
        void OnAddException(int ent)
        {
            EntitiesDictionary[ent].remains_exceptions++;
            if (EntitiesDictionary[ent].was_added)
                RemoveEntity(ent);
        }
        void OnRemoveException(int ent)
        {
            EntitiesDictionary[ent].remains_exceptions--;
            if (EntitiesDictionary[ent].ShouldJoin())
                AddEntity(ent);
        }

        public void Delete() // функция, которая обнуляет группу
        {
            // придумать что делать если на одну группу ссылаются несколько ссылок
            /*FinalEvents();
            Entities = null;
            Components = Exceptions = null;
            hash_code_components = hash_code_exceptions = 0;
            groups.Remove(this);*/
        }

        public bool Contains(int entity)
        {
            return EntitiesDictionary.ContainsKey(entity);
        }

        #region Equals/HashCode/Enumerator


        public override bool Equals(object obj)
        {
            return obj.GetType() != GetType() ? false : this == (Group)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public IEnumerator<int> GetEnumerator()
        {
            EntContainer[] array = EntitiesDictionary.Values.ToArray();

            for (int i = 0; i < array.Length; i++)
                if (array[i].was_added)
                    yield return array[i].entity; 
        }

        #endregion Equals/HashCode/Enumerator


        class EntContainer 
        {
            public int remains_components; // сколько компонентов осталось до полного набора
            public int remains_exceptions; // сколько осталось лишних исключений
            public int entity;
            public bool was_added; // была ли сущность добавлена в группу
            int start_num_of_remains_components;

            public static EntContainer Empty { get => new EntContainer(); }

            EntContainer() : this(-2, -2) { }

            public EntContainer(int entity, int num_of_components)
            {
                remains_components = num_of_components;
                remains_exceptions = 0;
                this.entity = entity;
                was_added = false;
                start_num_of_remains_components = num_of_components;
            }

            public bool ShouldJoin()
            {
                if (remains_components < 0)
                    Debug.LogError("remains_components меньше нуля " + remains_components);
                if (remains_exceptions < 0)
                    Debug.LogError("remains_exceptions меньше нуля " + remains_exceptions);


                return remains_components == 0 && remains_exceptions == 0 && entity >= 0;
            }

            public void Zeroing()
            {
                remains_components = start_num_of_remains_components;
                remains_exceptions = 0;
                entity = -1;
                was_added = false;
            }

            public static explicit operator int(EntContainer container)
            {
                return container.entity;
            }
        }
    }

    #region ComponentsList

    public class ComponentsList 
    {
        public List<Type> types;
        public ComponentsList Empty { get => new ComponentsList(); }

        public ComponentsList()
        {
            types = new List<Type>();
        }
    }

    public class ComponentsList<T1> : ComponentsList 
                                        where T1 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            Storage.Init<T1>();
            types = new List<Type>
            {
                typeof(T1)
            };
        }
    }

    public class ComponentsList<T1, T2> : ComponentsList
                                        where T1 : ComponentBase, IComponent, new()
                                        where T2 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            Storage.Init<T1>();
            Storage.Init<T2>();
            types = new List<Type>
            {
                typeof(T1),
                typeof(T2)
            };
        }
    }

    public class ComponentsList<T1, T2, T3> : ComponentsList
                                       where T1 : ComponentBase, IComponent, new()
                                       where T2 : ComponentBase, IComponent, new()
                                       where T3 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            Storage.Init<T1>();
            Storage.Init<T2>();
            Storage.Init<T3>();
            types = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3)
            };
        }
    }

    public class ComponentsList<T1, T2, T3, T4> : ComponentsList
                                       where T1 : ComponentBase, IComponent, new()
                                       where T2 : ComponentBase, IComponent, new()
                                       where T3 : ComponentBase, IComponent, new()
                                       where T4 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            Storage.Init<T1>();
            Storage.Init<T2>();
            Storage.Init<T3>();
            Storage.Init<T4>();
            types = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4)
            };
        }
    }

    public class ComponentsList<T1, T2, T3, T4, T5> : ComponentsList
                                      where T1 : ComponentBase, IComponent, new()
                                      where T2 : ComponentBase, IComponent, new()
                                      where T3 : ComponentBase, IComponent, new()
                                      where T4 : ComponentBase, IComponent, new()
                                      where T5 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            Storage.Init<T1>();
            Storage.Init<T2>();
            Storage.Init<T3>();
            Storage.Init<T4>();
            Storage.Init<T5>();
            types = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4),
                typeof(T5)
            };
        }
    }

    public class ComponentsList<T1, T2, T3, T4, T5, T6> : ComponentsList
                                      where T1 : ComponentBase, IComponent, new()
                                      where T2 : ComponentBase, IComponent, new()
                                      where T3 : ComponentBase, IComponent, new()
                                      where T4 : ComponentBase, IComponent, new()
                                      where T5 : ComponentBase, IComponent, new()
                                      where T6 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            Storage.Init<T1>();
            Storage.Init<T2>();
            Storage.Init<T3>();
            Storage.Init<T4>();
            Storage.Init<T5>();
            Storage.Init<T6>();
            types = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4),
                typeof(T5),
                typeof(T6)
            };
        }
    }

    #endregion ComponentsList


}

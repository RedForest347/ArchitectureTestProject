using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;
using Stopwatch = System.Diagnostics.Stopwatch;

public class TestAllSystem : MonoBehaviour
{
    Group group1, group2, group3, group4, group5, group6, group8;

    List<GameObject> gameObjects;
    List<Group> groups;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            FullTest();
    }

    void FullTest()
    {
        Debug.LogWarning("Начат тест фреймворка");
        Stopwatch time = Stopwatch.StartNew();
        bool successfull = true;

        group1 = Group.Create(new ComponentsList<CompTest1, CompTest2>());
        group2 = Group.Create(new ComponentsList<CompTest1, CompTest2>());
        group3 = Group.Create(new ComponentsList<CompTest1>(), new ComponentsList<CompTest1>());
        group4 = Group.Create(new ComponentsList<CompTest1>(), new ComponentsList<CompTest2>());
        group5 = Group.Create(new ComponentsList<CompTest1>());
        group6 = Group.Create(new ComponentsList<CompTest7>());
        group8 = Group.Create(new ComponentsList(), new ComponentsList<CompTest1>());

        gameObjects = new List<GameObject>(); // лист всех GameObject задействованных в тесте
        groups = new List<Group>() { group1, group2, group3, group4, group5, group6 }; // лист всех Group задействованных в тесте


        if (group1.entities_count + group2.entities_count + group3.entities_count + group4.entities_count + group5.entities_count != 0)
            SendErrorMessage("При начале теста в группах находятся сущности");

        #region correct add component test

        GameObject ent1 = new GameObject();
        gameObjects.Add(ent1);
        ent1.AddComponent<TestEntity1>();
        if (ent1.GetComponent<TestEntity1>().GetEntityComponent<CompTest1>() == null)
            SendErrorMessage("Компонент не был добавлен");
        if (ent1.GetComponent<TestEntity1>().GetEntityComponent<CompTest2>() != null)
            SendErrorMessage("Ошибочное добавление компонента");
        if (ent1.GetComponent<TestEntity1>().GetEntityComponent<CompTest3>() != null)
            SendErrorMessage("Ошибочное добавление компонента");
        if (ent1.GetComponent<TestEntity1>().GetEntityComponent<CompTest4>() != null)
            SendErrorMessage("Ошибочное добавление компонента");

        GameObject ent2 = new GameObject();
        gameObjects.Add(ent2);
        ent2.AddComponent<TestEntity2>();
        if (ent2.GetComponent<TestEntity2>().GetEntityComponent<CompTest1>() == null)
            SendErrorMessage("Компонент не был добавлен");
        if (ent2.GetComponent<TestEntity2>().GetEntityComponent<CompTest2>() == null)
            SendErrorMessage("Компонент не был добавлен");
        if (ent2.GetComponent<TestEntity2>().GetEntityComponent<CompTest3>() == null)
            SendErrorMessage("Компонент не был добавлен");
        if (ent2.GetComponent<TestEntity2>().GetEntityComponent<CompTest4>() == null)
            SendErrorMessage("Компонент не был добавлен");

        GameObject ent3 = new GameObject();
        gameObjects.Add(ent3);
        ent3.AddComponent<TestEntity3>();
        if (ent3.GetComponent<TestEntity3>().GetEntityComponent<CompTest1>() == null)
            SendErrorMessage("Компонент не был добавлен");
        if (ent3.GetComponent<TestEntity3>().GetEntityComponent<CompTest2>() == null)
            SendErrorMessage("Компонент не был добавлен");
        if (ent3.GetComponent<TestEntity3>().GetEntityComponent<CompTest3>() == null)
            SendErrorMessage("Компонент не был добавлен");
        if (ent3.GetComponent<TestEntity3>().GetEntityComponent<CompTest4>() == null)
            SendErrorMessage("Компонент не был добавлен");

        #endregion correct add component test

        #region correct num of entityes in group test

        if (group1.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе.");
        if (group2.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group3.entities_count != 0)
            SendErrorMessage("Игнорирование исключений");
        if (group4.entities_count != 1)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group5.entities_count != 3)
            SendErrorMessage("Неправильное количество сущностей в группе");

        if (ent1.GetComponent<TestEntity1>().GetAllComponents().Count != 1)
            SendErrorMessage("Добавление повторяющихся компонентов");

        if (ent2.GetComponent<TestEntity2>().GetAllComponents().Count != 4)
            SendErrorMessage("Неправильное добавление компонентов");

        ent2.GetComponent<EntityBase>().RemoveCmp<CompTest2>();
        if (group5.entities_count != 3)
            SendErrorMessage("Некорректное поведение групп при удалении компонента");
        ent2.GetComponent<EntityBase>().AddCmp<CompTest2>();


        ent1.GetComponent<EntityBase>().AddCmp<CompTest2>();
        if (group1.entities_count != 3)
            SendErrorMessage("Некорректное поведение групп при добавлении компонента");
        ent1.GetComponent<EntityBase>().RemoveCmp<CompTest2>();

        GameObject ent4 = new GameObject();
        gameObjects.Add(ent4);
        ent4.AddComponent<TestEntity1>();

        if (group1.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group2.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group3.entities_count != 0)
            SendErrorMessage("Игнорирование исключений");
        if (group4.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group5.entities_count != 4)
            SendErrorMessage("Неправильное количество сущностей в группе");

        ent1.GetComponent<TestEntity1>().RemoveCmp(typeof(CompTest1));
        if (ent1.GetComponent<TestEntity1>().GetEntityComponent<CompTest1>() != null)
            SendErrorMessage("Некорректное удаление компонента");

        if (group1.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group2.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group3.entities_count != 0)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group4.entities_count != 1)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group5.entities_count != 3)
            SendErrorMessage("Неправильное количество сущностей в группе");

        #endregion correct num of entityes in group test

        #region Pooling test
        // пока что отключен
        /*
        ent1.GetComponent<EntityBase>().Add<PoolComponent>();
        GameObject ent11 = Instantiate(ent1);
        
        ent11.GetComponent<EntityBase>().GetEntityComponent<CompTest1>().ammo1 = 10;
        ent11.GetComponent<EntityBase>().GetEntityComponent<CompTest1>().health1 = 10;
        PoolManager.DestroyS(ent11);
        ent11 = PoolManager.InstantiateS(ent1);
        gameObjects.Add(ent11);

        if (ent11.GetComponent<EntityBase>().GetEntityComponent<CompTest1>().health1 == 10 || ent11.GetComponent<EntityBase>().GetEntityComponent<CompTest1>().ammo1 == 10)
            SendErrorMessage("Компоненты не возвращаються в исходное состояние");
            */
        #endregion Pooling test

        #region Enable/Disable Test

        if (group6.entities_count != 0)
            SendErrorMessage("Некорректное поведение группы");

        int start_count = group8.entities_count;

        GameObject ent7 = new GameObject();
        gameObjects.Add(ent7);
        ent7.AddComponent<TestEntity7>();

        //Debug.Log("group No" + group8.GetHashCode() % 1631);

        if (group8.entities_count != start_count + 1)
            SendErrorMessage("Некорректное поведение группы при пустом листе компонентов ");
        group8.Delete(); // должна быть здесь

        if (group6.entities_count != 1)
            SendErrorMessage("Некорректное поведение группы");

        ent7.SetActive(false);

        if (group6.entities_count != 0)
            SendErrorMessage("Некорректное поведение при деактивации объекта");

        if (Storage.GetComponent<CompTest7>(ent7.GetComponent<EntityBase>().entity) != null)
            SendErrorMessage("Некорректное поведение при деактивации объекта");

        ent7.SetActive(true);

        if (Storage.GetComponent<CompTest7>(ent7.GetComponent<EntityBase>().entity) == null)
            SendErrorMessage("Некорректное поведение при повторной активации объекта");

        #endregion Enable/Disable Test

        #region CreateGroupInRuntime

        Group group7 = Group.Create(new ComponentsList<CompTest1, CompTest2, CompTest3, CompTest4>());
        groups.Add(group7);
        if (group7.entities_count == 0)
            SendErrorMessage("Ошибки при создании группы в рантайме");

        #endregion CreateGroupInRuntime

        #region DeleteGameObjects

        for (int i = 0; i < gameObjects.Count; i++)
            Destroy(gameObjects[i]);



        #endregion DeleteGameObjects

        #region Others

        int count = 0;
        for (int i = 0; i < groups.Count; i++)
            count += groups[i].entities_count;

        if (count != 0)
        {
            string mes = "";
            for (int i = 0; i < groups.Count; i++)
                mes += groups[i].entities_count;

            SendErrorMessage("при окончании теста в группах находятся сущности " + mes);
        }

        #endregion Others

        #region DeleteGroups

        for (int i = 0; i < groups.Count; i++)
        {
            groups[i].Delete();
        }

        #endregion DeleteGroups

        if (successfull)
        {
            Debug.Log("тест закончен успешно за " + time.ElapsedMilliseconds + " ms");
            PerformanceTest();
        }
        else
            Debug.LogError("тест закончен с ошибками за " + time.ElapsedMilliseconds + " ms");

        void SendErrorMessage(string message)
        {
            successfull = false;
            Debug.LogError(message);
        }
    }

    void PerformanceTest()
    {
        long test_time = 0;
        group1 = Group.Create(new ComponentsList<CompTest1, CompTest2, CompTest3, CompTest4, CompTest5, CompTest6>());
        group2 = Group.Create(new ComponentsList<CompTest1, CompTest2>(), new ComponentsList<CompTest3, CompTest4, CompTest5, CompTest6>());

        groups = new List<Group> { group1, group2 };
        gameObjects = new List<GameObject>();


        for (int i = 0; i < 10; i++)
        {
            gameObjects.Add(new GameObject());
        }

        Debug.LogWarning("начат тест производительности фреймворка");
        //Stopwatch time = Stopwatch.StartNew();

        foreach (GameObject obj in gameObjects)
            obj.AddComponent<TestEntity3>();

        int dif = 100;

        test_time += TestContains(dif);
        test_time += TestIEnumerator(dif);
        test_time += TestAddRemoveComponent(dif);
        test_time += TestAddRemoveGroup(dif);

        for (int i = 0; i < gameObjects.Count; i++)
            Destroy(gameObjects[i]);

        Debug.Log("Тест закончен за " + test_time + " ms");

        long TestContains(int difficult)
        {
            Stopwatch time = Stopwatch.StartNew();

            for (int i = 0; i < difficult; i++)
            {
                foreach (Group group in groups)
                {
                    foreach (int ent in group)
                    {
                        group.Contains(ent);
                    }
                }
            }

            long total_time = time.ElapsedMilliseconds;

            Debug.Log("Contains test complete for " + total_time + " ms");

            return total_time;
        }

        long TestIEnumerator(int difficult)
        {
            Stopwatch time = Stopwatch.StartNew();

            for (int i = 0; i < difficult; i++)
            {
                foreach (Group group in groups)
                {
                    foreach (int ent in group)
                    {
                        ;
                    }
                }
            }

            long total_time = time.ElapsedMilliseconds;

            Debug.Log("IEnumerator test complete for " + total_time + " ms");

            return total_time;
        }

        long TestAddRemoveComponent(int difficult)
        {
            GameObject testObj = new GameObject();

            Stopwatch time = Stopwatch.StartNew();
            testObj.AddComponent<TestEntity4>();
            EntityBase entityBase = testObj.GetComponent<EntityBase>();

            for (int i = 0; i < difficult; i++)
            {
                entityBase.RemoveCmp<CompTest1>();
                entityBase.AddCmp<CompTest1>();
            }

            long total_time = time.ElapsedMilliseconds;
            Debug.Log("AddRemoveComponent test complete for " + total_time + " ms");

            Destroy(testObj);

            return total_time;
        }

        long TestAddRemoveGroup(int difficult)
        {
            GameObject testObj = new GameObject();

            Stopwatch time = Stopwatch.StartNew();
            testObj.AddComponent<TestEntity4>();
            EntityBase entityBase = testObj.GetComponent<EntityBase>();

            for (int i = 0; i < difficult; i++)
            {
                entityBase.RemoveCmp<CompTest3>();
                entityBase.RemoveCmp<CompTest4>();
                entityBase.RemoveCmp<CompTest5>();
                entityBase.RemoveCmp<CompTest6>();

                entityBase.AddCmp<CompTest3>();
                entityBase.AddCmp<CompTest4>();
                entityBase.AddCmp<CompTest5>();
                entityBase.AddCmp<CompTest6>();
            }

            long total_time = time.ElapsedMilliseconds;
            Debug.Log("AddRemoveGroup test complete for " + total_time + " ms");

            Destroy(testObj);

            return total_time;
        }

    }
}

[Serializable]
[HideComponent]
public class CompTest1 : ComponentBase
{
    [Pool]
    public float health1;
    [Pool]
    public int ammo1;
}

[Serializable]
[HideComponent]
public class CompTest2 : ComponentBase
{
    [Pool]
    public float health2;
    [Pool]
    public int ammo2;
}

[Serializable]
[HideComponent]
public class CompTest3 : ComponentBase
{
    [Pool]
    public float health3;
    [Pool]
    public int ammo3;
}

[Serializable]
[HideComponent]
public class CompTest4 : ComponentBase
{
    public float health4;
    public int ammo4;
}

[Serializable]
[HideComponent]
public class CompTest5 : ComponentBase
{
    public float health5;
    public int ammo5;
}

[Serializable]
[HideComponent]
public class CompTest6 : ComponentBase
{
    public float health6;
    public int ammo6;
}

[Serializable]
[HideComponent]
public class CompTest7 : ComponentBase
{
    public float health7;
    public int ammo7;
}


public class TestEntity1 : EntityBase
{
    public override void Setup()
    {
        AddCmp<CompTest1>();
        AddCmp<CompTest1>();
        AddCmp<CompTest1>();
        AddCmp<CompTest1>();
    }
}

public class TestEntity2 : EntityBase
{
    public override void Setup()
    {
        AddCmp<CompTest1>();
        AddCmp<CompTest2>();
        AddCmp<CompTest3>();
        AddCmp<CompTest4>();
    }
}

public class TestEntity3 : EntityBase
{
    public override void Setup()
    {
        AddCmp<CompTest1>();
        AddCmp<CompTest2>();
        AddCmp<CompTest3>();
        AddCmp<CompTest4>();
    }
}

public class TestEntity4 : EntityBase
{
    public override void Setup()
    {
        AddCmp<CompTest1>();
        AddCmp<CompTest2>();
        AddCmp<CompTest3>();
        AddCmp<CompTest4>();
        AddCmp<CompTest5>();
        AddCmp<CompTest6>();
        AddCmp<CompTest7>();
    }
}

public class TestEntity7 : EntityBase
{
    public override void Setup()
    {
        AddCmp<CompTest7>();
    }
}

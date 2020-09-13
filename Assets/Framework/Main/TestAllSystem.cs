using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;
using Stopwatch = System.Diagnostics.Stopwatch;

public class TestAllSystem : MonoBehaviour
{
    Group group1, group2, group3, group4, group5, group6, group8;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            FullTest();
    }

    void FullTest()
    {
        Debug.LogWarning("TestAllSystem упразнен. будет выполнен только тест производительности");
        PerformanceTest();
    }

    void PerformanceTest()
    {
        long test_time = 0;
        group1 = Group.Create(new ComponentsList<CompTest1, CompTest2, CompTest3, CompTest4, CompTest5, CompTest6>());
        group2 = Group.Create(new ComponentsList<CompTest1, CompTest2>(), new ComponentsList<CompTest3, CompTest4, CompTest5, CompTest6>());

        List<Group> groups = new List<Group> { group1, group2 };
        //List<Entity> entities = new List<Entity>();


        //for (int i = 0; i < 10; i++)
        //    entities.Add(EntityCreator.Entity1());

        Debug.LogWarning("начат тест производительности фреймворка");

        int different = 100;

        test_time += TestContains(different);
        test_time += TestIEnumerator(different);
        test_time += TestAddRemoveComponent(different);
        test_time += TestAddRemoveGroup(different);

        //for (int i = 0; i < entities.Count; i++)
        //    Destroy(entities[i]);

        Debug.Log("Тест, сложностью " + different + " закончен за " + test_time + " ms");

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
                        ;// пустая строчка
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
            EntityBase entityBase = testObj.AddComponent<Entity>();

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
            EntityBase entityBase = testObj.AddComponent<Entity>();

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


public class EntityCreator
{
    public static Entity Entity1()
    {
        GameObject obj = new GameObject();
        Entity entity = obj.AddComponent<Entity>();

        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest1>();
        return entity;
    }

    public static Entity Entity2()
    {
        GameObject obj = new GameObject();
        Entity entity = obj.AddComponent<Entity>();

        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest2>();
        entity.AddCmp<CompTest3>();
        entity.AddCmp<CompTest4>();
        return entity;
    }

    public static Entity Entity3()
    {
        GameObject obj = new GameObject();
        Entity entity = obj.AddComponent<Entity>();

        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest2>();
        entity.AddCmp<CompTest3>();
        entity.AddCmp<CompTest4>();
        return entity;
    }

    public static Entity Entity4()
    {
        GameObject obj = new GameObject();
        Entity entity = obj.AddComponent<Entity>();

        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest2>();
        entity.AddCmp<CompTest3>();
        entity.AddCmp<CompTest4>();
        entity.AddCmp<CompTest5>();
        entity.AddCmp<CompTest6>();
        entity.AddCmp<CompTest7>();
        return entity;
    }

    public static Entity Entity7()
    {
        GameObject obj = new GameObject();
        Entity entity = obj.AddComponent<Entity>();

        entity.AddCmp<CompTest7>();
        return entity;
    }

}

using RangerV;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static RangerV.ThreadManager;
using Stopwatch = System.Diagnostics.Stopwatch;
//using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public int ents = 100;
    public int comps = 1;

    private void Start()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(0);
        }
        /*if (Input.GetKeyDown(KeyCode.K))
        {
            Entity entity = gameObject.AddComponent<Entity>();
            entity.AddCmp<HealthComponent>();
            entity.AddCmp<CollisionDamageComponent>();
            
        }*/
    }

    void DDD()
    {
        

        Stopwatch time = Stopwatch.StartNew();

        List<int> list = new List<int>(10);

        for (int i = 0; i < ents; i++)
            list.Add(i);

        for (int j = 0; j < 10000; j++)
            for (int i = 0; i < list.Count; i++)
                list.Contains(i);

        Debug.Log("Contains on " + time.ElapsedMilliseconds);


        EntityBase Ent = null;
        

        int k = 0;
        while (Ent == null)
            Ent = EntityBase.GetEntity(k++);

        int entity = Ent.entity;

        time = Stopwatch.StartNew();

        for (int j = 0; j < 10000; j++)
            for (int i = 0; i < comps; i++)
                Storage.ContainsComponent<CompTest1>(entity);

        Debug.Log("ContainsComponent on " + time.ElapsedMilliseconds);



        time = Stopwatch.StartNew();

        for (int j = 0; j < 10000; j++)
        {
            if (ents * ents / comps < 520)
            {
                for (int i = 0; i < list.Count; i++)
                    list.Contains(i);
            }
            else
            {
                for (int i = 0; i < comps; i++)
                    Storage.ContainsComponent<CompTest1>(entity);
            }
        }
        Debug.Log("super on " + time.ElapsedMilliseconds);
    }

}




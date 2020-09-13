using RangerV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using static RangerV.ThreadManager;
using Stopwatch = System.Diagnostics.Stopwatch;
using UnityEditor.Experimental.TerrainAPI;
using System.Linq;
using UnityEditor.Playables;
//using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public int ents = 100;
    public int comps = 1;
    public List<ISequence> sequences;

    public List<ComponentBase> componentBases;

    private void Start()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            //SceneManager.LoadScene(0);
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

[System.Serializable]
public class Some
{
    public List<ComponentBase> list;
}


[CustomEditor(typeof(Test))]
//[CanEditMultipleObjects]
public class SomeInspector : Editor
{
    Test test;

    private void OnEnable()
    {
        test = (Test)target;

        //test.componentBases = test.GetComponents<ComponentBase>().ToList();
        Debug.Log("OnEnable");
        //test.componentBases.Add();
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        NewOnInspectorGUI();
    }

    void NewOnInspectorGUI()
    {
        //GUILayout.Label("DDD XD");
        for (int i = 0; i < test.componentBases.Count; i++)
        {
            EditorGUI.BeginChangeCheck();

            ComponentBase componentBase = (ComponentBase)EditorGUILayout.ObjectField(test.componentBases[i], typeof(ComponentBase), true);

            if (componentBase == null)
            {
                SetDirty();
                test.componentBases[i] = null;
            }
            else if (componentBase is ISequence)
            {
                SetDirty();
                test.componentBases[i] = componentBase;
            }
            else
            {
                Debug.LogWarning("not ISequence");
            }

            if (EditorGUI.EndChangeCheck())
            {
                //EditorUtility.SetDirty(test);
                //Undo.RecordObject(test, "Changed Area Of Effect");
            }
        }

        void SetDirty()
        {
            EditorUtility.SetDirty(test);
            Undo.RecordObject(test, "Changed Area Of Effect");
        }
    }
}


public interface ISequence
{
    //void OnAdd();
}




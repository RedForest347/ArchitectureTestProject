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
using UnityEngine.Experimental.UIElements;
using System.ComponentModel;
using UnityEngine.EventSystems;
using System.Reflection;
//using System.Diagnostics;
//using UnityEngine.SceneManagement;


//https://yandex.ru/video/preview?text=авторасширяемая%20textArea%20в%20юнити&path=wizard&parent-reqid=1600076661810613-1154153130362982894900278-production-app-host-man-web-yp-265&wiz_type=vital&filmId=18311393161572850192


public class Test : MonoBehaviour
{
    public int ents = 100;
    public int comps = 1;


    //public Func<int> func;
    //public Action action;
    //public event Action actionEvent;
    //public Event animationEvent;
    //public EventHandler eventHandler;
    public EventProvider provider;
    public EventBase bas;
    public EventDescriptor descriptor;
    public EventTrigger trigger;
    //public EventBase descriptor;
    //public EventDescriptor descriptor;


    //public List<ComponentBase> componentBases;

    private void Start()
    {
        MethodInfo[] methodInfo = GetType().GetMethods();
        MethodInfo DInf = GetType().GetMethod("DDD");

        Stopwatch time = Stopwatch.StartNew();

        for (int i = 0; i < 10000; i++)
        {
            DInf.Invoke(this, null);
        }
        Debug.Log("on " + time.ElapsedMilliseconds + " ms");

        time.Restart();
        for (int i = 0; i < 1000000; i++)
        {
            DDD();
        }
        Debug.Log("on " + time.ElapsedMilliseconds + " ms");

        //Debug.Log("всего " + methodInfo.Length + " методов");

        /*for (int i = 0; i < methodInfo.Length; i++)
        {
            if (methodInfo[i].Name == "DDD")
                methodInfo[i].Invoke(this, null);
        }*/
    }

    public void DDD()
    {
        //Debug.Log("DDD complete");
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
}



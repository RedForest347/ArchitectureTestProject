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


    //https://yandex.ru/video/preview?text=авторасширяемая%20textArea%20в%20юнити&path=wizard&parent-reqid=1600076661810613-1154153130362982894900278-production-app-host-man-web-yp-265&wiz_type=vital&filmId=18311393161572850192


public class Test : MonoBehaviour
{
    public int ents = 100;
    public int comps = 1;
    

    //public List<ComponentBase> componentBases;

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
}



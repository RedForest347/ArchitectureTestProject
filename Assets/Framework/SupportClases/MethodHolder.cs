using RangerV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class MethodHolder
{
    //Inspector data
    public bool show_private_methods;

    //Method Data
    public string type_name;
    public string method_name;
    public string assembly_name;
    public Component component;


    public void StartMethod()
    {
        Assembly.Load(assembly_name).GetType(type_name).GetMethod(method_name).Invoke(component, null);
    }

}

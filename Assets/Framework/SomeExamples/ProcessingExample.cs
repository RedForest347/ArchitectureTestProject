using UnityEngine;
using System.Collections.Generic;
using RangerV;
using Stopwatch = System.Diagnostics.Stopwatch;
using System;

public class ProcessingExample1 : ProcessingBase, ICustomAwake, ICustomUpdate
{
    //public Group group = Group.CreateGroup(Group.CreateComponentsList<HealthComponent>());
    public Group group = Group.Create(new ComponentsList<HealthComponent>());

    public void OnAwake()
    {

    }


    public void CustomUpdate()
    {

    }
}


public class ProcessingExample2 : ProcessingBase, ICustomAwake, ICustomUpdate
{
    public void OnAwake()
    {

    }

    public void CustomUpdate()
    {
       
    }
}


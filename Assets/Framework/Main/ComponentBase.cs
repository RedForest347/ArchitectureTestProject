using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace RangerV
{
    /// <summary>
    /// для компонентов стоит применять интерфейс ICustomAwake для инициализации необходимых данных.
    /// добавление компонента в ManagerUpdate производится из EntityBase при создании и добавлении
    /// компонента к сущности. также, OnAwake (метод интерфейса ICustomAwake) вызывается при повторном 
    /// Enable компонента. для реализации инициализации, которая будет происходить единожды, можно
    /// использовать конструктор компонента
    /// 
    /// 
    /// была идея реализовать использование интерфейса ICustomStart, который будет вызываться каждый раз при 
    /// Enable компонента. при этом, ICustomAwake будет вызываться единожды при инициализации (добавлении) компонента на сущность, 
    /// но пока смысла не вижу. по необходимости есть возможность реализовать данный вариант
    /// 
    /// следует именовать [имя_компонента]Cmp
    /// </summary>
    [System.Serializable]
    public class ComponentBase: MonoBehaviour, IComponent
    {
        int entity;
        EntityBase entityBase;// добавить
        //public string name;
    }
}

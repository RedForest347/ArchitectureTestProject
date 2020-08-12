using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace RangerV
{
    /// <summary>
    /// для компонентов стоит применять интерфейс IAwake для инициализации необходимых данных.
    /// добавление компонента в ManagerUpdate производится в EntityBase при создании и добавлении
    /// компонента к сущности.
    /// </summary>
    [System.Serializable]
    public class ComponentBase: MonoBehaviour, IComponent
    {
        //public string name;
    }
}

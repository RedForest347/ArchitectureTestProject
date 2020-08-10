using UnityEngine;
using RangerV;

[System.Serializable]
public class ArmorComponentAdd
{
    [SerializeField]
    public float armor_regen;
}


[Component("Main/Armor")]
public class ArmorComponent : ComponentBase
{
    [Pool, SerializeField]
    public float armor;
    [Pool, SerializeField]
    public ArmorComponentAdd armor_add;

    public ArmorComponent()
    {

    }
}

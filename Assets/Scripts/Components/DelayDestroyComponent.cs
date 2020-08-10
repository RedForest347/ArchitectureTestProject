using UnityEngine;
using RangerV;

[System.Serializable]
public class DelayDestroyComponent : ComponentBase
{
    [Pool]
    public float time;

    public DelayDestroyComponent()
    {

    }
}

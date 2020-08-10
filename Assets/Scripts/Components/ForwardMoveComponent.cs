using UnityEngine;
using RangerV;


[System.Serializable]
public class ForwardMoveComponent : ComponentBase
{
    [Pool]
    public float speed;
    [Pool]
    public Vector3 local_direction;
    [Pool]
    public Vector3 world_direction;
    //-----------------------------------------//
    [HideInInspector] public Vector3 velocity;


    public ForwardMoveComponent()
    {

    }
}

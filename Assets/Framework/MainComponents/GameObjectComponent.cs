using UnityEngine;
using RangerV;

[Component("Engine/GameObject component", "d_Prefab Icon")]
public class GameObjectComponent : ComponentBase
{
    public GameObject GameObject;
    //public Vector3 previous_positon = new Vector3(0, 0, 0);
    //public Vector3 current_position = new Vector3(0, 0, 0);
    //public Vector3 velocity = new Vector3(0, 0, 0);

    public GameObjectComponent()
    {
        GameObject = null;
    }

    public GameObjectComponent(GameObject GO)
    {
        GameObject = GO;
    }
}

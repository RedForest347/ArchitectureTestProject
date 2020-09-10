using UnityEngine;
using RangerV;

[Component("Engine/Physics component", "Rigidbody Icon")]
public class PhysicsComponent : ComponentBase
{
    public Collider Collider;
    public Rigidbody Rigidbody;

    public PhysicsComponent()
    {
        Collider = null;
        Rigidbody = null;
    }

    public PhysicsComponent(GameObject GO)
    {
        Collider = GO.GetComponent<Collider>();
        Rigidbody = GO.GetComponent<Rigidbody>();
    }
}

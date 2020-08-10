using UnityEngine;
using RangerV;

public class ForwardMoveProcessing : ProcessingBase, ICustomAwake, ICustomUpdate, ICustomStart
{
    Group moveGroup = Group.Create(new ComponentsList<ForwardMoveComponent>());

    public void OnAwake()   //при добавлении в GlobalSystemStorage
    {

    }

    public void OnStart()   //при активации в сущьностей в стартере
    {
        //foreach (int entity in moveGroup)
        //{
        //    PhysicsComponent physicsComponent = Storage<PhysicsComponent>.StorageForType.components[entity];
        //    Debug.Log(physicsComponent);
        //}
    }

    public void CustomUpdate()
    {
        GameObjectComponent gameObjectComponent;
        ForwardMoveComponent forwardMoveComponent;

        foreach (int entity in moveGroup)
        {
            gameObjectComponent = Storage.GetComponent<GameObjectComponent>(entity);
            forwardMoveComponent = Storage.GetComponent<ForwardMoveComponent>(entity);

            Vector3 local_direction = new Vector3//(0.1f, 0, 0);
            (
                forwardMoveComponent.local_direction.x * (forwardMoveComponent.speed / 10),
                forwardMoveComponent.local_direction.y * (forwardMoveComponent.speed / 10),
                forwardMoveComponent.local_direction.z * (forwardMoveComponent.speed / 10)
            );
            gameObjectComponent.GameObject.transform.Translate(local_direction * Time.deltaTime);

            Vector3 world_direction = new Vector3//(0.1f, 0, 0);  //?
            (
                forwardMoveComponent.world_direction.x * (forwardMoveComponent.speed / 10),
                forwardMoveComponent.world_direction.y * (forwardMoveComponent.speed / 10),
                forwardMoveComponent.world_direction.z * (forwardMoveComponent.speed / 10)
            );
            gameObjectComponent.GameObject.transform.Translate((world_direction + forwardMoveComponent.velocity) * Time.deltaTime, Space.World);
        }
    }
}

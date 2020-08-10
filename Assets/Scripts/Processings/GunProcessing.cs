using UnityEngine;
using RangerV;

public class GunProcessing : ProcessingBase, ICustomStart, ICustomUpdate
{
    Group gun_group = Group.Create(new ComponentsList<PlasmaGunComponent, GameObjectComponent, TimerComponent>());

    PlasmaGunComponent plasmaGunComponent;
    GameObject gameObject;
    GameObjectComponent gameObjectComp;
    TimerComponent timer1;


    public void OnStart()   //при добавлении в GlobalSystemStorage
    {
        foreach (int entity in gun_group)
        {
            gameObjectComp = Storage.GetComponent<GameObjectComponent>(entity);
            timer1 = Storage.GetComponent<TimerComponent>(entity);
            gameObject = gameObjectComp.GameObject;

            gameObjectComp.previous_positon = gameObject.transform.position;
            gameObjectComp.current_position = gameObject.transform.position;
            timer1.StratTimer(Storage.GetComponent<PlasmaGunComponent>(entity).cooldown);
        }
    }

    public void CustomUpdate()      //Update
    {
        foreach (int entity in gun_group)
        {
            gameObjectComp = Storage.GetComponent<GameObjectComponent>(entity);
            gameObject = gameObjectComp.GameObject;
            plasmaGunComponent = Storage.GetComponent<PlasmaGunComponent>(entity);
            timer1 = Storage.GetComponent<TimerComponent>(entity);

            gameObjectComp.current_position = gameObject.transform.position;
            gameObjectComp.velocity = (gameObjectComp.current_position - gameObjectComp.previous_positon) / Time.deltaTime;

            if (Input.GetKey(KeyCode.Space) && timer1.CheckTimer())
            {
                Vector3 position = gameObject.transform.position;
                Vector3 rotation = gameObject.transform.rotation.eulerAngles;
                //Quaternion rotation = Quaternion.Euler(0, 0, 0);

                int bullet_entity = GameObject.Instantiate(plasmaGunComponent.AmmoPrefab, position, Quaternion.Euler(rotation)).GetComponent<EntityBase>().entity;

                //Storage.GetComponent<ForwardMoveComponent>(bullet_entity).velocity = gameObjectComp.velocity;
                Storage.GetComponent<PhysicsComponent>(bullet_entity).Rigidbody.AddRelativeForce(new Vector3(0, 5, 0), ForceMode.VelocityChange);

                timer1.StratTimer(plasmaGunComponent.cooldown);
            }

            gameObjectComp.previous_positon = gameObjectComp.current_position;
        }
    }


}

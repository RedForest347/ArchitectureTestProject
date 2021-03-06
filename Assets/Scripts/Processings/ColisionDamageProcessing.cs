﻿using UnityEngine;
using RangerV;
using System;

public class ColisionDamageProcessing : ProcessingBase, ICustomUpdate, ICustomStart
{
    Group collision_group = Group.Create(new ComponentsList<CollisionDamageComponent, PhysicsComponent, CollisionComponent>());
    Group target_group = Group.Create(new ComponentsList<HealthComponent, GameObjectComponent>());


    public void OnStart()
    {
        collision_group.OnAddEntity += DDD;
    }

    void DDD(int entity)
    {
        //Debug.Log("DDD");
        Storage.GetComponent<CollisionComponent>(entity).AddOnTriggerAction(GiveDamage, CollisionActionType.Enter, GameObject.Find("EaglePlayer").GetComponent<Collider>());

    }


    public void CustomUpdate()
    {

    }


    public void GiveDamage(Collider collider, int entity)
    {
        int target_entity = collider.gameObject.GetComponent<Entity>().entity;

        if (target_group.Contains(target_entity))
        {
            HealthComponent healthComponent = Storage.GetComponent<HealthComponent>(target_entity);
            healthComponent.health -= 5;
            if (healthComponent.health <= 0)
                GameObject.Destroy(Storage.GetComponent<GameObjectComponent>(target_entity).GameObject);

            if (Storage.GetComponent<CollisionDamageComponent>(entity).destroy_on_collision)
            {
                //Debug.Log("Storage.GetComponent<GameObjectComponent>(entity) = " + Storage.GetComponent<GameObjectComponent>(entity));
                GameObject.Destroy(Storage.GetComponent<GameObjectComponent>(entity).GameObject);
            }
        }
    }

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class ShipPhysicsProcessing : ProcessingBase, ICustomAwake, ICustomUpdate
{
    Group shipsGroup = Group.Create(new ComponentsList<ShipSettingsComponent>());

    Vector3 EulerAngleVelocity;
    ShipSettingsComponent shipSettingsComponent;
    ShipControlSignal ShipControlSignal;
    PhysicsComponent physicsComponent;

    public void OnAwake()
    {

    }

    public void CustomUpdate()
    {
        foreach (int entity in shipsGroup)
        {
            shipSettingsComponent = Storage.GetComponent<ShipSettingsComponent>(entity);
            ShipControlSignal = shipSettingsComponent.ShipControlSignal;
            EulerAngleVelocity = shipSettingsComponent.EulerAngleVelocity;
            physicsComponent = Storage.GetComponent<PhysicsComponent>(entity);


            //---------------------------------------------------//
            EffectsWork();

            //---------------------------------------------------//
            Vector3 direction = new Vector3(0, 0, 0);
            float main_force = shipSettingsComponent.MainEngineForce;
            float boost_force = shipSettingsComponent.BoostForce;
            float maneuver_force = shipSettingsComponent.ManeuverEngineForce;


            if (ShipControlSignal.Up)
            {
                direction += new Vector3(0, 1, 0) * main_force;
                if (ShipControlSignal.Boost)
                    direction += new Vector3(0, 1, 0) * boost_force;
            }
            else if (ShipControlSignal.Down)
                direction += new Vector3(0, -1, 0) * maneuver_force;

            if (ShipControlSignal.Left)
                direction += new Vector3(-1, 0, 0) * maneuver_force;
            else if (ShipControlSignal.Rigt)
                direction += new Vector3(1, 0, 0) * maneuver_force;


            physicsComponent.Rigidbody.AddRelativeForce(direction);

            float x = Mathf.Clamp(physicsComponent.Rigidbody.velocity.x, -10, 10);
            float y = Mathf.Clamp(physicsComponent.Rigidbody.velocity.y, -10, 10);
            physicsComponent.Rigidbody.velocity = new Vector3(x, y, 0);


            //---------------------------------------------------//
            float roll_force = shipSettingsComponent.RollForce / 1000;

            if (ShipControlSignal.RollLeft)
                EulerAngleVelocity += new Vector3(0, 0, 60) * roll_force;
            else if (ShipControlSignal.RollRigt)
                EulerAngleVelocity += new Vector3(0, 0, -60) * roll_force;

            float z = Mathf.Clamp(EulerAngleVelocity.z, -120, 120);
            EulerAngleVelocity.z = z;

            Quaternion deltaRotation = Quaternion.Euler(EulerAngleVelocity * Time.deltaTime);
            physicsComponent.Rigidbody.MoveRotation(physicsComponent.Rigidbody.rotation * deltaRotation);
            shipSettingsComponent.EulerAngleVelocity = EulerAngleVelocity;

        }
    }


    void EffectsWork()
    {
        GameObject[] effects = shipSettingsComponent.MainEngineEffect;
        #region OLD
        //if (ShipControlSignal.Up)
        //    EffectAnimation(effects, "State", 1);
        //if (ShipControlSignal.Up)
        //    EffectAnimation(effects, "State", 0);
        //if (ShipControlSignal.Up && ShipControlSignal.Boost)
        //    EffectAnimation(effects, "State", 2);
        //if (ShipControlSignal.Up && ShipControlSignal.Boost)
        //    EffectAnimation(effects, "State", 1);


        //effects = shipSettingsComponent.ForwardEngineEffect;
        //if (ShipControlSignal.Down)
        //    EffectAnimation(effects, "State", 1);
        //if (ShipControlSignal.Down)
        //    EffectAnimation(effects, "State", 0);


        //effects = shipSettingsComponent.RightEngineEffect;
        //if (ShipControlSignal.Left)
        //    EffectAnimation(effects, "State", 1);
        //if (ShipControlSignal.Left)
        //    EffectAnimation(effects, "State", 0);


        //effects = shipSettingsComponent.LeftEngineEffect;
        //if (ShipControlSignal.Rigt)
        //    EffectAnimation(effects, "State", 1);
        //if (ShipControlSignal.Rigt)
        //    EffectAnimation(effects, "State", 0);


        //effects = new GameObject[]
        //{
        //    shipSettingsComponent.RightEngineEffect[1],
        //    shipSettingsComponent.LeftEngineEffect[0]
        //};
        //if (ShipControlSignal.RollLeft)
        //    EffectAnimation(effects, "State", 1);
        //if (ShipControlSignal.RollLeft)
        //    EffectAnimation(effects, "State", 0);


        //effects = new GameObject[]
        //{
        //    shipSettingsComponent.RightEngineEffect[0],
        //    shipSettingsComponent.LeftEngineEffect[1]
        //};
        //if (ShipControlSignal.RollRigt)
        //    EffectAnimation(effects, "State", 1);
        //if (ShipControlSignal.RollRigt)
        //    EffectAnimation(effects, "State", 0);
        #endregion

        if (ShipControlSignal.Up)
        {
            EffectAnimation(effects, "State", 1);
            if (ShipControlSignal.Boost)
                EffectAnimation(effects, "State", 2);
        }
        else
            EffectAnimation(effects, "State", 0);


        effects = shipSettingsComponent.ForwardEngineEffect;
        if (ShipControlSignal.Down)
            EffectAnimation(effects, "State", 1);
        else
            EffectAnimation(effects, "State", 0);


        effects = shipSettingsComponent.RightEngineEffect;
        if (ShipControlSignal.Left)
            EffectAnimation(effects, "State", 1);
        else
            EffectAnimation(effects, "State", 0);


        effects = shipSettingsComponent.LeftEngineEffect;
        if (ShipControlSignal.Rigt)
            EffectAnimation(effects, "State", 1);
        else
            EffectAnimation(effects, "State", 0);


        effects = new GameObject[]
        {
            shipSettingsComponent.RightEngineEffect[1],
            shipSettingsComponent.LeftEngineEffect[0]
        };
        if (ShipControlSignal.RollLeft)
            EffectAnimation(effects, "State", 1);
        else
            EffectAnimation(effects, "State", 0);


        effects = new GameObject[]
        {
            shipSettingsComponent.RightEngineEffect[0],
            shipSettingsComponent.LeftEngineEffect[1]
        };
        if (ShipControlSignal.RollRigt)
            EffectAnimation(effects, "State", 1);
        else
            EffectAnimation(effects, "State", 0);
    }


    void EffectAnimation(GameObject[] effects, string state_name, int state)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            //effects[i].SetActive(true);
            effects[i].GetComponent<Animator>().SetInteger(state_name, state);
        }
    }

}

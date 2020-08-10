using UnityEngine;
using RangerV;

public class PlayerControlProcessing : ProcessingBase, ICustomAwake, ICustomUpdate
{
    Group playerGroup = Group.Create(new ComponentsList<PlayerShipControlComponent, ShipSettingsComponent>());

    public void OnAwake()   //при добавлении в GlobalSystemStorage
    {

    }

    public void CustomUpdate()      //Update
    {
        foreach (int entity in playerGroup)
        {
            PlayerShipControlComponent playerShipControlComponent = Storage.GetComponent<PlayerShipControlComponent>(entity);
            ShipSettingsComponent shipSettingsComponent = Storage.GetComponent<ShipSettingsComponent>(entity);

            //-----------------------------------------------//
            ShipControlSignal shipControlSignal = new ShipControlSignal
            {
                Up = Input.GetKey(playerShipControlComponent.ControlSettings.Up),
                Boost = Input.GetKey(playerShipControlComponent.ControlSettings.Up) && Input.GetKey(playerShipControlComponent.ControlSettings.Boost),
                Down = Input.GetKey(playerShipControlComponent.ControlSettings.Down),
                Left = Input.GetKey(playerShipControlComponent.ControlSettings.Left),
                Rigt = Input.GetKey(playerShipControlComponent.ControlSettings.Rigt),
                RollLeft = Input.GetKey(playerShipControlComponent.ControlSettings.RollLeft),
                RollRigt = Input.GetKey(playerShipControlComponent.ControlSettings.RollRigt)
            };

            //SignalManager<ShipControlSignal>.SignalManagerInstance.SendSignal(shipControlSignal);
            shipSettingsComponent.ShipControlSignal = shipControlSignal;
        }
    }
}

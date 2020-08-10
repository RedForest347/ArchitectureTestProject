using UnityEngine;
using RangerV;
using UnityEditor;

[System.Serializable]
public class PlayerShipControlComponent : ComponentBase
{
    //public KeyCode Up;
    //public KeyCode Down;
    //public KeyCode Left;
    //public KeyCode Rigt;
    //public KeyCode Boost;
    //public KeyCode RollLeft;
    //public KeyCode RollRigt;

    public ControlSettings ControlSettings;

    //[Space(10)]
    //public KeyCode Shoot;

    public PlayerShipControlComponent()
    {

    }
}


public struct ShipControlSignal
{
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Rigt;
    public bool Boost;
    public bool RollLeft;
    public bool RollRigt;
}

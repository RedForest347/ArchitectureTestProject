using UnityEngine;
using RangerV;


[System.Serializable]
public class ShipSettingsComponent : ComponentBase
{
    [Header(" Ship settings:")]
    public float MainEngineForce;
    public float ManeuverEngineForce;

    [Space(15)]
    public GameObject[] MainEngineEffect;
    public GameObject[] LeftEngineEffect;
    public GameObject[] RightEngineEffect;
    public GameObject[] ForwardEngineEffect;

    [Space(15)]
    public float BoostForce;

    [Space(15)]
    public float RollForce;

    //-----------------------------------------//
    [HideInInspector] public ShipControlSignal ShipControlSignal;
    [HideInInspector] public Vector3 EulerAngleVelocity;


    public ShipSettingsComponent()
    {

    }
}

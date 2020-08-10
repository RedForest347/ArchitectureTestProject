using UnityEngine;
using RangerV;

[System.Serializable]
public class PlasmaGunComponent : ComponentBase
{
    [Pool]
    public int ammo;
    [Pool]
    public float cooldown;

    public GameObject AmmoPrefab;

    public PlasmaGunComponent()
    {

    }
}

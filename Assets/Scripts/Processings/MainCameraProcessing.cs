using UnityEngine;
using RangerV;

public class MainCameraProcessing : ProcessingBase, ICustomAwake, ICustomUpdate, ICustomFixedUpdate
{
    Group playersGroup = Group.Create(new ComponentsList<PlayerShipControlComponent>());

    Camera MainCamera;

    public MainCameraProcessing()
    {
        MainCamera = Camera.main;

    }

    public void OnAwake()   //при добавлении в GlobalSystemStorage
    {
        
    }

    public void CustomUpdate()
    {
        foreach (int entity in playersGroup)
        {
            GameObject obj = Storage.GetComponent<GameObjectComponent>(entity).GameObject;
            MainCamera.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, MainCamera.transform.position.z);
        }
    }

    public void CustomFixedUpdate()
    {
        
    }

}

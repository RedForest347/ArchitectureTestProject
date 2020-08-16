using UnityEngine;
using RangerV;

public class MainCameraProcessing : ProcessingBase, ICustomAwake, ICustomUpdate, ICustomFixedUpdate
{
    Group playersGroup = Group.Create(new ComponentsList<PlayerShipControlComponent>());

    Camera MainCamera;

    public void OnAwake()   //при добавлении в GlobalSystemStorage
    {
        MainCamera = Camera.main;
    }

    public void CustomUpdate()      //Update
    {
        foreach (int entity in playersGroup)
        {
            GameObject gameObject = Storage.GetComponent<GameObjectComponent>(entity).GameObject;
            //PlayerShipControlComponent playerShipControlComponent = Storage<PlayerShipControlComponent>.StorageForType.components[entity];
            //Debug.Log("Processings.Count = " + GlobalSystemStorage.Instance.Processings.Count);
            //Debug.Log("MainCamera = " + MainCamera);
            MainCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, MainCamera.transform.position.z);
        }
    }

    public void CustomFixedUpdate()      //FixedUpdate
    {
        
    }

}

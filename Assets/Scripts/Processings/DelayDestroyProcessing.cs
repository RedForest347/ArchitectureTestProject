using UnityEngine;
using RangerV;

public class DelayDestroyProcessing : ProcessingBase, ICustomUpdate
{
    Group group = Group.Create(new ComponentsList<DelayDestroyComponent>());

    public void CustomUpdate()      //Update
    {
        foreach (int entity in group)
        {
            GameObject gameObject = Storage.GetComponent<GameObjectComponent>(entity).GameObject;
            DelayDestroyComponent delayDestroyComponent = Storage.GetComponent<DelayDestroyComponent>(entity);

            GameObject.Destroy(gameObject, delayDestroyComponent.time);
        }
    }
}

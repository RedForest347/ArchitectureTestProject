using UnityEngine;
using RangerV;


[System.Serializable]
public class TimerComponent : ComponentBase, ICustomUpdate, ICustomAwake
{
    public float count;
    public float time;

    public TimerComponent()
    {

    }


    public void OnAwake()
    {
        //Debug.Log("OnAwake");
        ManagerUpdate.InstanceManagerUpdate.AddTo(this);
    }

    public void StratTimer(float time)
    {
        this.time = time;
        RestratTimer();
    }

    public void RestratTimer()
    {
        count = 0;
    }

    public bool CheckTimer()
    {
        if (count <= time)
        {
            return false;
        }
        else
            return true;

    }

    public void CustomUpdate()
    {
        //Debug.Log("CustomUpdate");
        count += Time.deltaTime;
    }
}
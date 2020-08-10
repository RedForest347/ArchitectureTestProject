using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOHolder : MonoBehaviour
{
    public GameObject Children;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            KernelManager.kernelManager.gameObject = new GameObject();
            Children.transform.parent = KernelManager.kernelManager.gameObject.transform;
        }

        //for()

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KernelManager
{
    public GameObject gameObject;

    static KernelManager _kernelManager;


    public static KernelManager kernelManager
    {
        get
        {
            if (_kernelManager == null)
                _kernelManager = new KernelManager();
            return _kernelManager;
        }
    }
}

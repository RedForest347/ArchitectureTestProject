using RangerV;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeSeqenceCmp : ComponentBase, ISequence
{
    public event Action StartNextSiquenceElem;
    //public event Action<int> DDD

    public void StartSequenceElem()
    {
        CorutineManager.StartCorutine(DDD());
    }

    IEnumerator DDD()
    {
        Debug.Log("Начата задача");
        for (int i = 0; i < 200; i++)
        {
            if (i % 50 == 0)
                Debug.Log(i);
            yield return null;
        }
        Debug.Log("Закончена задача");
        yield return null;

        StartNextSiquenceElem?.Invoke();

    }

    public void DoSome()
    {
        Debug.Log("func work!!!");
    }

}

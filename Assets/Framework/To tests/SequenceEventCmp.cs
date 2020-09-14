using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;

/// <summary>
/// последовательность событий
/// </summary>
public class SequenceEventCmp : ComponentBase, ISequence, ICustomAwake
{
    public bool start_is_awake;
    public List<SequenceElemData> sequenceElemData;

    int current_elem;



    public event Action StartNextSiquenceElem;

    public void OnAwake()
    {
        StartSequence();
    }

    public void StartSequence()
    {
        current_elem = 0;

        for (int i = 0; i < sequenceElemData.Count; i++)
        {
            sequenceElemData[i].sequence.StartNextSiquenceElem += StartNext;
        }
        sequenceElemData[current_elem++].sequence.StartSequenceElem();
    }

    public void StartSequenceElem()
    {
        
    }

    void StartNext()
    {
        if (sequenceElemData.Count >= current_elem)
        {
            sequenceElemData[current_elem++].sequence.StartSequenceElem();
            Debug.Log("Стартовала новая задача");
        }
        else
        {
            Debug.Log("Все задачи выполнены");
        }
        
    }


    IEnumerator DDD()
    {
        Debug.Log("Начата задача");
        for (int i = 0; i < 200; i++)
        {
            if (i % 5 == 0)
                Debug.Log(i);
            yield return null;
        }
        Debug.Log("Закончена задача");
        yield return null;
        StartNext();

    }



}

public class SomeSeqenceCmp : ComponentBase, ISequence
{
    public event Action StartNextSiquenceElem;

    public void StartSequenceElem()
    {
        Debug.Log("Стартовала первая задача");
        CorutineManager.StartCorutine(DDD());
    }

    void StartNext()
    {
        StartNextSiquenceElem?.Invoke();
        Debug.Log("Стартовала новая задача");
    }

    IEnumerator DDD()
    {
        Debug.Log("Начата задача");
        for (int i = 0; i < 200; i++)
        {
            if (i % 5 == 0)
                Debug.Log(i);
            yield return null;
        }
        Debug.Log("Закончена задача");
        yield return null;
        StartNext();

    }
}



public interface ISequence
{
    void StartSequenceElem();
    event Action StartNextSiquenceElem;
}

[System.Serializable]
public class SequenceElemData
{
    public ISequence sequence { get => componentBase as ISequence; }
    public ComponentBase componentBase;

    public string note;


    //Inspector data
    public bool show_elem;
    public Vector2 scroll;

    public SequenceElemData()
    {
        this.componentBase = null;
        this.note = "";
        this.show_elem = false;
    }
}

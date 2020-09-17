using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;
using System.Linq;
using System.Reflection;

/// <summary>
/// инструкция по использованию:
/// на сущность добавляется компонент SequenceEventCmp.
/// какой либо компонент наследуется от ISequence. 
/// (реализуются все необходимые методы интерфейса ISequence)
/// затем через инспектор кидается на SequenceEventCmp
/// метод StartSequenceElem выполняется когда подошла очередь выполнять код очередного элемента листа sequenceElemData
/// затем, по окончанию всех необходимых действий, после вызова StartNextSiquenceElem начинается выполнение кода следующего элемента
/// по окончании выполнения кода всех элементов, вызывается евент OnCompleteSequence
/// </summary>
public class SequenceEventCmp : ComponentBase, ICustomAwake
{
    public bool start_is_awake;
    public List<SequenceElemData> sequenceElemData;

    /// int - сущность (ее номер), на которой выполнелась последовательность
    public event Action<int> OnCompleteSequence;

    bool was_started;
    int current_elem;

    public void OnAwake()
    {
        //StartSequence();
    }

    public void StartSequence()
    {
        if (was_started)
        {
            Debug.LogWarning("Данная последовательность событий уже была запущена. она не может быть повторно запущена");
            return;
        }

        current_elem = 0;

        List<ISequence> uniqueElement = sequenceElemData.GroupBy(x => x.sequence).Select(g => g.Key).ToList();  // убираются повторяющиеся элементы
                                                                                                            //(из всех повторяющихся элементов остается один)

        for (int i = 0; i < uniqueElement.Count; i++)
        {
            uniqueElement[i].StartNextSiquenceElem += StartNext;
        }

        sequenceElemData[current_elem++].sequence.StartSequenceElem();
    }

    void StartNext()
    {
        if (current_elem < sequenceElemData.Count)
        {
            Debug.Log("SequenceEventCmp запускает новую задачу");
            sequenceElemData[current_elem++].sequence.StartSequenceElem();
            
        }
        else
        {
            OnCompleteSequence?.Invoke(entity);
            Debug.Log("Все задачи выполнены");
        }
        
    }


    public bool CheckSequenceElemsForCorrectValues()
    {
        for (int i = 0; i < sequenceElemData.Count; i++)
            if (!(sequenceElemData[i].componentBase is ISequence))
                return false;

        return true;
    }

}

[System.Serializable]
public class SequenceElemData
{
    public ISequence sequence { get => componentBase as ISequence; }
    public ComponentBase componentBase;

    public string note;
    public MethodInfo methodInfo;

    //Inspector data
    public bool show_elem;
    public bool show_private_methods;
    


    public Vector2 scroll;
    

    public SequenceElemData()
    {
        this.componentBase = null;
        this.note = "";
        this.show_elem = false;
    }
}

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
}

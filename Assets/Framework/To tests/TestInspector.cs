using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Test))]
public class TestInspector : Editor
{
    Test obj;

    private void OnEnable()
    {
        obj = (Test)target;
    }
}

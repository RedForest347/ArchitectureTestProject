using RangerV;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(SequenceEventCmp))]
public class SequenceInspector : Editor
{
    SequenceEventCmp sequence;

    private void OnEnable()
    {
        sequence = (SequenceEventCmp)target;
    }

    public override void OnInspectorGUI()
    {
        NewOnInspectorGUI();
    }

    void NewOnInspectorGUI()
    {

        if (sequence.sequenceElemData == null)
            sequence.sequenceElemData = new List<SequenceElemData>();


        for (int i = 0; i < sequence.sequenceElemData.Count; i++)
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            ComponentBase componentBase = (ComponentBase)EditorGUILayout.ObjectField(sequence.sequenceElemData[i].componentBase, typeof(ComponentBase), true);

            if (GUILayout.Button("Remove", GUILayout.Width(140), GUILayout.Height(15)))
            {
                sequence.sequenceElemData.RemoveAt(i--);
                continue;
            }

            EditorGUILayout.EndHorizontal();



            sequence.sequenceElemData[i].show_elem = EditorGUILayout.Foldout(sequence.sequenceElemData[i].show_elem, "Описание", true);

            if (sequence.sequenceElemData[i].show_elem)
            {
                GUISkin skin = GUI.skin;
                skin.textArea.wordWrap = true;

                EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
                

                sequence.sequenceElemData[i].scroll = EditorGUILayout.BeginScrollView(sequence.sequenceElemData[i].scroll);
                sequence.sequenceElemData[i].note = EditorGUILayout.TextArea(sequence.sequenceElemData[i].note, new GUIStyle(skin.textArea));
                EditorGUILayout.EndScrollView();

                
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();


            if (componentBase == null || componentBase is ISequence)
            {
                sequence.sequenceElemData[i].componentBase = componentBase;
                SetDirty();
            }
            else
            {
                Debug.LogWarning("not ISequence");
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(sequence);
                Undo.RecordObject(sequence, "Changed Area Of Effect");
            }
        }

        if (GUILayout.Button("Add", GUILayout.Width(150), GUILayout.Height(30)))
        {
            sequence.sequenceElemData.Add(new SequenceElemData());
            SetDirty();
        }

        if (!sequence.CheckSequenceElemsForCorrectValues())
        {
            EditorGUILayout.HelpBox("Не все элементы в списке корректны (не существуют или не являются ISequence)", MessageType.Error);
        }

        void SetDirty()
        {
            EditorUtility.SetDirty(sequence);
            Undo.RecordObject(sequence, "Changed Sequence");
        }
    }
}
using RangerV;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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


        #region Show Sequence Elements

        for (int i = 0; i < sequence.sequenceElemData.Count; i++)
        {
            EditorGUI.BeginChangeCheck();






            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Select Function", GUILayout.Width(140), GUILayout.Height(15)))
                CreateDropDownMenu(sequence.sequenceElemData[i]);

            sequence.sequenceElemData[i].show_private_methods = GUILayout.Toggle(sequence.sequenceElemData[i].show_private_methods, "show private methods");

            EditorGUILayout.EndHorizontal();

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

        #endregion Show Sequence Elements




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



        void CreateDropDownMenu(SequenceElemData sequenceData)
        {
            GenericMenu dropdownMenu = new GenericMenu();

            List<ComponentBase> CmpList = sequence.entityBase.GetAllComponents();

            List<MethodInfo> methodInfo = new List<MethodInfo>();

            for (int i = 0; i < CmpList.Count; i++)
            {
                if (CmpList[i] != null)
                {
                    if (sequenceData.show_private_methods)
                    {
                        methodInfo.AddRange(CmpList[i].GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).ToList());
                    }
                    else
                    {
                        methodInfo.AddRange(CmpList[i].GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).ToList());
                    }
                }
            }

            methodInfo.Sort((i, j) => string.Compare(i.DeclaringType + "/" + i.Name, j.DeclaringType + "/" + j.Name));

            Debug.Log("всего " + methodInfo.Count + " методов");

            for (int i = 0; i < methodInfo.Count; i++)
            {
                string path = "";

                if (methodInfo[i].DeclaringType?.BaseType == typeof(ComponentBase))
                    path += "RangerV.Components/";

                path += methodInfo[i].DeclaringType + "/";


                dropdownMenu.AddItem(new GUIContent(path + methodInfo[i].Name), false, DDD, i);
            }



            dropdownMenu.ShowAsContext();

        }
    }


    void DDD(object int_value)
    {
        Debug.Log("DDD " + (int)int_value);
    }
}
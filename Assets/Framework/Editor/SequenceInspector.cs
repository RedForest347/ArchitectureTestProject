using RangerV;
using System;
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
        sequence = target as SequenceEventCmp;
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

            List<DropDownMethodData> methodData = new List<DropDownMethodData>();

            for (int i = 0; i < CmpList.Count; i++)
            {
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

                if (sequenceData.show_private_methods)
                    bindingFlags |= BindingFlags.NonPublic;


                List<MethodInfo> Methods = CmpList[i].GetType().GetMethods(bindingFlags).Where((p) => p.GetParameters().Length == 0).ToList();

                for (int k = 0; k < Methods.Count; k++)
                    methodData.Add(new DropDownMethodData(Methods[k], CmpList[i]));

            }

            methodData.Sort((i, j) => string.Compare(i.methodInfo.DeclaringType + "/" + i.methodInfo.Name, j.methodInfo.DeclaringType + "/" + j.methodInfo.Name));

            //Debug.Log("всего " + methodInfo.Count + " методов");

            for (int i = 0; i < methodData.Count; i++)
            {
                string path = "";

                if (methodData[i].methodInfo.DeclaringType?.BaseType == typeof(ComponentBase))
                    path += "RangerV.Components/";

                path += methodData[i].methodInfo.DeclaringType + "/";


                //type = methodInfo[i].DeclaringType
                //method name = methodInfo[i].Name


                dropdownMenu.AddItem(new GUIContent(path + methodData[i].methodInfo.Name), false, SetMethodData, methodData[i]);
            }



            dropdownMenu.ShowAsContext();

        }
    }

    void SetMethodData(object _methodData)
    {
        DropDownMethodData methodData = (DropDownMethodData)_methodData;

        //Debug.Log("(string)arr[0] = " + (string)arr[0]);
        //Debug.Log("new Type = " + Type.GetType(typeof(SomeSeqenceCmp).FullName));

        //Type selectedCmpType = Type.GetType((string)arr[0]);


        // methodInfo[i].DeclaringType.FullName, methodInfo[i].Name, methodInfo[i].DeclaringType.Assembly.FullName, methodInfo[i] 


        //methodData.componentBase

        string type_name = methodData.methodInfo.DeclaringType.FullName;
        string selected_func = methodData.methodInfo.Name;
        string assembly_name = methodData.methodInfo.DeclaringType.Assembly.FullName;

        Type selectedCmpType = Assembly.Load(assembly_name).GetType(type_name);

        Debug.Log("select type = " + selectedCmpType + " select func = " + selected_func + " assembly_name = " + assembly_name);

        ComponentBase componentBase = sequence.entityBase.GetCmp(selectedCmpType);

        

        selectedCmpType.GetMethod(selected_func).Invoke(componentBase, null);


    }


    class DropDownMethodData
    {
        public MethodInfo methodInfo;
        public ComponentBase componentBase;

        public DropDownMethodData(MethodInfo methodInfo, ComponentBase componentBase)
        {
            this.methodInfo = methodInfo;
            this.componentBase = componentBase;
        }
    }
}
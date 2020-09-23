using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
//using UnityEditor.UIElements;
//using UnityEngine.UIElements;
using RangerV;
using System.Linq;
using System;

[CustomPropertyDrawer(typeof(MethodHolder))]
public class MethodHolderEditor : PropertyDrawer
{
    ComponentBase componentBase;
    MethodHolder someT;
    SerializedProperty property;

    public override void OnGUI(Rect position, SerializedProperty _property, GUIContent label)
    {
        property = _property;

        EditorGUI.BeginProperty(position, label, _property);

        GUISkin skin = GUI.skin;

        componentBase = (ComponentBase)(_property.serializedObject.targetObject);
        someT = (MethodHolder)componentBase.GetType().GetField(_property.name).GetValue(_property.serializedObject.targetObject);

        string dropdown_button_name = someT.method_name;


        GUIStyle style = skin.GetStyle("Button");
        GUIContent content = new GUIContent(dropdown_button_name == "" ? "no function selected" : dropdown_button_name);

        style.fixedWidth = Math.Max(new GUIStyle().CalcSize(content).x + 20, 100);
        style.fixedHeight = Math.Max(new GUIStyle().CalcSize(content).y + 5, 20);

        

        if (EditorGUI.DropdownButton(position, content, FocusType.Passive, style))
        {
            CreateDropDownMenu(someT, componentBase.GetComponent<SomeSeqenceCmp>());
        }

        position.x += new GUIStyle().CalcSize(content).x + 60;
        if (EditorGUI.DropdownButton(position, new GUIContent("DoFunc"), FocusType.Passive, style))
        {
            someT.StartMethod();
        }

        EditorGUI.EndProperty();

        void CreateDropDownMenu(MethodHolder someT, ComponentBase componentBase)
        {
            GenericMenu dropdownMenu = new GenericMenu();

            List<ComponentBase> CmpList = componentBase.entityBase.GetAllComponents();
            List<MethodInfo> methodInfos = new List<MethodInfo>(100);

            for (int i = 0; i < CmpList.Count; i++)
            {
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

                List<MethodInfo> Methods = CmpList[i].GetType().GetMethods(bindingFlags)
                    .Where((p) => p.GetParameters().Length == 0 && p.DeclaringType.BaseType == typeof(ComponentBase))
                    .ToList();

                for (int k = 0; k < Methods.Count; k++)
                    methodInfos.Add(Methods[k]);

            }

            methodInfos.Sort((i, j) => string.Compare(i.DeclaringType + "/" + i.Name, j.DeclaringType + "/" + j.Name));

            for (int i = 0; i < methodInfos.Count; i++)
            {
                string path = "";

                //if (methodInfos[i].DeclaringType?.BaseType == typeof(ComponentBase))
                //    path += "RangerV.Components/";

                path += methodInfos[i].DeclaringType + "/";


                bool on = methodInfos[i].Name == someT.method_name;
                dropdownMenu.AddItem(new GUIContent(path + methodInfos[i].Name), on, SetMethodData, methodInfos[i]);
            }

            dropdownMenu.ShowAsContext();
        }
    }

    void SetMethodData(object _methodInfo)
    {
        MethodInfo methodInfo = (MethodInfo)_methodInfo;

        someT.method_name = methodInfo.Name;
        someT.type_name = methodInfo.DeclaringType.Name;
        someT.assembly_name = methodInfo.DeclaringType.Assembly.FullName;
        someT.component = componentBase.GetComponent(methodInfo.DeclaringType);

        EditorUtility.SetDirty(componentBase.gameObject);
        Undo.RecordObject(property.serializedObject.targetObject, "Changed Sequence");

        someT.StartMethod();
    }
}

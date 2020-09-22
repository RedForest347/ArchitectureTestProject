using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
//using UnityEditor.UIElements;
//using UnityEngine.UIElements;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using RangerV;
using System.Linq;
using System;

[CustomPropertyDrawer(typeof(SomeT))]
public class TestEditor : PropertyDrawer
{
    Test test;
    SomeT someT;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        GUISkin skin = GUI.skin;

        GUIStyle style = skin.GetStyle("Button");
        style.fixedWidth = 100;

        test = (Test)(property.serializedObject.targetObject);
        someT = (SomeT)test.GetType().GetField(property.name).GetValue(property.serializedObject.targetObject);

        if (EditorGUI.DropdownButton(position, new GUIContent("DDD"), FocusType.Passive, style))
        {

            CreateDropDownMenu(someT, test.GetComponent<SomeSeqenceCmp>());

            Debug.Log("name = " + property.name);


            /*Type type = property.GetType();

            FieldInfo[] fieldInfo = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            Debug.Log("length = " + fieldInfo.Length);

            for (int i = 0; i < fieldInfo.Length; i++)
            {
                Debug.Log("name = " + fieldInfo[i].Name + " type = " + fieldInfo[i].FieldType);
            }*/
            //property.pro
            //int ddd = property.FindPropertyRelative("ddd").obj;
            //Debug.Log("ddd = " + ddd);
            //CreateDropDownMenu(property., (ComponentBase)(property.serializedObject.targetObject));
            //Debug.Log(((Test)property.serializedObject.targetObject));
        }





        EditorGUI.EndProperty();

        void CreateDropDownMenu(SomeT someT, ComponentBase componentBase)
        {
            GenericMenu dropdownMenu = new GenericMenu();
            //ComponentBase sequence = (ComponentBase)(property.serializedObject.targetObject);

            List<ComponentBase> CmpList = componentBase.entityBase.GetAllComponents();

            List<MethodInfo> methodInfos = new List<MethodInfo>(100);
            //List<DropDownMethodData> methodData = new List<DropDownMethodData>();

            for (int i = 0; i < CmpList.Count; i++)
            {
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

                //if (someT.show_private_methods)
                //    bindingFlags |= BindingFlags.NonPublic;


                List<MethodInfo> Methods = CmpList[i].GetType().GetMethods(bindingFlags).Where((p) => p.GetParameters().Length == 0).ToList();

                for (int k = 0; k < Methods.Count; k++)
                    methodInfos.Add(Methods[k]);

            }

            methodInfos.Sort((i, j) => string.Compare(i.DeclaringType + "/" + i.Name, j.DeclaringType + "/" + j.Name));

            //Debug.Log("всего " + methodInfo.Count + " методов");

            for (int i = 0; i < methodInfos.Count; i++)
            {
                string path = "";

                if (methodInfos[i].DeclaringType?.BaseType == typeof(ComponentBase))
                    path += "RangerV.Components/";

                path += methodInfos[i].DeclaringType + "/";


                //type = methodInfo[i].DeclaringType
                //method name = methodInfo[i].Name


                dropdownMenu.AddItem(new GUIContent(path + methodInfos[i].Name), false, DDD, methodInfos[i]);
            }



            dropdownMenu.ShowAsContext();

        }

    }

    void DDD(object o)
    {
        MethodInfo methodInfo = (MethodInfo)o;
        Debug.Log("Select name = " + methodInfo.Name + " type = " + methodInfo.DeclaringType + " assembly = " + methodInfo.DeclaringType.Assembly.FullName);
    }
    

}

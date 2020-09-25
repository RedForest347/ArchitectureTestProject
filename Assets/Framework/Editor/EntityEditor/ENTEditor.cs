using System.Collections.Generic;

using UnityEngine;

using System.Reflection;
using System;
using System.Linq;
using UnityEditor;

namespace RangerV
{
    #region CUSTOM_ENT_EDITOR_WINDOW_V2
    public class ENTEditor : EditorWindowSelected<Entity>
    {

        [MenuItem("Window/Entity editor")]
        static void GetWindow()
        {
            GetWindow<ENTEditor>("Entity editor");
        }

        protected override void SelectionChange()
        {
            if (selected_object != null)
            {
                if (selected_object.Components.Count == Selection.activeGameObject.GetComponents<ComponentBase>().Length)
                    Debug.Log("компоненты соответствуют действительности ");
                else
                {

                    Debug.LogError("компоненты не соответствуют действительности ");
                }
            }
        }

        protected override void GUIDraw()
        {
            //if (selected_object.show_comp.Count != selected_object.Components.Count)
            //{
            //    selected_object.show_comp = new List<bool>();
            //    for (int i = 0; i < selected_object.Components.Count; i++)
            //        selected_object.show_comp.Add(false);
            //}

            EditorGUILayout.BeginVertical(GUIEditorSettings.box_1_0);
            {
                EditorGUILayout.BeginVertical(GUIEditorSettings.box_1_1);
                EditorGUILayout.LabelField("ETITY:    " + selected_object.entity.ToString() + "    '" + selected_object.gameObject.name + "'", GUIEditorSettings.headerLabel);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();


                #region SHOW_COMPONENTS
                EditorGUILayout.BeginHorizontal(GUIEditorSettings.box_1_1);
                EditorGUILayout.LabelField("Components count:    " + selected_object.Components.Count, EditorStyles.boldLabel);

                if (GUILayout.Button("Component manager", GUIEditorSettings.button_0, GUILayout.Width(140f), GUILayout.Height(20f)))
                {
                    AddComponent();
                }

                EditorGUILayout.EndHorizontal();

                if (selected_object.Components.Count > 0)
                {
                    int compBases_count = selected_object.Components.Count;
                    for (int component_index = 0; component_index < compBases_count; component_index++)
                    {
                        #region SHOW_COMPONENT

                        ComponentBase component = selected_object.Components[component_index];
                        EditorGUILayout.BeginVertical();
                        {
                            if (component != null)
                            {
                                EditorGUILayout.BeginVertical();
                                EditorGUILayout.BeginHorizontal(GUIEditorSettings.box_0_1);
                                {
                                    EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

                                    if (component.GetType().GetCustomAttribute<ComponentAttribute>() != null)
                                        EditorGUILayout.LabelField(new GUIContent(component.GetType().GetCustomAttribute<ComponentAttribute>().GetIcon()), GUILayout.Width(20));
                                    else
                                        EditorGUILayout.LabelField(new GUIContent(EditorGUIUtility.IconContent("dll Script Icon").image), GUILayout.Width(20));

                                    selected_object.show_comp[component_index] = EditorGUILayout.Foldout(selected_object.show_comp[component_index], component.GetType().ToString(), true);
                                    EditorGUILayout.EndHorizontal();

                                    if (component_index != 0 && GUILayout.Button("↑", GUIEditorSettings.button_0, GUILayout.Width(20f)))
                                    {
                                        selected_object.Components[component_index] = selected_object.Components[component_index - 1];
                                        selected_object.Components[component_index - 1] = component;

                                        bool b = selected_object.show_comp[component_index];
                                        selected_object.show_comp[component_index] = selected_object.show_comp[component_index - 1];
                                        selected_object.show_comp[component_index - 1] = b;
                                    }

                                    if (component_index != compBases_count - 1)
                                    {
                                        if (GUILayout.Button("↓", GUIEditorSettings.button_0, GUILayout.Width(20f)))
                                        {
                                            selected_object.Components[component_index] = selected_object.Components[component_index + 1];
                                            selected_object.Components[component_index + 1] = component;

                                            bool b = selected_object.show_comp[component_index];
                                            selected_object.show_comp[component_index] = selected_object.show_comp[component_index + 1];
                                            selected_object.show_comp[component_index + 1] = b;
                                        }
                                    }
                                    else
                                        EditorGUILayout.LabelField("", GUILayout.Width(20));

                                    if (GUILayout.Button("Remove", GUIEditorSettings.button_0, GUILayout.Width(70f)))
                                    {
                                        Debug.Log("pressed Remove Button");
                                        RemoveItem(component_index);
                                        break;
                                    }
                                }
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.EndVertical();
                                ShowComponentFields(component, component_index);
                            }
                            else
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.HelpBox("Component missing", MessageType.Warning);
                                if (GUILayout.Button("Remove", GUIEditorSettings.button_0, GUILayout.Width(70f)))
                                {
                                    RemoveItem(component_index);
                                    break;
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        EditorGUILayout.EndVertical();
                        #endregion
                    }
                }
                #endregion
            }
            EditorGUILayout.EndHorizontal();


        }




        void AddComponent()
        {
            GenericMenu dropdownMenu = new GenericMenu();
            Type componentType = typeof(ComponentBase);
            List<Type> add_component_list = Assembly.GetAssembly(componentType)
                                     .GetTypes()
                                     .Where(type =>
                                     {
                                         return type.IsSubclassOf(componentType)
                                         && type.GetCustomAttribute<HideComponent>() == null;
                                         //&& selected_object.compBases.Where(componentBase => componentBase.GetType() == type).ToList().Count == 0;
                                     })
                                     .ToList();

            AddItems(true);
            dropdownMenu.AddSeparator("");
            AddItems(false);
            dropdownMenu.ShowAsContext();

            void AddItems(bool with_component_attribute)
            {
                for (int i = 0; i < add_component_list.Count; i++)
                {
                    if (with_component_attribute ? (add_component_list[i].GetCustomAttribute<ComponentAttribute>() != null)
                                                 : (add_component_list[i].GetCustomAttribute<ComponentAttribute>() == null))
                    {
                        string menuPath;

                        if (with_component_attribute) 
                            menuPath = add_component_list[i].GetCustomAttribute<ComponentAttribute>().GetPath();
                        else 
                            menuPath = add_component_list[i].Name;

                        bool set = selected_object.Components.Where(componentBase => componentBase.GetType() == add_component_list[i]).ToList().Count != 0;

                        /*if (selected_object.Components.Where(componentBase => componentBase.GetType() == add_component_list[i]).ToList().Count != 0) 
                            set = true;
                        */
                        
                        if (set) 
                            dropdownMenu.AddItem(new GUIContent(menuPath), set, RemoveItem, add_component_list[i]);
                        else 
                            dropdownMenu.AddItem(new GUIContent(menuPath), set, AddItem, add_component_list[i]);
                    }
                }
            }

        }

        void AddItem(object componentType)
        {
            if (componentType != null)
            {
                //Component comp = selected_object.gameObject.AddComponent((Type)componentType);
                //comp.hideFlags = HideFlags.HideInInspector;
                //selected_object.Components.Add(comp as ComponentBase);
                selected_object.AddCmp((Type)componentType);
                //selected_object.show_comp.Add(false);
                ApplyPrefab();
            }
        }


        void RemoveItem(int index)
        {
            //DestroyImmediate(selected_object.Components[index]);
            //selected_object.Components.RemoveAt(index);
            selected_object.RemoveCmp(selected_object.Components[index].GetType());
            //selected_object.show_comp.RemoveAt(index);///
            ApplyPrefab();
        }
        void RemoveItem(object type)
        {
            for (int index = 0; index < selected_object.Components.Count; index++)
                if (selected_object.Components[index].GetType() == (Type)type)
                    RemoveItem(index);

            ApplyPrefab();
        }

        void ApplyPrefab()
        {
            if (selected_object.gameObject == null)
                Debug.Log("selected_object.gameObject == null");

            EditorUtility.SetDirty(selected_object);
            Undo.RecordObject(selected_object, "Changed selected_object");

            //PrefabUtility.ApplyPrefabInstance(selected_object.gameObject, InteractionMode.AutomatedAction); // сделать корректную отмену добавления/удаления компонентов
            //EditorUtility.SetDirty(selected_object.gameObject);
            //нужно как то помутить сцену грязной
        }

        void ShowComponentFields(ComponentBase component, int index)
        {
            if (selected_object.show_comp[index])
            {
                GUILayout.Space(3);
                EditorGUILayout.BeginVertical(GUIEditorSettings.box_0_0);
                {
                    EditorGUILayout.Separator();
                    Editor editor = Editor.CreateEditor(component);
                    editor.OnInspectorGUI();
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(20);
            }
        }

    }
    #endregion

}

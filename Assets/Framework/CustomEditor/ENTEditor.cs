using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;

namespace RangerV
{
    #region CUSTOM_ENT_INSPECTOR
    //[CustomEditor(typeof(ENT))]
    //public class ENTEditor : Editor
    //{
    //    ENT selected_object;
    //    private SerializedProperty list;
    //    ENTEditorSettings ENTEditorSettings;
    //    //GUIEditorSettings GUIEditorSettings;

    //    public void OnEnable()
    //    {
    //        selected_object = (ENT)target;
    //        list = serializedObject.FindProperty("compBases");
    //        if (selected_object.editorVariables.show_comp.Count != selected_object.Components.Count && selected_object.Components.Count != 0)
    //            selected_object.editorVariables.show_comp = new List<bool>(selected_object.Components.Count);
    //        ENTEditorSettings = GameObject.Find("[SETUP]").GetComponent<ENTEditorSettings>();
    //    }


    //    public override void OnInspectorGUI()
    //    {
    //        SkinInitialize();
    //        GUIStyle headerLabel = new GUIStyle(EditorStyles.boldLabel);
    //        headerLabel.alignment = TextAnchor.MiddleCenter;

    //        DefaultInspector();
    //        GUILayout.Space(20);


    //        EditorGUILayout.BeginVertical("box");
    //        EditorGUILayout.LabelField("ETITY    " + selected_object.entity.ToString(), headerLabel);
    //        EditorGUILayout.EndVertical();


    //        EditorGUILayout.Separator();
    //        serializedObject.Update();
    //        EditorGUILayout.BeginVertical("box");
    //        {
    //            #region SHOW_COMPONENTS
    //            EditorGUILayout.LabelField("Components count:    " + selected_object.Components.Count, headerLabel);
    //            EditorGUILayout.Separator();
    //            if (selected_object.Components.Count > 0)
    //            {
    //                int compBases_count = selected_object.Components.Count;
    //                for (int component_index = 0; component_index < compBases_count; component_index++)
    //                {
    //                    #region SHOW_COMPONENT
    //                    ComponentBase component = selected_object.Components[component_index];
    //                    //GUI.backgroundColor = ENTEditorSettings.ui_color_2;
    //                    EditorGUILayout.BeginVertical("box");
    //                    EditorGUILayout.BeginHorizontal();
    //                    {
    //                        selected_object.editorVariables.show_comp[component_index] = EditorGUILayout.Foldout(selected_object.editorVariables.show_comp[component_index], component.GetType().ToString(), true);
    //                        if (component_index != 0 && GUILayout.Button("/\\", GUILayout.Width(20f)))
    //                        {
    //                            selected_object.Components[component_index] = selected_object.Components[component_index - 1];
    //                            selected_object.Components[component_index - 1] = component;
    //                        }
    //                        if (component_index != compBases_count - 1 && GUILayout.Button("\\/", GUILayout.Width(20f)))
    //                        {
    //                            selected_object.Components[component_index] = selected_object.Components[component_index + 1];
    //                            selected_object.Components[component_index + 1] = component;
    //                        }
    //                        if (GUILayout.Button("Remove", GUILayout.Width(70f)))
    //                        {
    //                            RemoveItem(component_index);
    //                            continue;
    //                        }
    //                    }
    //                    EditorGUILayout.EndHorizontal();
    //                    ShowComponentFields(component, component_index);
    //                    EditorGUILayout.EndVertical();
    //                    #endregion
    //                }
    //            }
    //            #endregion

    //            EditorGUILayout.Separator();
    //            if (GUILayout.Button("Component manager", GUILayout.Width(140f), GUILayout.Height(20f)))
    //            {
    //                AddComponent();
    //            }
    //        }
    //        EditorGUILayout.EndVertical();
    //        serializedObject.ApplyModifiedProperties();



    //        EditorGUILayout.Separator();
    //        EditorGUILayout.Separator();
    //    }




    //    void AddComponent()
    //    {
    //        GenericMenu dropdownMenu = new GenericMenu();
    //        Type componentType = typeof(ComponentBase);
    //        List<Type> add_component_list = Assembly.GetAssembly(componentType)
    //                                 .GetTypes()
    //                                 .Where(type =>
    //                                 {
    //                                     return type.IsSubclassOf(componentType)
    //                                     && type.GetCustomAttribute<HideComponent>() == null;
    //                                             //&& selected_object.compBases.Where(componentBase => componentBase.GetType() == type).ToList().Count == 0;
    //                                         })
    //                                 .ToList();

    //        AddItems(true);
    //        dropdownMenu.AddSeparator("");
    //        AddItems(false);
    //        //dropdownMenu.DropDown(new Rect(100, 100, 200, 200));
    //        dropdownMenu.ShowAsContext();

    //        void AddItems(bool with_component_attribute)
    //        {
    //            for (int i = 0; i < add_component_list.Count; i++)
    //            {
    //                if (with_component_attribute ? (add_component_list[i].GetCustomAttribute<ComponentAttribute>() != null)
    //                                             : (add_component_list[i].GetCustomAttribute<ComponentAttribute>() == null))
    //                {
    //                    string menuPath = "";
    //                    if (with_component_attribute) menuPath = add_component_list[i].GetCustomAttribute<ComponentAttribute>().GetPath();
    //                    else menuPath = add_component_list[i].Name;
    //                    bool set = false;
    //                    if (selected_object.Components.Where(componentBase => componentBase.GetType() == add_component_list[i]).ToList().Count != 0) set = true;
    //                    if (set) dropdownMenu.AddItem(new GUIContent(menuPath), set, RemoveItem, add_component_list[i]);
    //                    else dropdownMenu.AddItem(new GUIContent(menuPath), set, AddItem, add_component_list[i]);
    //                }
    //            }
    //        }

    //    }
    //    void AddItem(object componentType)
    //    {
    //        if (componentType != null)
    //        {
    //            if (selected_object.ComponentHolder == null)
    //            {
    //                selected_object.ComponentHolder = new GameObject();
    //                selected_object.ComponentHolder.name = "ComponentHolder";
    //                selected_object.ComponentHolder.transform.parent = selected_object.transform;
    //                selected_object.ComponentHolder.transform.localPosition = Vector3.zero;
    //            }
    //            Component comp = selected_object.ComponentHolder.AddComponent((Type)componentType);

    //            //comp.hideFlags = HideFlags.HideInInspector;

    //            selected_object.Components.Add(comp as ComponentBase);
    //            selected_object.editorVariables.show_comp.Add(false);
    //        }
    //        AddComponent();
    //    }


    //    void RemoveItem(int index)
    //    {
    //        DestroyImmediate(selected_object.Components[index]);
    //        selected_object.Components.RemoveAt(index);
    //        selected_object.editorVariables.show_comp.RemoveAt(index);
    //    }
    //    void RemoveItem(object type)
    //    {
    //        for (int index = 0; index < selected_object.Components.Count; index++)
    //            if (selected_object.Components[index].GetType() == (Type)type)
    //                RemoveItem(index);
    //    }


    //    void ShowComponentFields(ComponentBase component, int index)
    //    {
    //        #region REFLECTION
    //        //Type type = component.GetType();

    //        //FieldInfo[] fields_info = type.GetFields();

    //        //for (int field = fields_info.Length - 1; field >= 0; field--)
    //        //{
    //        //    EditorGUILayout.BeginVertical();
    //        //    {
    //        //        EditorGUILayout.LabelField(fields_info[field].Name);
    //        //        //EditorGUILayout.PropertyField(component);
    //        //        //EditorGUILayout.PropertyField(new SerializedProperty().serializedObject.f)


    //        //        EditorGUILayout.BeginVertical(contentBox);
    //        //        {
    //        //            Type type_ch = fields_info[field].FieldType;
    //        //            FieldInfo[] fields_info_ch = type_ch.GetFields();

    //        //            for (int field_ch = fields_info_ch.Length - 1; field_ch >= 0; field_ch--)
    //        //            {
    //        //                EditorGUILayout.LabelField(fields_info_ch[field_ch].Name);
    //        //            }
    //        //        }
    //        //        EditorGUILayout.EndVertical();

    //        //    }
    //        //    EditorGUILayout.EndVertical();


    //        //}
    //        #endregion

    //        //if (!selected_object.show_components_in_inspector)
    //        //selected_object.compBases[index].hideFlags = HideFlags.HideInInspector;
    //        if (selected_object.editorVariables.show_comp[index])
    //        {
    //            EditorGUILayout.BeginVertical();
    //            {
    //                EditorGUILayout.Separator();
    //                var editor = Editor.CreateEditor(component);
    //                editor.OnInspectorGUI();
    //            }
    //            EditorGUILayout.EndVertical();
    //        }
    //    }


    //    void SkinInitialize()
    //    {
    //        if (ENTEditorSettings.useCustomSkin)
    //            GUI.skin = ENTEditorSettings.skin;
    //        else
    //        {
    //            GUI.skin.box.padding.left = 15;
    //            GUI.skin.box.padding.right = 7;
    //            GUI.skin.box.padding.bottom = 7;
    //            GUI.skin.box.padding.top = 7;

    //            EditorStyles.helpBox.padding.left = 15;
    //            EditorStyles.helpBox.padding.right = 7;
    //            EditorStyles.helpBox.padding.bottom = 7;
    //            EditorStyles.helpBox.padding.top = 7;
    //        }
    //        GUI.backgroundColor = ENTEditorSettings.ui_color_1;
    //    }


    //    #region DEFAULT INSPECTOR

    //    bool is_insp_show;
    //    void DefaultInspector()
    //    {
    //        is_insp_show = EditorGUILayout.Foldout(is_insp_show, "Default Inspector");
    //        if (is_insp_show)
    //        {
    //            DrawDefaultInspector();
    //        }
    //    }

    //    #endregion

    //}
    #endregion




    #region CUSTOM_ENT_EDITOR_WINDOW_V1
    //public class ENTEditor : EditorWindow
    //{
    //    bool is_entity_selected = false;
    //    GameObject selected_object_GO = null;
    //    ENT selected_object = null;
    //    SerializedObject serializedObject = null;
    //    bool locked;
    //    Vector2 scroll;
    //    GUIEditorSettings GUIEditorSettings;
    //    ENTEditorSettings EditorSettings;


    //    [MenuItem("Window/Entity editor")]
    //    static void GetWindow()
    //    {
    //        GetWindow<ENTEditor>("Entity editor");
    //    }
    //    private void OnEnable()
    //    {
    //        GUIEditorSettings = new GUIEditorSettings();
    //        EditorSettings = EditorGUIUtility.Load("Entity editor settings") as ENTEditorSettings;
    //    }
    //    void OnGUI()
    //    {
    //        EditorGUILayout.BeginHorizontal(EditorStyles.toolbarButton);
    //        {
    //            locked = GUILayout.Toggle(locked, EditorGUIUtility.IconContent("InspectorLock"), EditorStyles.toolbarButton, GUILayout.Width(30));
    //            GUILayout.FlexibleSpace();
    //            if (GUILayout.Button("Override prefab", EditorStyles.toolbarButton, GUILayout.Width(100)))
    //                PrefabUtility.ApplyPrefabInstance(selected_object_GO, InteractionMode.UserAction);
    //            if (GUILayout.Button("Revert prefab changes", EditorStyles.toolbarButton, GUILayout.Width(150)))
    //                PrefabUtility.RevertPrefabInstance(selected_object_GO, InteractionMode.UserAction);
    //        }
    //        EditorGUILayout.EndHorizontal();

    //        if (is_entity_selected)
    //        {
    //            scroll = EditorGUILayout.BeginScrollView(scroll);
    //            if (selected_object == null || serializedObject == null)
    //                CheckState();
    //            GUIEditorSettings.SkinInitialize();
    //            MainGUI();
    //            EditorGUILayout.EndScrollView();
    //        }
    //        else
    //        {
    //            EditorGUILayout.HelpBox("Choose entity to edit (GameObject with ENT class)", MessageType.Info);
    //        }
    //    }
    //    private void OnInspectorUpdate()
    //    {
    //        CheckState();
    //        Repaint();
    //    }
    //    void CheckState()
    //    {
    //        ENT ent = null;
    //        if (Selection.activeGameObject != null)
    //            ent = Selection.activeGameObject.GetComponent<ENT>();
    //        if (ent != null)
    //        {
    //            is_entity_selected = true;
    //            selected_object = ent;
    //            serializedObject = new SerializedObject(selected_object);
    //            selected_object_GO = Selection.activeGameObject;
    //            Repaint();
    //            if (ent.show_comp.Count != ent.Components.Count)
    //            {
    //                ent.show_comp = new List<bool>();
    //                for (int i = 0; i < ent.Components.Count; i++)
    //                    ent.show_comp.Add(false);
    //            }
    //        }
    //        else
    //        {
    //            is_entity_selected = false;
    //            selected_object = null;
    //            serializedObject = null;
    //            selected_object_GO = null;
    //            Repaint();
    //        }
    //    }







    //    void MainGUI()
    //    {

    //        EditorGUILayout.BeginVertical(GUIEditorSettings.box_1_0);
    //        {
    //            EditorGUILayout.BeginVertical(GUIEditorSettings.box_1_1);
    //            EditorGUILayout.LabelField("ETITY:    " + selected_object.entity.ToString() + "    '" + selected_object.gameObject.name + "'", GUIEditorSettings.headerLabel);
    //            EditorGUILayout.EndVertical();
    //            EditorGUILayout.Separator();


    //            #region SHOW_COMPONENTS
    //            EditorGUILayout.BeginHorizontal(GUIEditorSettings.box_1_1);
    //            EditorGUILayout.LabelField("Components count:    " + selected_object.Components.Count, EditorStyles.boldLabel);
    //            if (GUILayout.Button("Component manager", GUILayout.Width(140f), GUILayout.Height(20f)))
    //            {
    //                AddComponent();
    //            }
    //            EditorGUILayout.EndHorizontal();

    //            if (selected_object.Components.Count > 0)
    //            {
    //                int compBases_count = selected_object.Components.Count;
    //                for (int component_index = 0; component_index < compBases_count; component_index++)
    //                {
    //                    #region SHOW_COMPONENT
    //                    ComponentBase component = selected_object.Components[component_index];
    //                    //GUI.backgroundColor = ENTEditorSettings.ui_color_2;
    //                    EditorGUILayout.BeginVertical();
    //                    {
    //                        if (component != null)
    //                        {
    //                            EditorGUILayout.BeginVertical();
    //                            EditorGUILayout.BeginHorizontal("box");
    //                            {
    //                                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
    //                                if (component.GetType().GetCustomAttribute<ComponentAttribute>() != null)
    //                                    EditorGUILayout.LabelField(new GUIContent(component.GetType().GetCustomAttribute<ComponentAttribute>().GetIcon()), GUILayout.Width(20));
    //                                else
    //                                    EditorGUILayout.LabelField(new GUIContent(EditorGUIUtility.IconContent("dll Script Icon").image), GUILayout.Width(20));
    //                                selected_object.show_comp[component_index] = EditorGUILayout.Foldout(selected_object.show_comp[component_index], component.GetType().ToString(), true);
    //                                EditorGUILayout.EndHorizontal();

    //                                if (component_index != 0 && GUILayout.Button("↑", GUILayout.Width(20f)))
    //                                {
    //                                    selected_object.Components[component_index] = selected_object.Components[component_index - 1];
    //                                    selected_object.Components[component_index - 1] = component;

    //                                    bool b = selected_object.show_comp[component_index];
    //                                    selected_object.show_comp[component_index] = selected_object.show_comp[component_index - 1];
    //                                    selected_object.show_comp[component_index - 1] = b;
    //                                }
    //                                if (component_index != compBases_count - 1)
    //                                {
    //                                    if (GUILayout.Button("↓", GUILayout.Width(20f)))
    //                                    {
    //                                        selected_object.Components[component_index] = selected_object.Components[component_index + 1];
    //                                        selected_object.Components[component_index + 1] = component;

    //                                        bool b = selected_object.show_comp[component_index];
    //                                        selected_object.show_comp[component_index] = selected_object.show_comp[component_index + 1];
    //                                        selected_object.show_comp[component_index + 1] = b;
    //                                    }
    //                                }
    //                                else
    //                                    EditorGUILayout.LabelField("", GUILayout.Width(20));
    //                                if (GUILayout.Button("Remove", GUILayout.Width(70f)))
    //                                {
    //                                    RemoveItem(component_index);
    //                                    break;
    //                                }
    //                            }
    //                            EditorGUILayout.EndHorizontal();
    //                            EditorGUILayout.EndVertical();
    //                            ShowComponentFields(component, component_index);
    //                        }
    //                        else
    //                        {
    //                            EditorGUILayout.BeginHorizontal();
    //                            EditorGUILayout.HelpBox("Component missing", MessageType.Warning);
    //                            if (GUILayout.Button("Remove", GUILayout.Width(70f)))
    //                            {
    //                                RemoveItem(component_index);
    //                                break;
    //                            }
    //                            EditorGUILayout.EndHorizontal();
    //                        }
    //                    }
    //                    EditorGUILayout.EndVertical();
    //                    #endregion
    //                }
    //            }
    //            #endregion
    //        }
    //        EditorGUILayout.EndHorizontal();



    //    }






    //    void AddComponent()
    //    {
    //        GenericMenu dropdownMenu = new GenericMenu();
    //        Type componentType = typeof(ComponentBase);
    //        List<Type> add_component_list = Assembly.GetAssembly(componentType)
    //                                 .GetTypes()
    //                                 .Where(type =>
    //                                 {
    //                                     return type.IsSubclassOf(componentType)
    //                                     && type.GetCustomAttribute<HideComponent>() == null;
    //                                     //&& selected_object.compBases.Where(componentBase => componentBase.GetType() == type).ToList().Count == 0;
    //                                 })
    //                                 .ToList();

    //        AddItems(true);
    //        dropdownMenu.AddSeparator("");
    //        AddItems(false);
    //        dropdownMenu.ShowAsContext();

    //        void AddItems(bool with_component_attribute)
    //        {
    //            for (int i = 0; i < add_component_list.Count; i++)
    //            {
    //                if (with_component_attribute ? (add_component_list[i].GetCustomAttribute<ComponentAttribute>() != null)
    //                                             : (add_component_list[i].GetCustomAttribute<ComponentAttribute>() == null))
    //                {
    //                    string menuPath = "";
    //                    if (with_component_attribute) menuPath = add_component_list[i].GetCustomAttribute<ComponentAttribute>().GetPath();
    //                    else menuPath = add_component_list[i].Name;
    //                    bool set = false;
    //                    if (selected_object.Components.Where(componentBase => componentBase.GetType() == add_component_list[i]).ToList().Count != 0) set = true;
    //                    if (set) dropdownMenu.AddItem(new GUIContent(menuPath), set, RemoveItem, add_component_list[i]);
    //                    else dropdownMenu.AddItem(new GUIContent(menuPath), set, AddItem, add_component_list[i]);
    //                }
    //            }
    //        }

    //    }
    //    void AddItem(object componentType)
    //    {
    //        if (componentType != null)
    //        {
    //            Component comp = selected_object.gameObject.AddComponent((Type)componentType);
    //            //comp.hideFlags = HideFlags.HideInInspector;
    //            selected_object.Components.Add(comp as ComponentBase);
    //            selected_object.show_comp.Add(false);
    //        }
    //    }


    //    void RemoveItem(int index)
    //    {
    //        DestroyImmediate(selected_object.Components[index]);
    //        selected_object.Components.RemoveAt(index);
    //        selected_object.show_comp.RemoveAt(index);
    //    }
    //    void RemoveItem(object type)
    //    {
    //        for (int index = 0; index < selected_object.Components.Count; index++)
    //            if (selected_object.Components[index].GetType() == (Type)type)
    //                RemoveItem(index);
    //    }


    //    void ShowComponentFields(ComponentBase component, int index)
    //    {
    //        #region REFLECTION
    //        //Type type = component.GetType();

    //        //FieldInfo[] fields_info = type.GetFields();

    //        //for (int field = fields_info.Length - 1; field >= 0; field--)
    //        //{
    //        //    EditorGUILayout.BeginVertical();
    //        //    {
    //        //        EditorGUILayout.LabelField(fields_info[field].Name);
    //        //        //EditorGUILayout.PropertyField(component);
    //        //        //EditorGUILayout.PropertyField(new SerializedProperty().serializedObject.f)


    //        //        EditorGUILayout.BeginVertical(contentBox);
    //        //        {
    //        //            Type type_ch = fields_info[field].FieldType;
    //        //            FieldInfo[] fields_info_ch = type_ch.GetFields();

    //        //            for (int field_ch = fields_info_ch.Length - 1; field_ch >= 0; field_ch--)
    //        //            {
    //        //                EditorGUILayout.LabelField(fields_info_ch[field_ch].Name);
    //        //            }
    //        //        }
    //        //        EditorGUILayout.EndVertical();

    //        //    }
    //        //    EditorGUILayout.EndVertical();


    //        //}
    //        #endregion

    //        if (selected_object.show_comp[index])
    //        {
    //            GUILayout.Space(3);
    //            EditorGUILayout.BeginVertical(GUIEditorSettings.box_0_0);
    //            {
    //                EditorGUILayout.Separator();
    //                var editor = Editor.CreateEditor(component);
    //                editor.OnInspectorGUI();
    //            }
    //            EditorGUILayout.EndVertical();
    //            GUILayout.Space(20);
    //        }
    //    }

    //}
    #endregion

    #region CUSTOM_ENT_EDITOR_WINDOW_V2
    public class ENTEditor : EditorWindowSelected<Entity>
    {

        [MenuItem("Window/Entity editor")]
        static void GetWindow()
        {
            GetWindow<ENTEditor>("Entity editor");
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
                        string menuPath = "";
                        if (with_component_attribute) menuPath = add_component_list[i].GetCustomAttribute<ComponentAttribute>().GetPath();
                        else menuPath = add_component_list[i].Name;
                        bool set = false;
                        if (selected_object.Components.Where(componentBase => componentBase.GetType() == add_component_list[i]).ToList().Count != 0) set = true;
                        if (set) dropdownMenu.AddItem(new GUIContent(menuPath), set, RemoveItem, add_component_list[i]);
                        else dropdownMenu.AddItem(new GUIContent(menuPath), set, AddItem, add_component_list[i]);
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
                selected_object.Add((Type)componentType);
                selected_object.show_comp.Add(false);
            }
        }


        void RemoveItem(int index)
        {
            //DestroyImmediate(selected_object.Components[index]);
            //selected_object.Components.RemoveAt(index);
            selected_object.RemoveComponent(selected_object.Components[index].GetType());
            selected_object.show_comp.RemoveAt(index);
        }
        void RemoveItem(object type)
        {
            for (int index = 0; index < selected_object.Components.Count; index++)
                if (selected_object.Components[index].GetType() == (Type)type)
                    RemoveItem(index);
        }

        void ShowComponentFields(ComponentBase component, int index)
        {
            #region REFLECTION
            //Type type = component.GetType();

            //FieldInfo[] fields_info = type.GetFields();

            //for (int field = fields_info.Length - 1; field >= 0; field--)
            //{
            //    EditorGUILayout.BeginVertical();
            //    {
            //        EditorGUILayout.LabelField(fields_info[field].Name);
            //        //EditorGUILayout.PropertyField(component);
            //        //EditorGUILayout.PropertyField(new SerializedProperty().serializedObject.f)


            //        EditorGUILayout.BeginVertical(contentBox);
            //        {
            //            Type type_ch = fields_info[field].FieldType;
            //            FieldInfo[] fields_info_ch = type_ch.GetFields();

            //            for (int field_ch = fields_info_ch.Length - 1; field_ch >= 0; field_ch--)
            //            {
            //                EditorGUILayout.LabelField(fields_info_ch[field_ch].Name);
            //            }
            //        }
            //        EditorGUILayout.EndVertical();

            //    }
            //    EditorGUILayout.EndVertical();


            //}
            #endregion

            if (selected_object.show_comp[index])
            {
                GUILayout.Space(3);
                EditorGUILayout.BeginVertical(GUIEditorSettings.box_0_0);
                {
                    EditorGUILayout.Separator();
                    var editor = Editor.CreateEditor(component);
                    editor.OnInspectorGUI();
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(20);
            }
        }

    }
    #endregion

}

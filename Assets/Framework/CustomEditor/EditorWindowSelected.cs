using UnityEngine;
using UnityEditor;
using System.IO;

namespace RangerV
{
    public class ObjectContainer<T>
    {
        public T _object;
    }

    public class EditorWindowSelected<T> : EditorWindowCore where T : class
    {
        public GameObject selected_gameObject;
        public string selected_object_json;
        public T selected_object
        {
            get
            {
                return JsonUtility.FromJson<ObjectContainer<T>>(selected_object_json)._object;
            }
            set
            {
                ObjectContainer<T> container = new ObjectContainer<T>();
                container._object = value;
                selected_object_json = JsonUtility.ToJson(container);
            }
        }
        public SerializedObject serializedObject;
        public bool locked;
        public ScriptableObject editor_settings;
        



        protected new void OnEnable()
        {
            base.OnEnable();
            Enable();
        }
        protected new void OnInspectorUpdate()
        {
            base.OnInspectorUpdate();
            CheckState();
        }
        protected new void OnSelectionChange()
        {
            base.OnSelectionChange();
            CheckState();
        }
        protected new void OnGUI()
        {
            base.OnGUI();
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbarButton);
            {
                locked = GUILayout.Toggle(locked, EditorGUIUtility.IconContent("InspectorLock"), EditorStyles.toolbarButton, GUILayout.Width(30));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Override prefab", EditorStyles.toolbarButton, GUILayout.Width(100)))
                    PrefabUtility.ApplyPrefabInstance(selected_gameObject, InteractionMode.UserAction);
                if (GUILayout.Button("Revert prefab changes", EditorStyles.toolbarButton, GUILayout.Width(150)))
                    PrefabUtility.RevertPrefabInstance(selected_gameObject, InteractionMode.UserAction);
            }
            EditorGUILayout.EndHorizontal();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            {
                //EditorGUILayout.LabelField("selected_object :  " + (selected_object?.ToString() ?? "null"));
                //EditorGUILayout.LabelField("Selection.activeGameObject :  " + (Selection.activeGameObject?.ToString() ?? "null"));
                //EditorGUILayout.LabelField("Lock :  " + locked.ToString());
                //EditorGUILayout.LabelField("selected_object_json :  " + selected_object_json));

                if (!selected_object?.Equals(null) ?? false)
                {
                    //if (EditorSettings.useCustomSkin)
                    //    GUI.skin = EditorSettings.skin;

                    GUI.color = GUIEditorSettings.mainUIColor;
                    GUI.contentColor = GUIEditorSettings.mainContentUIColor;
                    GUI.backgroundColor = GUIEditorSettings.mainBackgroundUIColor;
                    GUIEditorSettings.SkinInitialize();

                    GUIDraw();
                }
                else
                {
                    EditorGUILayout.HelpBox("Choose entity to edit (GameObject with ENT class)", MessageType.Info);
                }
            }
            EditorGUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbarButton);
            {
                EditorGUILayout.LabelField("GUI style: " + stylePath);
            }
            EditorGUILayout.EndHorizontal();
        }




        void CheckState()
        {
            if (!locked)
            {
                GameObject go = Selection.activeGameObject;
                T obj = go?.GetComponent<T>();
                if (!obj?.Equals(null) ?? false)
                {
                    selected_object = obj;
                    selected_gameObject = go;
                    serializedObject = new SerializedObject(obj as UnityEngine.Object);
                }
                else
                {
                    selected_object = null;
                    selected_gameObject = null;
                    serializedObject = null;
                }
                Repaint();
            }
        }


        protected virtual void GUIDraw() { }
        protected virtual void Enable() { }

    }
}

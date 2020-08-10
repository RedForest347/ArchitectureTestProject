using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using UnityEditor;
using System;

[CustomEditor(typeof(PlayerShipControlComponent))]
public class PlayerShipControlComponentEditor : Editor
{
    PlayerShipControlComponent myScript;
    private SerializedProperty controlSettings;
    private CustomGUIeditorSettings GUIEditorSettings;

    public void OnEnable()
    {
        try
        {
            myScript = (PlayerShipControlComponent)target;
            controlSettings = serializedObject.FindProperty("ControlSettings");
            GUIEditorSettings = (CustomGUIeditorSettings)Resources.Load("EntityGUIeditorSettings");
        }
        catch (Exception ex) { ex.ToString(); }
    }

    public override void OnInspectorGUI()
    {
        GUIEditorSettings.SkinInitialize();

        serializedObject.Update();
        CustomEditorBlocks.ShowScriptableObject(controlSettings, myScript.ControlSettings, GUIEditorSettings.box_0_1);
        serializedObject.ApplyModifiedProperties();
    }

}




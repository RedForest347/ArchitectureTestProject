using System;
using UnityEditor;
using UnityEngine;

namespace RangerV
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HideComponent : Attribute
    {

    }


    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        private string path = "";
        private Texture icon = null;
        public ComponentAttribute(string path, Texture icon)
        {
            this.path = path;
            this.icon = icon;
        }
        public ComponentAttribute(string path, string unity_editor_icon)
        {
            this.path = path;
            this.icon = EditorGUIUtility.IconContent(unity_editor_icon).image;
        }
        public ComponentAttribute(string path)
        {
            this.path = path;
            this.icon = EditorGUIUtility.IconContent("dll Script Icon").image;
        }
        public string GetPath()
        {
            return path;
        }
        public Texture GetIcon()
        {
            return icon;
        }
    }


    [AttributeUsage(AttributeTargets.Field)]
    class PoolAttribute : Attribute
    {

    }


}
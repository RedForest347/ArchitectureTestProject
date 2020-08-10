using System.Collections.Generic;
using UnityEngine;

namespace RangerV
{

    public class Entity : EntityBase
    {
        //public List<ComponentBase> compBases = new List<ComponentBase>();
        //public EditorVariables editorVariables;
        public List<bool> show_comp = new List<bool>();

        public override void Setup()
        {
            /*for (int i = 0; i < Components.Count; i++)
            {
                AddFromStartList(Components[i]);
            }*/
        }
    }
}

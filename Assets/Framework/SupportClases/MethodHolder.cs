using System.Reflection;
using UnityEngine;


namespace RangerV
{

    [System.Serializable]
    public class MethodHolder
    {
        //Inspector data
        public bool show_private_methods;

        //Method Data
        public string type_name;
        public string method_name;
        public string assembly_name;
        public Component component;


        MethodInfo cashedMethodInfo;

        public void StartMethod()
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Assembly.Load(assembly_name).GetType(type_name).GetMethod(method_name, bindingFlags).Invoke(component, null);
        }


        //используется для кеширования cashedMethodInfo и более быстрого последующего вызова
        // рекомендуется вызывать в старте или около того (при загрузке сцены)
        /*public void Init()
        {
            if (assembly_name == "" || assembly_name == null)
                throw new System.ArgumentNullException("не определена функция в MethodHolder, но произведена попытка ее вызова");

            cashedMethodInfo = Assembly.Load(assembly_name).GetType(type_name).GetMethod(method_name);
        }*/

    }

}

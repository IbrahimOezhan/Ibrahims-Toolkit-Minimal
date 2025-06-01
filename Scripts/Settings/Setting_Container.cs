using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TemplateTools;
using UnityEngine;

[Serializable]
public class Setting_Container
{
    [SerializeField, SerializeReference] private Setting setting;

    [SerializeField, OnValueChanged("OnValueChanged"), ValueDropdown("GetAllSubtypes")] private string extension = "None";
    public Setting GetSetting()
    {
        return setting;
    }

    private Type[] GetDerivedTypes()
    {
        var baseType = typeof(Setting);

        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.IsClass
                        && !t.IsAbstract
                        && baseType.IsAssignableFrom(t)).ToArray();
    }

    private IEnumerable GetAllSubtypes()
    {
        List<string> subtypes = GetDerivedTypes().Select(x => x.Name).ToList();
        subtypes.Sort((a, b) => 
        {
            return a.CompareTo(b);
            });
        subtypes.Insert(0, "None");
        return subtypes;
    }

    public void OnValueChanged()
    {
        List<Type> types = GetDerivedTypes().ToList();

        Type type = types.Find(x => x.Name == extension);

        if (type != null)
        {
            setting = (Setting)Activator.CreateInstance(type);
        }

        extension = "None";
    }
}

using Collection;
using UnityEngine;
using UnityEditor;
using Managers; 

[CustomPropertyDrawer(typeof(AreaDropdownAttribute))]
public class AreaDropdownDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 1. Ensure this attribute is only used on Integers
        if (property.propertyType != SerializedPropertyType.Integer)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        // 2. Find the AreaManager in the current scene
        AreaManager areaManager = ServiceLocator.Get<AreaManager>();

        // 3. Fallback: If no AreaManager is found, just draw a normal integer field
        if (areaManager == null || areaManager._areas == null || areaManager._areas.Count == 0)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        // 4. Build a list of names from the AreaManager
        string[] areaNames = new string[areaManager._areas.Count];
        for (int i = 0; i < areaManager._areas.Count; i++)
        {
            var area = areaManager._areas[i];
            // Use the _locationName you defined, or a fallback name
            string name = (area != null && !string.IsNullOrEmpty(area._locationName)) ? area._locationName : $"Area {i}";
            
            // Format it nicely like "0: Main Menu"
            areaNames[i] = $"{i}: {name}"; 
        }

        // 5. Get the currently saved integer value
        int selectedIndex = property.intValue;
        
        // 6. Prevent errors if the index is out of bounds
        if (selectedIndex < 0 || selectedIndex >= areaNames.Length)
        {
            selectedIndex = 0; // Or whatever default is appropriate
        }

        // 7. Draw the actual dropdown in the Unity Inspector!
        EditorGUI.BeginProperty(position, label, property);
        property.intValue = EditorGUI.Popup(position, label.text, selectedIndex, areaNames);
        EditorGUI.EndProperty();
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Data;
using Attributes;

namespace Editor
{
    [CustomPropertyDrawer(typeof(TransitionVideoDropdownAttribute))]
    public class TransitionVideoDropdownDrawer : PropertyDrawer
    {
        private static string[] _optionsCache;
        private static int[] _valuesCache;
        private static double _lastRefreshTime;
        private const double RefreshInterval = 2.0;

        private void CheckCache()
        {
            if (_optionsCache == null || EditorApplication.timeSinceStartup - _lastRefreshTime > RefreshInterval)
            {
                RefreshCache();
            }
        }

        private void RefreshCache()
        {
            _lastRefreshTime = EditorApplication.timeSinceStartup;
            
            string[] guids = AssetDatabase.FindAssets("t:TransitionScriptableObject");
            
            if (guids.Length == 0)
            {
                _optionsCache = new string[0];
                _valuesCache = new int[0];
                return;
            }

            List<string> optionsList = new List<string>();
            List<int> valuesList = new List<int>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TransitionScriptableObject so = AssetDatabase.LoadAssetAtPath<TransitionScriptableObject>(path);

                if (so != null && so._transitionClips != null)
                {
                    for (int i = 0; i < so._transitionClips.Count; i++)
                    {
                        var clip = so._transitionClips[i];
                        string clipName = clip != null ? clip.name : "Empty";
                        
                        // Group by ScriptableObject name to keep things organized if there are multiple
                        optionsList.Add($"{so.name}/{clipName}");
                        valuesList.Add(i);
                    }
                }
            }

            _optionsCache = optionsList.ToArray();
            _valuesCache = valuesList.ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            CheckCache();

            if (_optionsCache == null || _optionsCache.Length == 0)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            int currentIndex = property.intValue;
            int selectedPopupIndex = 0;

            // Find which option is currently selected based on the saved integer value
            for (int i = 0; i < _valuesCache.Length; i++)
            {
                if (_valuesCache[i] == currentIndex)
                {
                    selectedPopupIndex = i;
                    break;
                }
            }

            // Draw the popup
            EditorGUI.BeginProperty(position, label, property);
            int newPopupIndex = EditorGUI.Popup(position, label.text, selectedPopupIndex, _optionsCache);
            
            // Save the actual integer value (the index of the video clip) back to the property
            if (newPopupIndex >= 0 && newPopupIndex < _valuesCache.Length)
            {
                property.intValue = _valuesCache[newPopupIndex];
            }
            
            EditorGUI.EndProperty();
        }
    }
}
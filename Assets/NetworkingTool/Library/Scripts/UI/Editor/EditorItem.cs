using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.NetworkingTool.Library.Scripts.UI
{
    //[CustomEditor(typeof(LobbyManager))]
    public class EditorItem : Editor
    {
        SerializedProperty allowSpectators, maxSpectators;


        void OnEnable()
        {
            allowSpectators = serializedObject.FindProperty("allowSpectators");
            maxSpectators = serializedObject.FindProperty("maxSpectators");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //    serializedObject.Update();
            //EditorGUILayout.PropertyField(allowSpectators);
            //    //EditorGUILayout.PropertyField(maxSpectators);
            //    serializedObject.ApplyModifiedProperties();
            if (!allowSpectators.boolValue) {
                maxSpectators.intValue = 0;
            }
            EditorGUILayout.LabelField("HOLA MUNDO: " + maxSpectators.intValue);
        }

        //public void OnSceneGUI()
        //{
        //    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        //}
    }
}

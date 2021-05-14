using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;


#if UNITY_EDITOR
[CustomEditor(typeof(StageEvent))]
public class StageEventInspector : Editor
{
    StageEvent obj;

    SerializedProperty Interval;
    SerializedProperty EventInstruction;
    SerializedProperty Paramaters;

    private void OnEnable()
    {

        obj = (StageEvent)target;

        if (target == null)
        {
            OnUnSelected();
            DestroyImmediate(this);
            return;
        }



        Interval = serializedObject.FindProperty("Interval");
        EventInstruction = serializedObject.FindProperty("m_events");
        Paramaters = serializedObject.FindProperty("");


        //Final function to call
        ApplyAll();
    }

    private void ApplyAll()
    {
        EditorUtility.SetDirty(obj);
        serializedObject.ApplyModifiedProperties();
    }

    private void OnUnSelected()
    {
        throw new NotImplementedException();
    }
}
#endif //UNITY_EDITOR

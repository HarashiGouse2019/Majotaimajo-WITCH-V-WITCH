using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(StageMap))]
public class StageMapEditor : Editor
{
    StageMap obj;
    static bool showStageMapEditor = false;
    SerializedProperty m_StageAnimation;
    SerializedProperty m_Description;
    SerializedProperty m_NativeEnemies;
    SerializedProperty m_Dialogue;
    SerializedProperty m_SequenceEvents;
    
    Vector2 scroll;
    Vector2 eventScroll;

    public void OnEnable()
    {
        obj = (StageMap)target;
        if(obj.mapName == string.Empty)
        {
            obj.mapName = "Untitled";
            
        }
        m_Description = serializedObject.FindProperty("description");
        m_StageAnimation = serializedObject.FindProperty("stageAnimation");
        m_NativeEnemies = serializedObject.FindProperty("nativeEnemies");
        m_Dialogue = serializedObject.FindProperty("dialogue");
        m_SequenceEvents = serializedObject.FindProperty("sequenceEvents");
    }
    
    public override void OnInspectorGUI()
    {
        //Map default information
        obj.mapName = EditorGUILayout.TextField("Name", obj.mapName);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        EditorGUILayout.PropertyField(m_Description, new GUIContent("Description"));
        EditorGUILayout.EndScrollView();

        EditorGUILayout.PropertyField(m_StageAnimation, new GUIContent("Stage Animation"));

        EditorGUILayout.PropertyField(m_NativeEnemies, new GUIContent("Native Enemies"), true);

        EditorGUILayout.PropertyField(m_Dialogue, new GUIContent("Dialogue"));

        //TODO: Add avaliable enemy prefabs

        //TODO: Sequence Events
        GUILayout.Label("SequenceEvents");
        eventScroll = EditorGUILayout.BeginScrollView(eventScroll);
        
        EditorGUILayout.PropertyField(m_SequenceEvents,  true);
        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif //UNITY_EDITOR
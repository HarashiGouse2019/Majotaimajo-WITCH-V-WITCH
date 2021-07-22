using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(NativeEnemy))]
public class NativeEnemyEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.indentLevel = 0;

        label = EditorGUI.BeginProperty(position, label, property);

        Rect contentPosition = EditorGUI.PrefixLabel(position, GUIContent.none);

        EditorGUI.ObjectField(contentPosition, property.FindPropertyRelative("pawn"), typeof(EnemyPawn), GUIContent.none);

        EditorGUI.EndProperty();
    }
}
#endif //UNITY_EDITOR
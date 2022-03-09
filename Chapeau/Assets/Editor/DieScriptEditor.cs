using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Die))]
public class DieScriptEditor : Editor
{
    SerializedProperty facesProp;
    private bool _toggleFaces = false;

    private void OnEnable()
    {
        facesProp = serializedObject.FindProperty("faces");
        if (!facesProp.isArray)
        {
            Debug.LogError("Faces in die is not an array");
        }
    }

    public override void OnInspectorGUI()
    {
        DrawPropertiesExcluding(serializedObject, "faces");

        serializedObject.Update();
        if (facesProp.arraySize == 0)
        {
            facesProp.arraySize = 6;
        }
        Die die = target as Die;

        _toggleFaces = EditorGUILayout.BeginFoldoutHeaderGroup(_toggleFaces, "Faces");
        if (_toggleFaces)
        {
            for(int i = 0; i < Die.s_directions.Length; i++)
            {
                EditorGUILayout.PropertyField(facesProp.GetArrayElementAtIndex(i), new GUIContent("Face of direction: " + Die.s_directions[i].ToString()));
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        serializedObject.ApplyModifiedProperties();
    }
}

using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UIElements;

namespace Seacore
{
    [CustomEditor(typeof(ImageDropdown), true)]
    [CanEditMultipleObjects]
    public class ImageDropdownEditor : SelectableEditor
    {
        private SerializedProperty m_Template;

        private SerializedProperty m_CaptionImage;

        private SerializedProperty m_ItemText;

        private SerializedProperty m_ItemImage;

        private SerializedProperty m_OnSelectionChanged;

        private SerializedProperty m_Value;

        private SerializedProperty m_Options;

        private SerializedProperty m_AlphaFadeSpeed;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Template = base.serializedObject.FindProperty("m_Template");
            m_CaptionImage = base.serializedObject.FindProperty("m_CaptionImage");
            m_ItemImage = base.serializedObject.FindProperty("m_ItemImage");
            m_OnSelectionChanged = base.serializedObject.FindProperty("m_OnValueChanged");
            m_Value = base.serializedObject.FindProperty("m_Value");
            m_Options = base.serializedObject.FindProperty("m_Options");
            m_AlphaFadeSpeed = base.serializedObject.FindProperty("m_AlphaFadeSpeed");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Template);
            EditorGUILayout.PropertyField(m_CaptionImage);
            EditorGUILayout.PropertyField(m_ItemImage);
            EditorGUILayout.PropertyField(m_Value);
            EditorGUILayout.PropertyField(m_AlphaFadeSpeed);
            EditorGUILayout.PropertyField(m_Options);
            EditorGUILayout.PropertyField(m_OnSelectionChanged);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

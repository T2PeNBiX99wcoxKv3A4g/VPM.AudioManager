using io.github.ykysnk.utils.Editor;
using UnityEditor;

namespace io.github.ykysnk.AudioManager.Editor;

[CustomEditor(typeof(SimpleAudioPlayer))]
public class SimpleAudioPlayerEditor : BasicEditor
{
    private const string AudioPrefabProp = "audioPrefab";
    private const string SourceCountProp = "sourceCount";

    private SerializedProperty? _audioPrefab;
    private SerializedProperty? _sourceCount;

    protected override void OnEnable()
    {
        _audioPrefab = serializedObject.FindProperty(AudioPrefabProp);
        _sourceCount = serializedObject.FindProperty(SourceCountProp);
    }

    protected override void OnInspectorGUIDraw()
    {
        EditorGUILayout.PropertyField(_audioPrefab);
        if (_audioPrefab?.objectReferenceValue == null)
            EditorGUILayout.HelpBox("The audio prefab is required.", MessageType.Error);
        EditorGUILayout.PropertyField(_sourceCount);
    }
}
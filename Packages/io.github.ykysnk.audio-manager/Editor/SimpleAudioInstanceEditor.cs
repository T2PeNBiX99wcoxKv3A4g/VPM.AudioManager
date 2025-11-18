using io.github.ykysnk.utils.Editor;
using UnityEditor;

namespace io.github.ykysnk.AudioManager.Editor;

[CustomEditor(typeof(SimpleAudioInstance))]
public class SimpleAudioInstanceEditor : BasicEditor
{
    protected override void OnInspectorGUIDraw()
    {
        EditorGUILayout.HelpBox("This component is use for handle audio source.", MessageType.Info);
    }
}
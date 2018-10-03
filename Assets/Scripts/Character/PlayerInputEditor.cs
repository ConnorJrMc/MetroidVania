using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerInput))]
public class PlayerInputEditor : Editor
{
    private bool m_IsPrefab = false;
    private bool m_IsNotInstance = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("Modify the prefab and not this instance", MessageType.Warning);
        if (GUILayout.Button("Select Prefab"))
        {
            Selection.activeObject = PrefabUtility.GetCorrespondingObjectFromSource(target);
        }
    }
}
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UserStudyScripts))]
public class UserStudyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        UserStudyScripts script = (UserStudyScripts)target;

        if (GUILayout.Button("Reset Everything"))
        {
            script.RestFunction();
        }
    }
}

using System;
using UnityEditor.Scripting.Python;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RunPython))]
public class PyRunnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RunPython script = (RunPython)target;

        if (GUILayout.Button("rerun scripts"))
        {
            runMain();
            runDistance();
        }
    }

    private static void RunPythonScript(string fileName)
    {
        Debug.unityLogger.Log(LogType.Warning, $"{Application.dataPath}/Scripts/PythonScripts/{fileName}");
        UnityEditor.Scripting.Python.Packages.PipPackages.AddPackage("bctpy");
        PythonRunner.RunFile($"{Application.dataPath}/Scripts/PythonScripts/{fileName}");
    }

    public void runMain()
    {
        RunPythonScript("main.py");
    }
    
    public void runDistance()
    {
        RunPythonScript("distance.py");
    }
}
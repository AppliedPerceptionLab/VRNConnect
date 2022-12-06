using System;
using UnityEditor.Scripting.Python;
using UnityEngine;
using UnityEditor;

public class PyRunnerEditor : Editor
{
    public static void RunPythonScript(string fileName)
    {
        Debug.unityLogger.Log(LogType.Warning, $"{Application.dataPath}/PythonScripts/{fileName}");
        PythonRunner.RunFile($"{Application.dataPath}/PythonScripts/{fileName}");
    }
}
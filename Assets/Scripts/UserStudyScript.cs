using System;
using UnityEngine;

public class UserStudyScript : MonoBehaviour
{
    public Boolean resetRigPosition = true;
    public void RestFunction()
    {
        Debug.unityLogger.Log(LogType.Warning, $"resetRigPosition = {resetRigPosition}");
        throw new NotImplementedException();
    }
    
    public void ThresholdFunction()
    {
        throw new NotImplementedException();
    }
    
    public void ScaleFunction()
    {
        throw new NotImplementedException();
    }

    public void RunUserStudy()
    {
        Debug.unityLogger.Log(LogType.Warning, $"RunUserStudy (with the following parameters):\n1.resetRigPosition = {resetRigPosition}");
        RestFunction();
        throw new NotImplementedException();
    }
}

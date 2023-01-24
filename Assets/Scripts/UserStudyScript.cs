using System;
using System.IO;
using System.Text;
using UnityEngine;

public class UserStudyScript : MonoBehaviour
{
    public Boolean resetRigPosition = true;
    public int UserID = 1;
    private int timerMilisec;
    private int mistakes;
    private UserStudyResults result;
    private string fileName;
    private bool task1Finish = false;
    private bool task2Finish = false;

    public void Start()
    {
        fileName = $"{Application.dataPath}/results.csv";
    }

    public void RestFunction()
    {
        Debug.unityLogger.Log(LogType.Warning, $"resetRigPosition = {resetRigPosition}");
        timerMilisec = 0;
        mistakes = 0;
        //throw new NotImplementedException();
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
        UserID += 1000;
        result = new UserStudyResults();
        result.UID = UserID;
        task1Finish = false;
        task2Finish = false;
        runTask1();
        runTask2();
    }

    private void runTask1()
    {
        RestFunction();
        //TODO write the task here
        result.TimeMilisecTask1 = timerMilisec;
        result.MistakesTask1 = mistakes;
        task1Finish = true;
    }

    private void runTask2()
    {
        RestFunction();
        //TODO write the task here
        result.TimeMilisecTask2 = timerMilisec;
        result.MistakesTask2 = mistakes;
        task2Finish = true;
    }

    public void WriteCSV()
    {
        if (allTasksFinished())
        {
            TextWriter tw;
            if (UserID == 1000)
            {
                tw = new StreamWriter(fileName, false);
                tw.WriteLine("UID, TimeMilisecTask1, MistakesTask1, TimeMilisecTask2, MistakesTask2, StartByHand, StartByController");
                tw.Close();
            }
            else
            {
                tw = new StreamWriter(fileName, true);
                StringBuilder str = new StringBuilder();
                str.Append(result.UID).Append(",")
                    .Append(result.TimeMilisecTask1).Append(",")
                    .Append(result.TimeMilisecTask2).Append(",")
                    .Append(result.MistakesTask1).Append(",")
                    .Append(result.MistakesTask2).Append(",")
                    .Append(result.StartByHand).Append(",")
                    .Append(result.StartByController);
                tw.WriteLine(str.ToString());
                tw.Close();   
            }
        }
    }

    private bool allTasksFinished()
    {

        if (!task1Finish || !task2Finish)
        {
            Debug.unityLogger.Log(LogType.Warning, $"WriteCSV Error task1Finish = {task1Finish}, task2Finish = {task2Finish}");
            return false;
        }

        return true;
    }
}

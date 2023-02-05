using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Debug = UnityEngine.Debug;

public class UserStudyScript : MonoBehaviour
{
    public GameObject Scaler;
    public GameObject MyRig;
    public GameObject Thresholder;
    public GameObject BrainParent;

    public int UserID = 1;

    public Boolean resetRigPosition = true;
    public Boolean resetThreshold = false;
    public Boolean resetScale = false;
    public Boolean resetBrainRotation = false;

    private long timerMilisec;
    private int NumberOfClicks;
    private int prevNumberOfClicks;
    private UserStudyResults result = new UserStudyResults();
    private string fileName;
    private bool continueClicked = false;
    private bool runUserStudy = false;
    private static List<Task> tasks = new List<Task>();
    private int totalTaskIndex = 0;
    private Stopwatch stopwatch;


    public void Start()
    {
        fileName = $"{Application.dataPath}/results.csv";
        GameObject.Find("Finish").GetComponent<Button>().interactable = false;
        GameObject.Find("Next").SetActive(true);
        stopwatch = new Stopwatch();
    }

    public void Update()
    {
        
    }

    private bool taksHandlerNext()
    {
        BrainParent.GetComponentInChildren<ReadCSV>().setUserStudyMode(true);
        prevNumberOfClicks = NumberOfClicks;
        RestFunction(true);
        if (totalTaskIndex == 0)
        {
            stopwatch.Start();
            BrainParent.GetComponentInChildren<ReadCSV>().colorizeNode(tasks[totalTaskIndex].node);
            HintTooltipUI.ShowTooltip_Static(tasks[totalTaskIndex].GetTooltipText);
            totalTaskIndex++;
        }
        else if (totalTaskIndex < tasks.Count)
        {
            stopwatch.Stop();
            timerMilisec = stopwatch.ElapsedMilliseconds;
            Debug.Log("Timer: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            tasks[totalTaskIndex - 1].taskFinish = true;
            tasks[totalTaskIndex - 1].taskTime = timerMilisec;
            tasks[totalTaskIndex - 1].taskClicks = prevNumberOfClicks;
            
            stopwatch.Start();
            BrainParent.GetComponentInChildren<ReadCSV>().colorizeNode(tasks[totalTaskIndex].node);
            HintTooltipUI.ShowTooltip_Static(tasks[totalTaskIndex].GetTooltipText);
            totalTaskIndex++;
        } else
        {
            stopwatch.Stop();
            timerMilisec = stopwatch.ElapsedMilliseconds;
            Debug.Log("Timer: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            tasks[totalTaskIndex - 1].taskFinish = true;
            tasks[totalTaskIndex - 1].taskTime = timerMilisec;
            tasks[totalTaskIndex - 1].taskClicks = prevNumberOfClicks;
            
            result.Tasks = tasks;
            totalTaskIndex++;
            return false;
        }
        
        return true;
    }

    public void RestFunction(bool isHardReset = false)
    {
        if (isHardReset)
        {
            timerMilisec = 0;
            NumberOfClicks = 0;
            continueClicked = false;
            MyRig.GetComponent<ResetRig>().ResetTransform();
            Scaler.GetComponent<Scaler>().OnReset();
            Thresholder.GetComponent<Thresholder>().OnReset();
            BrainParent.GetComponent<BrainRotator>().OnReset();
        }
        else
        {
            timerMilisec = 0;
            NumberOfClicks = 0;
            continueClicked = false;
            if (resetRigPosition)
            {
                MyRig.GetComponent<ResetRig>().ResetTransform();
            }

            if (resetScale)
            {
                Scaler.GetComponent<Scaler>().OnReset();
            }

            if (resetThreshold)
            {
                Thresholder.GetComponent<Thresholder>().OnReset();
            }

            if (resetBrainRotation)
            {
                BrainParent.GetComponent<BrainRotator>().OnReset();
            }
        }
        
    }

    public void RunUserStudy()
    {
        RestFunction(true);
        UserID += 1000;
        result = new UserStudyResults();
        result.UID = UserID;
        totalTaskIndex = 0;
        runUserStudy = true;
        
        tasks = createUserTasks(UserID%2==0);
        taksHandlerNext();
    }

    private List<Task> createUserTasks(bool shouldStartWithHand)
    {
        result.StartByHand = shouldStartWithHand;
        result.StartByController = !shouldStartWithHand;
        List<Task> t = new List<Task>();
        for (int i = 1; i <= 2; i++)
        {
            Task myTask;
            switch (i)
            {
                case 1:
                    for (int j = 0; j < 6; j++)
                    {
                        myTask = new Task();
                        myTask.taskType = i;
                        if (j<3)
                        {
                            myTask.interactionMethod = shouldStartWithHand? "_hand" : "_controller";
                        }
                        else
                        {
                            myTask.interactionMethod = shouldStartWithHand? "_controller" : "_hand";
                        }
                        switch (j % 3)
                        {
                            case 0 :
                                myTask.node = new [] { "6r_R" };
                                myTask.detailToRead = "Lobe";
                                break;
                            case 1:
                                myTask.node = new [] { "3a_L" };
                                myTask.detailToRead = "Degree";
                                break;
                            case 2:
                                myTask.node = new [] { "V2_R" };
                                myTask.detailToRead = "Clustering Coefficient";
                                break;
                        }
                        t.Add(myTask);
                    }
                    break;
                case 2:
                    for (int j = 0; j < 6; j++)
                    {
                        myTask = new Task();
                        myTask.taskType = i;
                        if (j<3)
                        {
                            myTask.interactionMethod = shouldStartWithHand? "_hand" : "_controller";
                        }
                        else
                        {
                            myTask.interactionMethod = shouldStartWithHand? "_controller" : "_hand";
                        }
                        switch (j % 3)
                        {
                            case 0 :
                                myTask.node = new [] { "p24_R","TA2_R" };
                                break;
                            case 1:
                                myTask.node = new [] { "MBelt_L","ProS_L" };
                                break;
                            case 2:
                                myTask.node = new [] { "LO1_R","V1_R" };
                                break;
                        }
                        myTask.detailToRead = "Count the Number of Connections";
                        t.Add(myTask);
                    }
                    break;
            }
        }

        return t;
    }

    public void WriteCSV(bool forced = false)
    {
        if (allTasksFinished() || forced)
        {
            TextWriter tw;
            if (UserID == 0)
            {
                tw = new StreamWriter(fileName, false);
                tw.WriteLine(result.getTitle());
                tw.Close();
            }
            else
            {
                tw = new StreamWriter(fileName, true);
                tw.WriteLine(result.getString());
                tw.Close();
            }
        }
    }

    private bool allTasksFinished()
    {
        bool e = true;
        foreach (var t in tasks)
        {
            if (!t.taskFinish)
            {
                e = false;
            }
        }

        if (e) return true;
        Debug.unityLogger.Log(LogType.Warning,
            $"WriteCSV Error tasks not finished");
        return false;

    }

    public void onNextClicked()
    {
        continueClicked = true;
        if (runUserStudy)
        {
            if (continueClicked)
            {
                //do something if user does click continue
                if (!taksHandlerNext())
                {
                    runUserStudy = false;
                    totalTaskIndex = 0;
                    GameObject.Find("Finish").GetComponent<Button>().interactable = true;
                    GameObject.Find("Next").GetComponent<Button>().interactable = false;
                    WriteCSV();
                }
            }
        }
    }

    public void onFinishClicked()
    {
        runUserStudy = false;
    }

    public void getUserClicks()
    {
        NumberOfClicks++;
    }
}
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UserStudyResults
{
    private bool hand1 = false;
    private bool hand2 = false;
    private bool controller1 = false;
    private bool controller2 = false;
    public UserStudyResults()
    {
        UID = 1000;
        StartByHand = false;
        StartByController = false;
    }

    public int UID { get; set; }
    public bool StartByHand { get; set; }
    public bool StartByController { get; set; }
    public List<Task> Tasks { get; set; }

    public string getTitle()
    {
        return
            "UID, taskType, interactionMethod, taskTime_avg, taskClicks_avg, " +
            "taskType, interactionMethod, taskTime_avg, taskClicks_avg, " +
            "taskType, interactionMethod, taskTime_avg, taskClicks_avg, " +
            "taskType, interactionMethod, taskTime_avg, taskClicks_avg, " +
            "StartByHand, StartByController";
    }


    public string getString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(UID).Append(",");
        foreach (var t in Tasks)
        {
            if (t.taskType == 1)
            {
                if (t.interactionMethod.Equals("_hand") && !hand1)
                {
                    str.Append($"Node Selection").Append(",")
                        .Append($"{t.interactionMethod}").Append(",")
                        .Append($"{calcAVGTime(t.taskType,t.interactionMethod)}").Append(",")
                        .Append($"{calcAVGClicks(t.taskType,t.interactionMethod)}").Append(",");
                    hand1 = true;
                }else if (t.interactionMethod.Equals("_controller") && !controller1)
                {
                    str.Append($"Node Selection").Append(",")
                        .Append($"{t.interactionMethod}").Append(",")
                        .Append($"{calcAVGTime(t.taskType,t.interactionMethod)}").Append(",")
                        .Append($"{calcAVGClicks(t.taskType,t.interactionMethod)}").Append(",");
                    controller1 = true;
                }

            } else if (t.taskType == 2)
            {
                if (t.interactionMethod.Equals("_hand") && !hand2)
                {
                    str.Append($"Shortest Path").Append(",")
                        .Append($"{t.interactionMethod}").Append(",")
                        .Append($"{calcAVGTime(t.taskType,t.interactionMethod)}").Append(",")
                        .Append($"{calcAVGClicks(t.taskType,t.interactionMethod)}").Append(",");
                    hand2 = true;
                } else if (t.interactionMethod.Equals("_controller") && !controller2)
                {
                    str.Append($"Shortest Path").Append(",")
                        .Append($"{t.interactionMethod}").Append(",")
                        .Append($"{calcAVGTime(t.taskType,t.interactionMethod)}").Append(",")
                        .Append($"{calcAVGClicks(t.taskType,t.interactionMethod)}").Append(",");
                    controller2 = true;
                }
            }
        }
        str.Append(StartByHand).Append(",")
            .Append(StartByController);
        return str.ToString();
    }

    private long calcAVGTime(int tTaskType, string tInteractionMethod)
    {
        long time = 0;
        foreach (var t in Tasks)
        {
            if (t.taskType == tTaskType && t.interactionMethod == tInteractionMethod)
            {
                time += t.taskTime;
            }
            
        }
        return time / 3;
    }
    
    private int calcAVGClicks(int tTaskType, string tInteractionMethod)
    {
        int click = 0;
        foreach (var t in Tasks)
        {
            if (t.taskType == tTaskType && t.interactionMethod == tInteractionMethod)
            {
                // if (tTaskType == 1)
                // {
                //     click += t.taskClicks - 2;
                // } 
                // else if (tTaskType ==2)
                // {
                //     click += t.taskClicks - 3;
                // }
                click += t.taskClicks;
            }
            
        }
        return click / 3;
    }
}
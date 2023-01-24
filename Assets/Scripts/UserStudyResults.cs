using UnityEngine;

public class UserStudyResults
{

    public UserStudyResults()
    {
        UID = 1000;
        TimeMilisecTask1 = 0;
        MistakesTask1 = 0;
        TimeMilisecTask2 = 0;
        MistakesTask2 = 0;
        StartByHand = false;
        StartByController = false;
    }
    public int UID { get; set; }
    public bool StartByHand   { get; set; }
    public bool StartByController   { get; set; }
    public int TimeMilisecTask1 { get; set; }
    public int MistakesTask1  { get; set; }
    public int TimeMilisecTask2 { get; set; }
    public int MistakesTask2  { get; set; }
}
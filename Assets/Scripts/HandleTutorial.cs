using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject next;
    public GameObject prev;
    private Button nextbtn;
    private Button prevbtn;
    private List<string> tutorialList;
    private int index = 0;
    void Start()
    {
        nextbtn = next.GetComponent<Button>();
        prevbtn = prev.GetComponent<Button>();

        CreateTutorialList();
    }

    private void CreateTutorialList()
    {
        nextbtn.interactable = false;
        prevbtn.interactable = false;
        index = 0;
        tutorialList = new List<string>();
        string guide = "test1";
        tutorialList.Add(guide);
        guide = "test2";
        tutorialList.Add(guide);
        guide = "test3";
        tutorialList.Add(guide);
        guide = "test4";
        tutorialList.Add(guide);
        guide = "test5";
        tutorialList.Add(guide);
        guide = "test6";
        tutorialList.Add(guide);
        showHint(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onNextClicked()
    {
        showHint(1);
    }

    public void onPrevClicked()
    {
        showHint(-1);
    }
    
    private void showHint(int offset)
    {
        index += offset;
        if (index == 0)
        {
            nextbtn.interactable = true;
            prevbtn.interactable = false;
        }
        else if (index == tutorialList.Count -1)
        {
            nextbtn.interactable = false;
            prevbtn.interactable = true;
        }
        else
        {
            nextbtn.interactable = true;
            prevbtn.interactable = true;
        }
        HintTooltipUI.ShowTooltip_Static(tutorialList[index]);
    }

    public void onReset()
    {
        CreateTutorialList();
    }
}

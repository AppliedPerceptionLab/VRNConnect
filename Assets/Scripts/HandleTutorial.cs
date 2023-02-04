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
        tutorialList = new Tutorial().steps;
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
        HintTooltipUI.ShowTooltip_Static(tutorialList[index] + $"\n({index+1}/{tutorialList.Count})");
    }

    public void onReset()
    {
        if (index == tutorialList.Count - 1)
        {
            CreateTutorialList();
        }
    }
}

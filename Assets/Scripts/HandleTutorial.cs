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
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    public void onReset()
    {
        CreateTutorialList();
    }
}

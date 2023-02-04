using System.Collections.Generic;

public class Tutorial
{
    public List<string> steps { get; private set; }
    
    public Tutorial()
    {
        steps = new List<string>();
        steps.Add("Welcome to the tutorial.\nBefore we go further try finding the trigger button on the controller.\nThen click on next.");
        steps.Add("You will learn about the controls and application functionality in the next few steps.");
        steps.Add("During each step, read the information on your right hand.\nThen click on the next/prev button when needed.");
        steps.Add("In your front right,\nYou can see the whole brain connectome in the form of a graph.");
        steps.Add("The colour of the nodes/edges corresponds to the brain atlas region.");
        steps.Add("You can interact with different elements in the application using your controller.");
        steps.Add("The trigger button is used for selection/deselection.\nTry selecting a node.");
        steps.Add("When you select a node,\nyou see information regarding that node on your left hand.\nTry it.");
        steps.Add("You can also move around/rotate \nusing the 2 axis buttons on top of the controller.\nTry it.");
        steps.Add("Oh wait, you don’t need to stand still.\nYou can also walk around the scene without the controller.\nTry.");
        steps.Add("You can also rotate the brain \nusing the a/b or x/y buttons on the controller.\nTry it.");
        steps.Add("On your left, you can see an interface which shows different settings.");
        steps.Add("Try changing some of the settings and see the effect on the brain.");
        steps.Add("You can also interact with your brain using your hands, \nfirst, you need to reset your field of view with the red reset button on top of the settings,\nthen put away your controllers.");
        steps.Add("When using your hands,\nyou can select/deselect by pitching with your thumb and index/middle finger.\nTry it.");
        steps.Add("Pinching might be easier when you are close to the subject.");
        steps.Add("Last but not least, if you select 2 nodes,\nyou can see the shortest path between them.\nTry it.");
        steps.Add("Congrats, you are now ready to use the app.");
    }
}
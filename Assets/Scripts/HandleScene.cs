using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleScene : MonoBehaviour
{
    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;
    // Start is called before the first frame update
    void Start()
    {
        // myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/scenes");
        // scenePaths = myLoadedAssetBundle.GetAllScenePaths();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openTutorial()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        // Debug.unityLogger.Log("Scene loading: " + scenePaths[2]);
        SceneManager.LoadScene("Scenes/TutorialScene", LoadSceneMode.Single);
    }

    public void openMainScene()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        // Debug.unityLogger.Log("Scene loading: " + scenePaths[1]);
        SceneManager.LoadScene("Scenes/NewScene", LoadSceneMode.Single);
    }
}

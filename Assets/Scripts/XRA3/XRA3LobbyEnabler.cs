using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XRA3LobbyEnabler : MonoBehaviour
{
    public string lobbyScene;
    public GameObject levelShapesObject;

    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.sceneLoaded += EnableLevelShapes;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= EnableLevelShapes;
    }

    public void EnableLevelShapes(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == lobbyScene)
        {
            levelShapesObject.SetActive(true);
        }
    }
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTrigger : MonoBehaviour
{
    // Name of Scene inside of "Scenes" folder
    [SerializeField] string scene;
    
    // final solution will propably be +1 index of scene
    // with scenes sorted in correct play order

    public void ChangeScene()
    {
        scene = "Scenes/" + scene;
        SceneManager.LoadScene(scene);
    }
}

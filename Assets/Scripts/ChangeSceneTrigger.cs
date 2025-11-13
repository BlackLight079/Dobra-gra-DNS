using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTrigger : MonoBehaviour
{
    // Name of Scene inside of "Scenes" folder
    [SerializeField] string scene;

    public void ChangeScene()
    {
        scene = "Scenes/" + scene;
        SceneManager.LoadScene(scene);
    }
}

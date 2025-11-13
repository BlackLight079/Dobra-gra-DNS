using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTrigger : MonoBehaviour
{
    [SerializeField] string scenePath;

    public void ChangeScene()
    {
        SceneManager.LoadScene(scenePath);
    }
}

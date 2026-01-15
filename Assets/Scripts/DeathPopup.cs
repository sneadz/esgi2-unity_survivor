using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPopup : MonoBehaviour
{
    public void Retry()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}

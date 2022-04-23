using UnityEngine;
using UnityEngine.SceneManagement;

public class Context : MonoBehaviour
{
    public static Context instance { get; private set; }

    private GenericPlayer localPlayer;
    private GenericPlayer opponentPlayer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

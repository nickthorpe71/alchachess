using UnityEngine.SceneManagement;

public static class GlobalSceneLoader
{
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
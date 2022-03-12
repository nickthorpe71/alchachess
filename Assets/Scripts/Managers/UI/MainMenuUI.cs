using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject[] menus;

    public void StartGame(int difficulty)
    {
        Debug.Log(difficulty);
        SceneManager.LoadScene(1);
    }

    public void OpenMenu(int indexToOpen)
    {
        CloseMenus();
        menus[indexToOpen].SetActive(true);
    }

    private void CloseMenus()
    {
        for (int i = 0; i < menus.Length; i++)
            menus[i].SetActive(false);
    }
}

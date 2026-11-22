using UnityEngine;
using UnityEngine.SceneManagement;

public class MM_Script : MonoBehaviour
{
    GameObject mainMenu;
    HTP_Script howToPlayMenu;

    void Awake()
    {
        mainMenu = transform.gameObject;
        howToPlayMenu = FindAnyObjectByType<HTP_Script>();

        mainMenu.SetActive(true);
    }

    public void ShowHowToPlayMenu()
    {
        mainMenu.SetActive(false);

        howToPlayMenu.ShowHowToPlayMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

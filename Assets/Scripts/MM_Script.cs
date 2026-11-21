using UnityEngine;
using UnityEngine.SceneManagement;

public class MM_Script : MonoBehaviour
{

    GameObject mainMenu;
    GameObject howToPlayBtn;
    GameObject startBtn;
    GameObject exitBtn;
    GameObject howToPlayMenu;

    void Awake()
    {
        mainMenu = transform.gameObject;
        GameObject grid = transform.Find("Grid").gameObject;
        startBtn = grid.transform.Find("StartButton").gameObject;
        exitBtn = grid.transform.Find("ExitButton").gameObject;
        howToPlayBtn = grid.transform.Find("HowToPlayButton").gameObject;

        howToPlayMenu = GameObject.Find("HowToPlayMenu");


        mainMenu.SetActive(true);
        howToPlayMenu.SetActive(false);
    }

    public void showHowToPlayMenu()
    {
        mainMenu.SetActive(false);
        howToPlayMenu.SetActive(true);
    }

    public void startGame()
    {
        SceneManager.LoadScene("Escena de joc");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    
}

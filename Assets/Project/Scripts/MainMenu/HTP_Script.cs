using UnityEngine;

public class HTP_Script : MonoBehaviour
{
    GameObject mainMenu;
    GameObject howToPlayMenuUI;
    void Awake()
    {
        mainMenu = GameObject.Find("MainMenu");

        howToPlayMenuUI = transform.Find("HowToPlayMenuUI").gameObject;

        howToPlayMenuUI.SetActive(false);
    }

    public void ShowHowToPlayMenu()
    {
        mainMenu.SetActive(false);
        howToPlayMenuUI.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        howToPlayMenuUI.SetActive(false);
    }
}

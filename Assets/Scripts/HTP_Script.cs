using UnityEngine;

public class HTP_Script : MonoBehaviour
{
    GameObject mainMenu;
    GameObject howToPlayMenu;
    GameObject goBackBtn;
    void Awake()
    {
        mainMenu = GameObject.Find("MainMenu");

        howToPlayMenu = transform.gameObject;
        goBackBtn = transform.Find("GoBackButton").gameObject;
    }

    public void shwoMainMenu()
    {
        mainMenu.SetActive(true);
        howToPlayMenu.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    private GameObject endgameUI;

    private void Awake()
    {
        endgameUI = transform.Find("EndGameUI").gameObject;

        endgameUI.SetActive(false);
    }

    public void OpenPanel()
    {
        endgameUI.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

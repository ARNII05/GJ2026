using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    private GameObject endgameUI;

    private GameObject separators;

    private void Awake()
    {
        endgameUI = transform.Find("EndGameUI").gameObject;
        separators = GameObject.Find("Separators");

        endgameUI.SetActive(false);
    }

    public void OpenPanel()
    {
        endgameUI.SetActive(true);
        separators.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

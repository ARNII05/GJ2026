using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    private GameObject winUI;

    private void Awake()
    {
        winUI = transform.Find("WinUI").gameObject;
            
        winUI.SetActive(false);
    }

    public void OpenPanel(float salary)
    {
        winUI.SetActive(true);

        TMP_Text salaryText = winUI.transform.Find("CheckImg").Find("SalaryText").GetComponent<TMP_Text>();

        salaryText.text = $"{(int)salary}";
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

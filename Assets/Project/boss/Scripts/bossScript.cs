
using UnityEngine;
using UnityEngine.SceneManagement;

public class bossScript : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(CambiarEscena), 5f);
    }

    void CambiarEscena()
    {
        SceneManager.LoadScene(2);
    }
}

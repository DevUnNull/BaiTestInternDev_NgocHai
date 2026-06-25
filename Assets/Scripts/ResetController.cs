using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetController : MonoBehaviour
{
    public Button resetButton;

    private void Start()
    {
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ReloadScene);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

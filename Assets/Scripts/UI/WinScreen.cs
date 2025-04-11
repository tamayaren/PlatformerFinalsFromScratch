using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject ui;
    
    private void Start()
    {
        GameManager.instance.GameWin.AddListener(() => this.ui.SetActive(true));
        
        this.button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject ui;
    
    private Transform target;
    private Humanoid humanoid;
    
    private void Start()
    {
        this.target = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (this.target == null) return;
        
        this.humanoid = this.target.GetComponent<Humanoid>();
        this.humanoid.OnDied.AddListener(() => this.ui.SetActive(true));
        
        this.button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }
}

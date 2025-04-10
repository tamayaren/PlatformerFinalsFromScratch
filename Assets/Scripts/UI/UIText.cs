using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    [SerializeField] private Humanoid playerHumanoid;
    
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI timerText;
    
    private void UpdateHealthText(float _)
    {
        this.healthText.text = $"HP: {this.playerHumanoid.health}/{this.playerHumanoid.maxHealth}";
    }

    private void UpdateTimerText(int _)
    {
        this.timerText.text = $"TIMER: {GameManager.instance.timer}";
    }

    private void Start()
    {
        this.playerHumanoid.OnHealthChanged.AddListener(UpdateHealthText);
        this.playerHumanoid.OnMaxHealthChanged.AddListener(UpdateHealthText);
        
        UpdateHealthText(this.playerHumanoid.health);
        //
        GameManager.instance.OnTimerChanged.AddListener(UpdateTimerText);
        UpdateTimerText(GameManager.instance.timer);
    }
}

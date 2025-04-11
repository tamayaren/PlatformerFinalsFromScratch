using Mono.Cecil;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int timer = 600;

    private void Awake() => instance = this;

    public UnityEvent<int> OnTimerChanged;
    public UnityEvent OnTimerEnded;

    public bool timerEnded = false;
    private float elapsed = 0f;

    public int score = 0;
    public UnityEvent<int> OnScoreChanged;
    public int maxScore = 10;
    
    public UnityEvent GameWin;
    
    private void Update()
    {
        if (this.timerEnded) return;
        if (this.timer <= 0f)
        {
            this.OnTimerEnded.Invoke();
            this.timerEnded = true;
            return;
        }
        
        this.elapsed += Time.deltaTime;

        if (this.elapsed >= 1f)
        {
            this.elapsed = 0f;
            
            this.timer -= 1;
            this.OnTimerChanged.Invoke(this.timer);
        }
    }

    public void IncrementScore(int add)
    {
        this.score += add;
        if (this.score >= this.maxScore)
            this.GameWin.Invoke();
        
        this.OnScoreChanged.Invoke(this.score);
    }
}

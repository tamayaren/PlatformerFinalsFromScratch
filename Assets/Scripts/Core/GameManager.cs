using Mono.Cecil;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int timer = 600;

    private void Start() => instance = this;

    public UnityEvent<int> OnTimerChanged;
    public UnityEvent OnTimerEnded;

    public bool timerEnded = false;
    private float elapsed = 0f;
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
}

using UnityEngine;
using UnityEngine.Events;

public class Humanoid : MonoBehaviour
{
    public float health
    {
        get => this._health;
        set
        {
            this._health = Mathf.Clamp(value, 0f, this._maxHealth);
            
            this.OnHealthChanged.Invoke(this._health);
        }
    }

    private float _health;

    public float maxHealth
    {
        get => this._maxHealth;
        set
        {
            this._maxHealth = Mathf.Max(1, value);
            
            this.OnMaxHealthChanged.Invoke(this._maxHealth);
        }
    }
    private float _maxHealth;
    
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent<float> OnMaxHealthChanged;
}

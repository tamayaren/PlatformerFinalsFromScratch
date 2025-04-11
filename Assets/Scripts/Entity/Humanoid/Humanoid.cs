using UnityEngine;
using UnityEngine.Events;

public enum HumanoidStateType
{
    Alive,
    Dead
}

public class Humanoid : MonoBehaviour
{
    public int health
    {
        get => this._health;
        set
        {
            this._health = Mathf.Clamp(value, 0, this._maxHealth);

            this.stateType = this._health > 0 ? HumanoidStateType.Alive : HumanoidStateType.Dead;
            if (this._health <= 0)
                this.OnDied.Invoke();
            
            this.OnHealthChanged.Invoke(this._health);
        }
    }

    private int _health;

    public int maxHealth
    {
        get => this._maxHealth;
        set
        {
            this._maxHealth = Mathf.Max(1, value);
            
            this.OnMaxHealthChanged.Invoke(this._maxHealth);
        }
    }
    private int _maxHealth;
    public bool isInvincible = false;
    
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent<float> OnMaxHealthChanged;
    public UnityEvent OnDied;
    
    public HumanoidStateType stateType = HumanoidStateType.Alive;

    public void Damage(int damage)
    {
        if (this.isInvincible) return;
        
        this.health -= damage;
    }

    public void Kill()
    {
        this.health = 0;
        this.OnDied.Invoke();
    }
}

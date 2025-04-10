using UnityEngine;

public class HumanoidDefiner : MonoBehaviour
{
    private Humanoid humanoid;
    public float startingMaxHealth = 10f;
    
    private void Start()
    {
        this.humanoid = GetComponent<Humanoid>();
        
        this.humanoid.maxHealth = this.startingMaxHealth;
        this.humanoid.health = this.humanoid.maxHealth;
    }
}

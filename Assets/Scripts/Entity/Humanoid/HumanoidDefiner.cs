using UnityEngine;

public class HumanoidDefiner : MonoBehaviour
{
    private Humanoid humanoid;
    public int startingMaxHealth = 10;
    
    private void Start()
    {
        this.humanoid = GetComponent<Humanoid>();
        
        this.humanoid.maxHealth = this.startingMaxHealth;
        this.humanoid.health = this.humanoid.maxHealth;
    }
}

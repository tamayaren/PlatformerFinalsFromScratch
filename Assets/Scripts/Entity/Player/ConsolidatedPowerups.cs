using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConsolidatedPowerups : MonoBehaviour
{
    public static ConsolidatedPowerups instance;
    private Humanoid humanoid;
    private Object3D object3D;

    //
    public bool fireballPowerup = false;
    //
    public bool marioStarPowerup = false;
    public float marioStarDuration = 10f;
    public float marioStarElapsed = 0f;

    public UnityEvent<string> OnPowerUp;
    private void Awake() => instance = this;

    private void Start()
    {
        this.humanoid = GetComponent<Humanoid>();
        this.object3D = GetComponent<Object3D>();
    }

    private IEnumerator MarioStar()
    {
        this.marioStarPowerup = true;
        yield return new WaitForSeconds(this.marioStarDuration);
        this.marioStarPowerup = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.fireballPowerup)
        {
            
            
            this.fireballPowerup = false;
        }

        if (this.marioStarPowerup)
        {
            CollisionManager.Instance.CheckCollision(this.object3D.collisionId, this.transform.position, out _, out List<GameObject> enemies);

            if (enemies != null && enemies.Count > 0)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.tag == "Enemy")
                    {
                        bool isHumanoid = enemy.TryGetComponent(out Humanoid humanoid);
                        if (!isHumanoid) continue;
                        if (humanoid.stateType == HumanoidStateType.Dead) continue;
                        
                        humanoid.Kill();
                    }
                }
            }
        }
    }

    public void StartFireballPowerup()
    {
        this.fireballPowerup = true;
        this.OnPowerUp.Invoke("Fireball");
    }
    public void StartExtraLifePowerup()
    {
        this.humanoid.health = this.humanoid.maxHealth;
        this.OnPowerUp.Invoke("Extra Life");
    }

    public void StartMarioStarPowerup()
    {
        StartCoroutine(MarioStar());
        this.OnPowerUp.Invoke("Mario Star");
    }
}

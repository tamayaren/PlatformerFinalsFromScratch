using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ConsolidatedPowerups : MonoBehaviour
{
    public static ConsolidatedPowerups instance;

    [SerializeField] private GameObject fireballPrefab;
    private Humanoid humanoid;
    private Object3D object3D;
    
    private ParticleSystem particleSystem;

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
        this.particleSystem = GetComponent<ParticleSystem>();
    }

    private IEnumerator MarioStar()
    {
        this.marioStarPowerup = true;
        this.particleSystem.Play();
        yield return new WaitForSeconds(this.marioStarDuration);
        this.marioStarPowerup = false;
        this.particleSystem.Stop();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && this.fireballPowerup)
        {
            this.fireballPowerup = false;
            GameObject fireball = Instantiate(this.fireballPrefab, this.transform.position, Quaternion.identity) as GameObject;

            GameObject target = GameObject.FindGameObjectsWithTag("Enemy").
                    OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).FirstOrDefault();

            fireball.transform.LookAt(target.transform.position);
            bool isExist = fireball.TryGetComponent(out FireballProjectile fireballProjectile);
            if (!isExist)
            {
                Destroy(fireball);
                return;
            }

            fireballProjectile.StartProjectile();
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

    public void GetRandomizePowerUp(int random)
    {
        switch (random)
        {
            case 0:
                StartFireballPowerup();
                break;
            case 1:
                StartExtraLifePowerup();
                break;
            case 2:
                StartMarioStarPowerup();
                break;
        }
    }
}

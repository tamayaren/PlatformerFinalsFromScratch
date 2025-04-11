using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private PhysicsBody physicsBody;
    private Object3D object3D;

    private Humanoid humanoid;

    private Transform target;
    private Object3D targetObject3D;
    
    private bool hitDebounce = false;
    private float hitCooldown = 3f;
    
    [SerializeField] private float movementSpeed = 5f;
    
    private void Awake()
    {
        this.physicsBody = GetComponent<PhysicsBody>();
        this.object3D = GetComponent<Object3D>();
        this.humanoid = GetComponent<Humanoid>();
    }

    private void Start()
    {
        this.target = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        if (this.target) 
            this.targetObject3D = this.target.GetComponent<Object3D>();
    }

    private IEnumerator StartHitCooldown()
    {
        this.hitDebounce = true;
        yield return new WaitForSeconds(this.hitCooldown);
        this.hitDebounce = false;
    }

    private void Attack(Object3D object3D)
    {
        bool isHumanoid = object3D.TryGetComponent(out Humanoid humanoid);
        if (!isHumanoid) return;
        
        humanoid.Damage(-1);
    }

    private void Update()
    {
        if (!this.target) return;
        
        Vector3 direction = (this.target.position - this.transform.position).normalized;
        Vector3 newPosition = this.transform.position + direction * (this.movementSpeed * Time.deltaTime);
        
        bool isCollision = CollisionManager.Instance.CheckCollision(this.object3D.collisionId, newPosition + new Vector3(0, .1f, 0), out List<int> collisions, out _);
        if (isCollision && this.physicsBody.isGrounded == PhysicsObjectState.Grounded)
        {
            this.physicsBody.velocity.y += 5f;

            if (!this.hitDebounce)
            {
                bool isHit = collisions.Contains(this.targetObject3D.collisionId);
                
                if (isHit)
                    Attack(this.targetObject3D);

                this.hitDebounce = true;
                StartCoroutine(StartHitCooldown());
            }
        }
        
        this.physicsBody.velocity.x = (direction.x * this.movementSpeed);
        this.physicsBody.velocity.z = (direction.z * this.movementSpeed);
    }
}

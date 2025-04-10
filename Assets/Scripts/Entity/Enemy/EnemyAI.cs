using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private PhysicsBody physicsBody;
    private Object3D object3D;

    private Humanoid humanoid;

    private Transform target;
    
    [SerializeField] private float movementSpeed = 5f;
    
    private void Awake()
    {
        this.physicsBody = GetComponent<PhysicsBody>();
        this.object3D = GetComponent<Object3D>();
        this.humanoid = GetComponent<Humanoid>();
    }

    private void Start()
    {
        this.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!this.target) return;
        
        Vector3 direction = (this.target.position - this.transform.position).normalized;
        Vector3 newPosition = this.transform.position + direction * (this.movementSpeed * Time.deltaTime);
        
        if (Object3D.CheckCollisionAt(this.object3D.collisionId, newPosition + new Vector3(0, .1f, 0)) && this.physicsBody.isGrounded == PhysicsObjectState.Grounded)
            this.physicsBody.velocity.y += 5f;
        
        this.physicsBody.velocity.x = (direction.x * this.movementSpeed);
        this.physicsBody.velocity.z = (direction.z * this.movementSpeed);
    }
}

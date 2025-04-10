using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerCameraFollow cameraFollow;
    private PhysicsBody physicsBody;

    public float jumpForce = 8f;
    public float moveSpeed = 5f;
    
    private void Start()
    {
        this.physicsBody = GetComponent<PhysicsBody>();
    }
    
    private void Update()
    {
        this.cameraFollow.SetPlayerPosition(this.transform.position);
        Transform cam = Camera.main.transform;
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 camForward = cam.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        float intendedMoveSpeed = moveSpeed;
        if (physicsBody.isGrounded == PhysicsObjectState.Freefalling)
        {
            intendedMoveSpeed /= 2f;
        }
        
        this.physicsBody.velocity.x = moveDirection.x * intendedMoveSpeed;
        this.physicsBody.velocity.z = moveDirection.z * intendedMoveSpeed;
        
        if ((Input.GetKeyUp(KeyCode.Space)) && physicsBody.isGrounded == PhysicsObjectState.Grounded)
        {
            physicsBody.velocity.y += this.jumpForce;
        }
    }
}

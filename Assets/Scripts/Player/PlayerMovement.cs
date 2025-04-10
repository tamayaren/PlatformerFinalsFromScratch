using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerCameraFollow cameraFollow;
    private PhysicsBody physicsBody;

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

        float moveSpeed = 5f;
        this.physicsBody.velocity.x = moveDirection.x * moveSpeed;
        this.physicsBody.velocity.z = moveDirection.z * moveSpeed;
    }
}

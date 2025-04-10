using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    // Target to follow (will be set by EnhancedMeshGenerator)
    private Transform _target;
    
    // Camera positioning
    public Vector3 offset = new Vector3(0, -5, -10);
    public float smoothSpeed = 0.125f;
    
    // Optional camera constraints
    public bool useConstraints = true;
    public Vector2 xConstraint = new Vector2(-100f, 100f);
    public Vector2 yConstraint = new Vector2(-50f, 100f);
    
    // Camera options
    public bool followX = true;
    public bool followY = true;
    public bool followZ = false;
    
    // Reference to player position
    private Vector3 playerPosition;
    
    // This method will be called by EnhancedMeshGenerator to set the player position
    public void SetPlayerPosition(Vector3 position)
    {
        this.playerPosition = position;
    }
    
    void LateUpdate()
    {
        if (this.playerPosition == Vector3.zero)
            return;
            
        // Calculate desired position based on player position and offset
        Vector3 desiredPosition = this.transform.position;
        
        // Only follow axes we want to follow
        if (this.followX) desiredPosition.x = this.playerPosition.x + this.offset.x;
        if (this.followY) desiredPosition.y = this.playerPosition.y + this.offset.y;
        if (this.followZ) desiredPosition.z = this.playerPosition.z + this.offset.z;
        
        // Apply constraints if enabled
        if (this.useConstraints)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, this.xConstraint.x, this.xConstraint.y);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, this.yConstraint.x, this.yConstraint.y);
        }
        
        // Smoothly interpolate between current position and desired position
        Vector3 smoothedPosition = Vector3.Lerp(this.transform.position, desiredPosition, this.smoothSpeed);
        this.transform.position = smoothedPosition;
        
        // Look at player (optional - comment this out if you want fixed camera angle)
        this.transform.LookAt(this.playerPosition);
    }
}

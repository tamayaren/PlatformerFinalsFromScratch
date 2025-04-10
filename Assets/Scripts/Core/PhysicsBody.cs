using System;
using UnityEditor;
using UnityEngine;

public enum PhysicsObjectState
{
    Grounded,
    Freefalling
}

public class PhysicsBody : MonoBehaviour
{
    private Object3D object3D;
    public float mass = 1f;
    public float dragCoefficient = 0.5f;
    public bool autoMass = true;
    public bool anchored = false;
    public bool ignoreAir = false;
    public bool ignoreGravity = false;

    public PhysicsObjectState isGrounded = PhysicsObjectState.Freefalling;
    public Vector3 velocity;
    
    
    void Start()
    {
        this.object3D = GetComponent<Object3D>();
        
        if (this.autoMass)
            this.mass = Mathf.Max(this.transform.lossyScale.magnitude, 0.01f);
    }
    
    void Update()
    {
        if (this.anchored)
        {
            this.velocity = Vector3.zero;
            return;
        }

        Vector3 position = this.object3D.position;
        Vector3 intendedVelocity = this.velocity;
        
        // Apply gravity
        if (Object3D.CheckCollisionAt(this.object3D.collisionId, new Vector3(position.x, position.y + (this.velocity.y * Time.deltaTime), position.z)))
        {
            if (this.velocity.y < 0f)
                this.isGrounded = PhysicsObjectState.Grounded;

            this.velocity.y = 0f;
        }
        else
        {
            if (!this.ignoreGravity)
            {
                this.isGrounded = PhysicsObjectState.Freefalling;
                this.velocity.y -= Constants.gravity * Time.deltaTime;
            }
        }
        

        // Add air drag force
        Vector3 horizontalVelocity = new Vector3(this.velocity.x, 0f, this.velocity.z);
        if (!this.ignoreAir && horizontalVelocity.sqrMagnitude > 0f)
        {
            Vector3 dragForce = -horizontalVelocity.normalized * (this.dragCoefficient * horizontalVelocity.sqrMagnitude);
            
            // F = ma -> a = F / m
            Vector3 dragAcceleration = dragForce / this.mass;
            
            this.velocity.x += dragAcceleration.x * Time.deltaTime;
            this.velocity.z += dragAcceleration.z * Time.deltaTime;
            
            if (Mathf.Abs(this.velocity.x) < 0.01f) this.velocity.x = 0f;
            if (Mathf.Abs(this.velocity.z) < 0.01f) this.velocity.z = 0f;
        }

        // Add offset to not falsely check ground
        if (Object3D.CheckCollisionAt(this.object3D.collisionId,
                new Vector3(position.x + (intendedVelocity.x * Time.deltaTime), position.y + 0.1f, position.z)))
            this.velocity.x = 0f;
        if (Object3D.CheckCollisionAt(this.object3D.collisionId,
                new Vector3(position.x, position.y + 0.1f, position.z + (intendedVelocity.z * Time.deltaTime))))
            this.velocity.z = 0f;

        this.transform.position += this.velocity * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Handles.Label(this.transform.position, $"{this.object3D.collisionId} POS");
        Handles.Label(this.transform.position + (this.velocity), $"{this.object3D.collisionId} VELPOS");
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.velocity);
    }
}

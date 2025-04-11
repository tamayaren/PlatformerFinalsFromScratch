using System.Collections.Generic;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

// Enhanced MeshGenerator with collision, player control, and camera following
public class Renderer : MonoBehaviour
{
    public static Renderer instance;
    
    public Material material;
    public int instanceCount = 100;
    private Mesh cubeMesh;
    public List<Matrix4x4> matrices = new List<Matrix4x4>();
    public List<int> colliderIds = new List<int>();
    
    public float width = 1f;
    public float height = 1f;
    public float depth = 1f;
    
    public float movementSpeed = 5f;
    public float gravity = 9.8f;
    
    private int playerID = -1;
    private Vector3 playerVelocity = Vector3.zero;
    private bool isGrounded = false;
    
    // Camera reference
    public PlayerCameraFollow cameraFollow;
    
    // Z-position constant for all boxes
    public float constantZPosition = 0f;
    
    // Range for random generation
    public float minX = -50f;
    public float maxX = 50f;
    public float minY = -50f;
    public float maxY = 50f;
    
    // Ground plane settings
    public float groundY = -20f;
    public float groundWidth = 200f;
    public float groundDepth = 200f;

    void Awake()
    {
        if (instance == null) instance = this;
    }
    
    void Start()
    {
        // Create the cube mesh
        CreateCubeMesh();
        
        // Set up random boxes
        GenerateRandomBoxes();
    }

    void CreateCubeMesh()
    {
        this.cubeMesh = new Mesh();
        
        // Create 8 vertices for the cube (corners)
        Vector3[] vertices = new Vector3[8]
        {
            // Bottom face vertices
            new Vector3(0, 0, 0),       // Bottom front left - 0
            new Vector3(this.width, 0, 0),   // Bottom front right - 1
            new Vector3(this.width, 0, this.depth),// Bottom back right - 2
            new Vector3(0, 0, this.depth),   // Bottom back left - 3
            
            // Top face vertices
            new Vector3(0, this.height, 0),       // Top front left - 4
            new Vector3(this.width, this.height, 0),   // Top front right - 5
            new Vector3(this.width, this.height, this.depth),// Top back right - 6
            new Vector3(0, this.height, this.depth)    // Top back left - 7
        };
        
        // Triangles for the 6 faces (2 triangles per face)
        int[] triangles = new int[36]
        {
            // Front face triangles (facing -Z)
            0, 4, 1,
            1, 4, 5,
            
            // Back face triangles (facing +Z)
            2, 6, 3,
            3, 6, 7,
            
            // Left face triangles (facing -X)
            0, 3, 4,
            4, 3, 7,
            
            // Right face triangles (facing +X)
            1, 5, 2,
            2, 5, 6,
            
            // Bottom face triangles (facing -Y)
            0, 1, 3,
            3, 1, 2,
            
            // Top face triangles (facing +Y)
            4, 7, 5,
            5, 7, 6
        };
        
        Vector2[] uvs = new Vector2[8];
        for (int i = 0; i < 8; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / this.width, vertices[i].z / this.depth);
        }

        this.cubeMesh.vertices = vertices;
        this.cubeMesh.triangles = triangles;
        this.cubeMesh.uv = uvs;
        this.cubeMesh.RecalculateNormals();
        this.cubeMesh.RecalculateBounds();
    }
    
    void GenerateRandomBoxes()
    {
        // Create random boxes (excluding player and ground)
        for (int i = 0; i < this.instanceCount - 2; i++)
        {
            // Random position (constant Z)
            Vector3 position = new Vector3(
                Random.Range(this.minX, this.maxX),
                Random.Range(this.minY, this.maxY), this.constantZPosition
            );
            
            // Random rotation only around Z axis
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            
            // Random non-uniform scale - different for each dimension
            Vector3 scale = new Vector3(
                Random.Range(0.5f, 3f),
                Random.Range(0.5f, 3f),
                Random.Range(0.5f, 3f)
            );
            
            // Register with collision system - properly handle rectangular shapes
            int id = CollisionManager.Instance.RegisterCollider(
                this.gameObject,
                position, 
                new Vector3(this.width * scale.x, this.height * scale.y, this.depth * scale.z), 
                true,
                "Object");
            
            // Create transformation matrix
            Matrix4x4 boxMatrix = Matrix4x4.TRS(position, rotation, scale);
            this.matrices.Add(boxMatrix);
            this.colliderIds.Add(id);
            
            // Update the matrix in collision manager
            CollisionManager.Instance.UpdateMatrix(id, boxMatrix);
        }
    }

    void Update()
    {
        RenderBoxes();
    }
    
    bool CheckCollisionAt(int id, Vector3 position)
    {
        return CollisionManager.Instance.CheckCollision(id, position, out _, out _);
    }
    
    void RenderBoxes()
    {
        // Convert list to array for Graphics.DrawMeshInstanced
        Matrix4x4[] matrixArray = this.matrices.ToArray();
        
        // Draw instanced meshes in batches of 1023 (GPU limit)
        for (int i = 0; i < matrixArray.Length; i += 1023) {
            int batchSize = Mathf.Min(1023, matrixArray.Length - i);
            Matrix4x4[] batchMatrices = new Matrix4x4[batchSize];
            System.Array.Copy(matrixArray, i, batchMatrices, 0, batchSize);
            Graphics.DrawMeshInstanced(this.cubeMesh, 0, this.material, batchMatrices, batchSize);
        }
    }

    void DecomposeMatrix(Matrix4x4 matrix, out Vector3 position, out Quaternion rotation, out Vector3 scale)
    {
        position = matrix.GetPosition();
        rotation = matrix.rotation;
        scale = matrix.lossyScale;
    }
    
    // Add a new random box at runtime (can be called from button or other trigger)
    public void AddRandomBox()
    {
        Vector3 position = new Vector3(
            Random.Range(this.minX, this.maxX),
            Random.Range(this.minY, this.maxY), this.constantZPosition
        );
        
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        
        // Random non-uniform scale - different for each dimension
        Vector3 scale = new Vector3(
            Random.Range(0.5f, 3f),
            Random.Range(0.5f, 3f),
            Random.Range(0.5f, 3f)
        );
        
        // Register with collision system - properly handle rectangular shapes
        int id = CollisionManager.Instance.RegisterCollider(
            this.gameObject,
            position, 
            new Vector3(this.width * scale.x, this.height * scale.y, this.depth * scale.z), true);
        
        Matrix4x4 boxMatrix = Matrix4x4.TRS(position, rotation, scale);
        this.matrices.Add(boxMatrix);
        this.colliderIds.Add(id);
        
        CollisionManager.Instance.UpdateMatrix(id, boxMatrix);
    }
}
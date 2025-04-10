using UnityEngine;

// Object3D : uses transform
public class Object3D : MonoBehaviour
{
    public bool canCollide = true;

    public Vector3 position = Vector3.zero;
    public Vector3 size = Vector3.one;
    public string tag = "Object";
    public int collisionId;
    
    private bool instantiated = false;
    
    private void Instantiate()
    {
        Quaternion rotation = this.transform.rotation;

        this.position = this.transform.position;
        this.collisionId = CollisionManager.Instance.RegisterCollider(this.position + this.size / 2f, this.size, this.tag);
        
        Matrix4x4 matrix = UpdateMesh(this.position, size, rotation);
        
        Renderer.instance.matrices.Add(matrix);
        Renderer.instance.colliderIds.Add(this.collisionId);
        
        CollisionManager.Instance.UpdateMatrix(this.collisionId, matrix);
        this.instantiated = true;
    }
    
    private void Start() => Instantiate();

    private Matrix4x4 UpdateMesh(Vector3 position, Vector3 scale, Quaternion rotation)
    {
        Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);
        return matrix;
    }
    
    public void Update()
    {
        if (!this.instantiated) return;
        Quaternion rotation = this.transform.rotation;

        this.position = this.transform.position;
        Matrix4x4 matrix = UpdateMesh(this.position, this.size, rotation);
        Renderer.instance.matrices[Renderer.instance.colliderIds.IndexOf(this.collisionId)] = matrix;
        
        CollisionManager.Instance.UpdateMatrix(this.collisionId, matrix);
        CollisionManager.Instance.UpdateCollider(this.collisionId, this.position + this.size / 2f, this.size);
    }

    public static bool CheckCollisionAt(int id, Vector3 position)
    {
        return CollisionManager.Instance.CheckCollision(id, position, out _);
    }
}

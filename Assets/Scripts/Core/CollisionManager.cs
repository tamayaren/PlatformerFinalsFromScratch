using System;
using System.Collections.Generic;
using UnityEngine;

public class AABBBounds
{
    public Vector3 Center { get; private set; }
    public Vector3 Size { get; private set; }
    public Vector3 Extents { get; private set; }
    public Vector3 Min { get; private set; }
    public Vector3 Max { get; private set; }
    public int ID { get; private set; }
    public string tag { get; private set; }
    public Matrix4x4 Matrix { get; set; }

    public AABBBounds(Vector3 center, Vector3 size, int id, string tag = "Object")
    {
        this.ID = id;
        this.tag = tag;
        UpdateBounds(center, size);
    }

    public void UpdateBounds(Vector3 center, Vector3 size)
    {
        this.Center = center;
        this.Size = size;
        this.Extents = size * 0.5f; // Half-size for efficient calculations
        this.Min = center - this.Extents;
        this.Max = center + this.Extents;
    }

    public bool Intersects(AABBBounds other)
    {
        return !(this.Max.x < other.Min.x || this.Min.x > other.Max.x || this.Max.y < other.Min.y || this.Min.y > other.Max.y || this.Max.z < other.Min.z || this.Min.z > other.Max.z);
    }
}

public class CollisionManager : MonoBehaviour
{
    private static CollisionManager _instance;
    public static CollisionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("CollisionManager");
                _instance = go.AddComponent<CollisionManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private Dictionary<int, AABBBounds> _colliders = new Dictionary<int, AABBBounds>();
    private int nextID = 0;

    public int RegisterCollider(Vector3 center, Vector3 size, string tag = "Object")
    {
        int id = this.nextID++;
        this._colliders[id] = new AABBBounds(center, size, id, tag);
        return id;
    }

    public void UpdateCollider(int id, Vector3 center, Vector3 size)
    {
        if (this._colliders.TryGetValue(id, out AABBBounds bounds))
        {
            bounds.UpdateBounds(center, size);
        }
    }

    public void RemoveCollider(int id)
    {
        if (this._colliders.ContainsKey(id))
        {
            this._colliders.Remove(id);
        }
    }

    public void UpdateMatrix(int id, Matrix4x4 matrix)
    {
        if (this._colliders.TryGetValue(id, out AABBBounds bounds))
        {
            bounds.Matrix = matrix;
        }
    }

    public bool CheckCollision(int id, Vector3 newCenter, out List<int> collidingIds)
    {
        collidingIds = new List<int>();
        if (!this._colliders.TryGetValue(id, out AABBBounds current))
            return false;

        // Create a temporary bounds for collision check
        AABBBounds temp = new AABBBounds(newCenter, current.Size, -1);

        bool collided = false;
        foreach (var kvp in this._colliders)
        {
            if (kvp.Key == id) continue; // Skip self-collision

            if (temp.Intersects(kvp.Value))
            {
                collidingIds.Add(kvp.Key);
                collided = true;
            }
        }
        return collided;
    }

    public Matrix4x4 GetMatrix(int id)
    {
        if (this._colliders.TryGetValue(id, out AABBBounds bounds))
        {
            return bounds.Matrix;
        }
        return Matrix4x4.identity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var kvp in this._colliders)
        {
            Gizmos.DrawWireCube(kvp.Value.Center, kvp.Value.Size);
        }
    }
}
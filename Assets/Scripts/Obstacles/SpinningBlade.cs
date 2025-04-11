using System;
using UnityEditor;
using UnityEngine;

public class SpinningBlade : MonoBehaviour
{
    private Object3D object3D;

    public float interval;
    private float rotation;

    private Transform target;
    
    private void Start()
    {
        this.object3D = GetComponent<Object3D>();
        
        this.target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void KillTarget()
    {
        bool humanoidExist = this.target.TryGetComponent(out Humanoid humanoid);
        if (!humanoidExist) return;
        
        if (humanoid.stateType == HumanoidStateType.Alive)
            humanoid.Kill();
    }

    private void Update()
    {
        this.rotation += this.interval * Time.deltaTime;
        
        this.transform.rotation = Quaternion.Euler(0, this.rotation, 0);
        
        float distance = Vector3.Distance(this.transform.position, this.target.position);

        if (distance <= this.object3D.size.z)
        {
            if (ConsolidatedPowerups.instance.marioStarPowerup) return;
            KillTarget();
        }
    }

    private void OnDrawGizmos()
    {
        
        float distance = Vector3.Distance(this.transform.position, this.target.position);
        
        Handles.Label(this.transform.position, $"DIST: {distance}");
    }
}

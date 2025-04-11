using System.Collections;
using System.Linq;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    private Object3D object3D;
    private PhysicsBody physicsBody;

    public bool isStarted = false;
    public float speed = 8f;
    
    public float checkTickSpeed = 1/30f;
    public float interval = 0f;
    private void Start()
    {
        this.physicsBody = GetComponent<PhysicsBody>();
        this.object3D = GetComponent<Object3D>();
    }

    private void Update()
    {
        if (!this.isStarted) return;
        
        this.physicsBody.velocity += this.transform.forward * this.speed;
        
        this.interval += Time.deltaTime;
        if (this.interval >= this.checkTickSpeed)
        {
            this.interval = 0f;
            GameObject target = GameObject.FindGameObjectsWithTag("Enemy").
                OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).FirstOrDefault();

            if (target && Vector3.Distance(this.transform.position, target.transform.position) < 12f)
            {
                bool isHumanoid = target.TryGetComponent(out Humanoid humanoid);

                if (!isHumanoid) return;
                humanoid.Kill();
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(8f);
        Destroy(this.gameObject);
    }

    public void StartProjectile()
    {
        this.isStarted = true;
        StartCoroutine(Despawn());
    }
}

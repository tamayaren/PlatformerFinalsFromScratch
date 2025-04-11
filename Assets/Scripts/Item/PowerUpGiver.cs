using UnityEngine;

public class PowerUpGiver : MonoBehaviour
{
    [SerializeField] private int powerupInt = 0;
    private Transform target;
    private Object3D object3D;

    private void Start()
    {
        this.target = GameObject.FindGameObjectWithTag("Player")?.transform;
        this.object3D = GetComponent<Object3D>();
        
        Random.seed = System.DateTime.Now.Millisecond;
        this.powerupInt = Random.Range(1, 3)-1;
    }

    private void Update()
    {
        float distance = Vector3.Distance(this.transform.position, this.target.position);

        if (distance < 2f)
        {
            ConsolidatedPowerups.instance.GetRandomizePowerUp(this.powerupInt);

            this.object3D.visible = false;
            Destroy(this.gameObject);
        }
    }
}

using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private float minInterval = 5f;
    [SerializeField] private float maxInterval = 8f;

    [SerializeField] private Vector3 maxPosition;
    [SerializeField] private float maxEnemies = 6f;
    private float interval = 0f;
    private float elapsed = 0f;

    public void Start()
    {
        this.interval = Random.Range(this.minInterval, this.maxInterval);
    }

    public void Update()
    {
        this.elapsed += Time.deltaTime;

        if (this.elapsed >= this.interval)
        {
            this.interval = Random.Range(this.minInterval, this.maxInterval);
            this.elapsed = 0f;

            if (GameObject.FindGameObjectsWithTag("Enemy").Length >= this.maxEnemies) return;
            
            Vector3 randomPosition = new Vector3(
                Random.Range(-this.maxPosition.x, this.maxPosition.x) * .5f,
                this.transform.position.y,
                Random.Range(-this.maxPosition.z, this.maxPosition.z) * .5f
            );
            
            Instantiate(this.enemyPrefab, randomPosition, Quaternion.identity);
        }
    }
}

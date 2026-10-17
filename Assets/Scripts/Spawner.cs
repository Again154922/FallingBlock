using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject fallerPrefab;
    
    [Header("生成间隔")]
    [SerializeField] private float startInterval = 1.0F;
    [SerializeField] private float minInterval = 0.32F;
    [SerializeField] private float rampSeconds = 60F;

    [Header("生成位置")]
    [SerializeField] private float spawnY = 6.5F;
    [SerializeField] private float rangeX = 7.2F;

    [Header("下落速度")]
    [SerializeField] private float startGravity = 0.7F;
    [SerializeField] private float maxGravity = 2.4F;

    private float elapsed;
    private float timer;
    
    public void OnRestart()
    {
        elapsed = 0F;
        timer = 0F;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        timer += Time.deltaTime;

        float difficulty = Mathf.Clamp01(elapsed / rampSeconds);
        float interval = Mathf.Lerp(startInterval, minInterval, difficulty);
        
        if (timer < interval) return;
        timer = 0F;
        
        Spawn(difficulty);
    }

    void Spawn(float difficulty)
    {
        Vector3 position = new Vector3(Random.Range(-rangeX, rangeX), spawnY, 0);
        GameObject faller = Instantiate(fallerPrefab, position, Quaternion.identity, transform);
        
        Rigidbody2D rb = faller.GetComponent<Rigidbody2D>();
        rb.gravityScale = Mathf.Lerp(startGravity, maxGravity, difficulty);
    }
}

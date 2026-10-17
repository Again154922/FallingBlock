using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject fallerPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 60 != 0) return;
        
        float randomX = Random.Range(-7.2F, 7.2F);
        var newFaller = Instantiate(fallerPrefab, new Vector3(randomX, 6.5F, 0), Quaternion.identity, transform);
        var fallerTransform = newFaller.GetComponent<Transform>();

        while (fallerTransform.position.y > -6.5F)
        {
            
        }
    }
}

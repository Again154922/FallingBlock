using UnityEngine;

public class Faller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -6.5F)
        {
            GameManager.Instance.OnBlockDestroy();
            Destroy(gameObject);
        }
    }
}

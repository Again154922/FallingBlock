using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 9;
    private float limitX = 7.2F;
    [SerializeField] private Transform playerTransform;
    
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log(speed);
        Debug.Log(limitX);
    }

    // Update is called once per frame
    void Update()
    {
        Direction? direction = GetDirection();
        if (direction is null) return;
        float targetX = playerTransform.position.x + (float)direction * speed * Time.deltaTime;

        if (targetX < -limitX)
        {
            playerTransform.position = new Vector3(-limitX, -4.5F, 0);
            return;
        }
        if (targetX > limitX)
        {
            playerTransform.position = new Vector3(limitX, -4.5F, 0);
            return;
        }
        
        playerTransform.position = new Vector3(targetX, -4.5F, 0);
    }

    Direction? GetDirection()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            return Direction.Left;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            return Direction.Right;
        }
        return null;
    }
}

public enum Direction
{
    Left = -1,
    Right = 1
}

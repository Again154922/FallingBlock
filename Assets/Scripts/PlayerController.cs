using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 15;
    [SerializeField] private float limitX = 7.2F;

    // Update is called once per frame
    void Update()
    {
        Direction? direction = GetDirection();
        if (direction is null) return;
        float targetX = transform.position.x + (float)direction * speed * Time.deltaTime;

        if (targetX < -limitX)
        {
            transform.position = new Vector3(-limitX, -4.5F, 0);
            return;
        }
        if (targetX > limitX)
        {
            transform.position = new Vector3(limitX, -4.5F, 0);
            return;
        }
        
        transform.position = new Vector3(targetX, -4.5F, 0);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Faller>(out _)) return;
        
        Destroy(other.gameObject);
        GameManager.Instance.OnCatch();
    }
}

public enum Direction
{
    Left = -1,
    Right = 1
}

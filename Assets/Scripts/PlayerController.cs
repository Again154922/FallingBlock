using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 15;
    [SerializeField] private float edgePadding = 0.2F;   // 离屏幕边缘留的余量

    private Camera _cam;
    private float _halfWidth;

    void Awake()
    {
        _cam = Camera.main;
        _halfWidth = transform.localScale.x * 0.5F;
    }

    // Update is called once per frame
    void Update()
    {
        Direction? direction = GetDirection();
        if (direction is null) return;

        float limitX = CurrentLimitX();
        float targetX = transform.position.x + (float)direction * speed * Time.deltaTime;
        targetX = Mathf.Clamp(targetX, -limitX, limitX);

        transform.position = new Vector3(targetX, transform.position.y, 0);
    }

    /// <summary>
    /// 可视宽度的一半 - 自身半宽 - 余量：窗口变窄时边界自动收紧，
    /// 保证挡板永远不出画面。
    /// </summary>
    private float CurrentLimitX()
    {
        float halfViewWidth = _cam.orthographicSize * _cam.aspect;
        return Mathf.Max(0.5F, halfViewWidth - _halfWidth - edgePadding);
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

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Visual")]
    public bool flipSprite = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // 1 = sağ, -1 = sol
    private int direction = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        UpdateMovement();
        UpdateVisual();
    }

    void Update()
    {
        // Tıklandığında yön değiştir
        if (Input.GetMouseButtonDown(0))
        {
            ReverseDirection();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    void ReverseDirection()
    {
        direction *= -1;
        UpdateMovement();
        UpdateVisual();
    }

    void UpdateMovement()
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    void UpdateVisual()
    {
        if (!flipSprite || spriteRenderer == null) return;
        spriteRenderer.flipX = direction < 0;
    }

    // 🔥 DUVARA DEĞİNCE OTOMATİK TERS DÖN
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("SagDuvar") ||
            collision.collider.CompareTag("SolDuvar"))
        {
            ReverseDirection();
        }
    }
}

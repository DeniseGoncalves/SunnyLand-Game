using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement and Jump")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;

    [Header("Components")]
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Esquerda: vai de 0 a 1, Direita: vai de 0 a -1
        rb.linearVelocityX = movementSpeed * horizontal;
    }
}

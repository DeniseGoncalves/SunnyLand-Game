using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement and Jump")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundLayerRadius;
    private bool isGrounded;

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

        HandleInput();

    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    void HandleInput() //Handle vem de manipular
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Esquerda: vai de 0 a 1, Direita: vai de 0 a -1

        //if(condição) {o que fazer caso a condição seja atendida}
        //if(apertei o botão de pulo?) {Pular();}
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true) //Verifica se a tecla de espaço foi pressionada e se o jogador está no chão
        {
            Jump();
        }

        rb.linearVelocityX = movementSpeed * horizontal; //Define a velocidade horizontal do Rigidbody2D com base na entrada do jogador

    }
    
    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, groundLayerRadius, groundLayer); //Verifica se há colisores na camada de chão dentro do círculo definido
    }

    void Jump()
    {
        rb.linearVelocityY = 0; //Zerar a velocidade vertical antes de pular
        rb.AddForceY(jumpForce, ForceMode2D.Impulse); //Adiciona uma força instantânea para cima
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, groundLayerRadius); //Desenha um círculo no editor para visualizar a área de verificação do chão
    }
}

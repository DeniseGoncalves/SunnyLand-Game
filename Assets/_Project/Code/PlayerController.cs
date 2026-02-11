using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator    animator;

    [Header("Movement and Jump")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField]private bool isLookLeft; //false == olhando para a direita, true == olhando para a esquerda
    private bool  isWalk;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundLayerRadius;
    private bool isGrounded;

    [Header("Stomp")]
    [SerializeField] private LayerMask stompLayer;
    [SerializeField] private Vector2 stompBoxSize;
    [SerializeField] private Vector2 stompBoxOffset;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        HandleInput();
        UpdateAnimations();

    }

    private void FixedUpdate()
    {
        GroundCheck();
        HandleStomp();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.transform.name);
    }

    void HandleInput() //Handle vem de manipular
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Direita: vai de 0 a 1, Esquerda: vai de 0 a -1
        //> 0 == andando para a direita, < 0 == andando para a esquerda, == 0 parado

        isWalk = horizontal != 0; //Se horizontal for diferente de 0, isWalk é true, senão é false

        if (horizontal > 0 && isLookLeft == true) //Estou andando para a direita e olhando para a esquerda?
        {
            //Sim
            Flip();
        }
        //Se não
        else if (horizontal < 0 && isLookLeft == false) //Estou andando para a esquerda e olhando para a direita?
        {
            //Sim
            Flip();
        }
        
        //if(condição) {o que fazer caso a condição seja atendida}
        //if(apertei o botão de pulo?) {Pular();}
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true) //Verifica se a tecla de espaço foi pressionada e se o jogador está no chão
        {
            Jump();
        }

        rb.linearVelocityX = movementSpeed * horizontal; //Define a velocidade horizontal do Rigidbody2D com base na entrada do jogador

    }

    void UpdateAnimations()
    {
        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("speedY", rb.linearVelocityY);
    }

    void HandleStomp()
    {
        //Não estou pisando no chão?
        //Estou caindo?

        if(isGrounded == false && rb.linearVelocityY < 0)
        {
            Collider2D col = Physics2D.OverlapBox((Vector2)transform.position + stompBoxOffset, stompBoxSize, 0, stompLayer);

            if(col != null)
            {
                Debug.Log(col.transform.name);
                //Debug.LogError("Pause");
                Jump(); //Pula novamente para simular o efeito de stomp
                
            }
        }
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

    void Flip()
    {
        isLookLeft = !isLookLeft; //Inverte o valor de isLookLeft
        Vector3 scale = transform.localScale; //Obtém a escala local do transform
        scale.x *= -1; //Inverte o sinal do X
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, groundLayerRadius); //Desenha um círculo no editor para visualizar a área de verificação do chão

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + stompBoxOffset, stompBoxSize); //Desenha um retângulo no editor para visualizar a área de verificação do stomp
    }
}

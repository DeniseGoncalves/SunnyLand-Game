using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator    animator;
    private PlayerHealthManager playerHealthManager;

    [Header("Movement and Jump")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField]private bool isLookLeft; //false == olhando para a direita, true == olhando para a esquerda
    private bool  isWalk;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //Tempo que o jogador fica no ar após pular
    private float coyoteTimeCounter; //Contador de tempo que o jogador fica no ar após pular

    [Header("Buffer Jump")]
    [SerializeField] private float jumpBufferTimer; //Tempo que o jogador pode apertar o botão de pulo antes de tocar o chão
    private float jumpBufferCounter;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundLayerRadius;
    private bool isGrounded;

    [Header("Wall Slide / Wall Jump")]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private bool isTouchingWall;

    [Header("Stomp")]
    [SerializeField] private LayerMask stompLayer;
    [SerializeField] private Vector2 stompBoxSize;
    [SerializeField] private Vector2 stompBoxOffset;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerHealthManager = GetComponent<PlayerHealthManager>();
    }

    // Update is called once per frame
    void Update()
    {

        HandleInput();
        UpdateAnimations();
        HandleCoyoteTime();
        HandleJumpBuffer();
        HandleWallSlide();

    }

    private void FixedUpdate()
    {
        GroundCheck();
        HandleStomp();
        WallCheck();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Danger":
                int defaultDamage = 1; //Valor de dano padrão
                playerHealthManager.TakeDamage(defaultDamage); //Chama a função de tomar dano do PlayerHealthManager, passando o valor de 1 como argumento
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Danger":
                //int defaultDamage = 1; //Valor de dano padrão
                //playerHealthManager.TakeDamage(defaultDamage); //Chama a função de tomar dano do PlayerHealthManager, passando o valor de 1 como argumento
                break;

            case "Item":
                print("Peguei um item!");
                collision.gameObject.SetActive(false); //Desativa o objeto para simular que ele foi coletado
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "movingPlataform":
                transform.parent = collision.transform; //Faz o jogador se tornar filho da plataforma, para que ele se mova junto com ela
                break;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "movingPlataform":
                transform.parent = null; //Remove a relação de paternidade, para que o jogador não se mova mais com a plataforma
                break;
        } 
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
        if (Input.GetKeyDown(KeyCode.Space)) //Verifica se a tecla de espaço foi pressionada e se o jogador está no chão
        {
            jumpBufferCounter = jumpBufferTimer; //Reseta o contador de tempo do buffer de pulo

            if(isGrounded == true || coyoteTimeCounter > 0) //Se o jogador estiver no chão ou se o contador de coyote time for maior que 0
            {
                Jump();
            }
        }

        rb.linearVelocityX = movementSpeed * horizontal; //Define a velocidade horizontal do Rigidbody2D com base na entrada do jogador

    }

    void UpdateAnimations()
    {
        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("speedY", rb.linearVelocityY);
        animator.SetFloat("isWall", isTouchingWall == true ? 1 : 0); //Se isTouchingWall for true, então isWall será 1, senão será 0
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
                if(col.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager script)) //Verifica se o objeto colidido possui o componente EnemyHealthManager
                {
                    script.TakeDamage(1);
                }
                //col.GetComponent<EnemyHealthManager>().TakeDamage(1);
                Jump(); //Pula novamente para simular o efeito de stomp
                
            }
        }
    }
    
    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, groundLayerRadius, groundLayer); //Verifica se há colisores na camada de chão dentro do círculo definido
        /*if(rb.linearVelocityY > 0) //Se a velocidade vertical for maior que 0, significa que o jogador está subindo
        {
            isGrounded = false; //Então o jogador não está no chão
        }*/

    }

    void WallCheck()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer); //Verifica se há colisores na camada de parede à esquerda do jogador
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer); //Verifica se há colisores na camada de parede à direita do jogador

        if(hitLeft.collider != null || hitRight.collider != null) //Se houver colisores à esquerda ou à direita do jogador
        {
            isTouchingWall = true; //Então o jogador está tocando uma parede
        }
        else
        {
            isTouchingWall = false; //Caso contrário, o jogador não está tocando uma parede
        }
    }

    void Jump()
    {
        rb.linearVelocityY = 0; //Zerar a velocidade vertical antes de pular
        rb.AddForceY(jumpForce, ForceMode2D.Impulse); //Adiciona uma força instantânea para cima
    }

    void HandleCoyoteTime()
    {
        if(isGrounded == true)
        {
            coyoteTimeCounter = coyoteTime; 
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; //Diminui o contador de tempo a cada frame - tipo uma contagem regressiva
        }
    }

    void HandleJumpBuffer()
    {
        jumpBufferCounter -= Time.deltaTime; //Diminui o contador de tempo a cada frame - tipo uma contagem regressiva

        if(jumpBufferCounter > 0 && isGrounded == true) //Se o contador de tempo for maior que 0 e o jogador estiver no chão
        {
            Jump();
            jumpBufferCounter = 0; //Zera o contador de tempo para evitar múltiplos pulos
        }
    }

    void HandleWallSlide()
    {
        //Estou tocando na parede?
        //Estou no chão?
        //Estou caindo?

        if(isTouchingWall == true && isGrounded == false && rb.linearVelocityY < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -wallSlideSpeed)); //Define a velocidade vertical do Rigidbody2D para o valor máximo entre a velocidade atual e a velocidade de deslize na parede
        }
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

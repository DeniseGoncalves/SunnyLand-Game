using UnityEngine;

public class EnemyBehaviorMoveAtoB : MonoBehaviour
{

    [SerializeField] private Transform enemyTransform;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float movementSpeed;

    [SerializeField] private bool isLookLeft;
    private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetEnemy();
    }

    void Update()
    {
        enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, target.position, movementSpeed * Time.deltaTime);

        if(Vector2.Distance(enemyTransform.position, target.position) < 0.05f) // se a distância entre o inimigo e o destino for menor que 0.05f
        {
            //Operador ternário: se o destino for o ponto A, então o destino será o ponto B, caso contrário, será o ponto A
            //condição ? valor_se_verdadeiro : valor_se_falso
            target = target == pointA ? pointB : pointA;
        }

        if(target.position.x > enemyTransform.position.x && isLookLeft == true) // meu destino está a direita e estou olhando para a esquerda?
        {
            Flip();
        }
        else if(target.position.x < enemyTransform.position.x && isLookLeft == false)
        {
            Flip();
        }
    }

    void Flip()
    {
        isLookLeft = !isLookLeft;
        Vector3 scale = enemyTransform.localScale;
        scale.x *= -1;
        enemyTransform.localScale = scale;
    }

    private void ResetEnemy()
    {
        enemyTransform.position = pointA.position;
        target = pointB;
        if(isLookLeft == true)
        {
            Flip();
        }
    }
    
}

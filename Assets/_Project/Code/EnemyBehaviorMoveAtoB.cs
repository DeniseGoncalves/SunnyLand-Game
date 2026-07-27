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
    }

    void Flip()
    {
        
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

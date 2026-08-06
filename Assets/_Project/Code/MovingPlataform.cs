using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    [SerializeField] private Transform plataform;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float movementSpeed;

    private int currentIndex;
    private bool isReversing;

    private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plataform.position = waypoints[0].position;
        currentIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(waypoints.Length < 2 || plataform == null)
        {
            return;
        }

        target = waypoints[currentIndex];

        if(Vector3.Distance(plataform.position, target.position) < 0.01f)
        {
            if(isReversing == false) //estou indo, iremos somar
            {
                currentIndex++;
                if(currentIndex >= waypoints.Length)
                {
                    currentIndex = waypoints.Length - 2;
                    isReversing = true;
                }
            }
        }
    }
}

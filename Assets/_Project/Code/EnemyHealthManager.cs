using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] private GameObject rootObject; //Objeto raiz do inimigo

    [Header("Health")]
    [SerializeField] private int maxHP; //Vida máxima
    private int currentHP; //Vida atual
    

    private void Start()
    {
        currentHP = maxHP; //A vida atual começa com a vida máxima
    }

    public void TakeDamage(int value)
    {
        currentHP -= value; //Subtrai o valor do dano da vida atual

        if(currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        rootObject.SetActive(false); //Desativa a raiz do inimigo, fazendo com que ele desapareça da cena
    }
}

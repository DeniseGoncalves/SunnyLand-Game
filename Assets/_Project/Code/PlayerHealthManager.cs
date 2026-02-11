using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private int maxHP; //Vida máxima
    private int currentHP; //Vida atual

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP; //A vida atual começa com a vida máxima
    }

    public void TakeDamage(int value) //valor de dano
    {
        currentHP -= value; //Subtrai o valor do dano da vida atual

        if (currentHP <= 0) //Se a vida atual for menor ou igual a 0
        {
            Die(); //Chama a função de morrer
        }
    }

    void Die()
    {
        print("Você morreu!"); 
        //Tenho vidas extras? Se a resposta for SIM (respawn), se NÃO (game over)
    }
}

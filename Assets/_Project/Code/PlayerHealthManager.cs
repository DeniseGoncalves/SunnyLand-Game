using UnityEngine;
using System.Collections;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private int maxHP; //Vida máxima
    private int currentHP; //Vida atual

    private SpriteRenderer spriteRenderer; //Componente SpriteRenderer do jogador
    private bool isInvulnerable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP; //A vida atual começa com a vida máxima
        spriteRenderer = GetComponent<SpriteRenderer>(); //Pega o componente SpriteRenderer do jogador
    }

    public void TakeDamage(int value, bool isDead = false) //valor de dano
    {
        if (isDead == true) //Se o jogador estiver morto, chama a função de morrer
        {
            Die();
            return; //Para a execução do resto do método takeDamage
        }

        if(isInvulnerable)
        {
            return; //Se o jogador estiver invulnerável, não recebe dano. O TakeDamage é interrompido por aqui
        }

        print("Tomei dano");

        currentHP -= value; //Subtrai o valor do dano da vida atual

        if (currentHP <= 0) //Se a vida atual for menor ou igual a 0
        {
            Die(); //Chama a função de morrer
        }
        else //Caso a vida seja maior que zero
        {
            StartCoroutine(nameof(DamageFeedbackCoroutine));
        }
    }

    void Die()
    {
        print("Você morreu!"); 
        //Tenho vidas extras? Se a resposta for SIM (respawn), se NÃO (game over)
    }

    IEnumerator DamageFeedbackCoroutine()
    {
        spriteRenderer.color = Color.red; //Muda a cor do jogador para vermelho
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;

        StartCoroutine(nameof(InvulnerabilityCoroutine));
    }

    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true; 

        for(int i = 0; i < 8; i++)
        {
            spriteRenderer.enabled = false; //Desativa o sprite do jogador
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.enabled = true; //Ativa o sprite do jogador
            yield return new WaitForSeconds(0.15f);
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }
}

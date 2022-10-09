using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoEAtaques : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movimentação")]
    [SerializeField] private float velocidade = 8f;
    private float horizontal;

    [Header("Pulo")]
    public Transform checkChao;
    public LayerMask camadaChao;
    [SerializeField]  private float forcaDoPulo = 8f;
    private int ContadorDePulos = 0;
    private bool olharDireita = true;


    [Header("Ataque")]
    public Transform pontoAtaque;
    public LayerMask camadaInimigos;
    [SerializeField]  private float alcanceAtaque = 1f;
    private int contadorDeGolpes = 0;
    [SerializeField] [Range(0,1)] private float atrasoAtaque = 0.5f;
    private bool podeBater = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * velocidade , rb.velocity.y);

        if (!olharDireita && horizontal > 0) Virar();
        else if(olharDireita && horizontal < 0) Virar();

        if (rb.velocity.y < 0) rb.velocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;

    }

    public void Movimentacao(InputAction.CallbackContext context) 
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Pular(InputAction.CallbackContext context) 
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        if (context.performed && EstaNoChao()) rb.velocity = new Vector2 (rb.velocity.x, forcaDoPulo);
        if (context.canceled && rb.velocity.y > 0f && ContadorDePulos == 1) {
            rb.velocity = new Vector2(rb.velocity.x, forcaDoPulo * 0.5f);
            ContadorDePulos++;
        }
        ContadorDePulos++;
    }

    private IEnumerator AtrasoAtaque() 
    {
        yield return new WaitForSeconds(atrasoAtaque);
        podeBater = true;
    }

    public void Ataque(InputAction.CallbackContext context) 
    {
        if (context.canceled && EstaNoChao() && podeBater) 
        {
            Collider2D[] ataque = Physics2D.OverlapCircleAll(pontoAtaque.position, alcanceAtaque, camadaInimigos );
            foreach (Collider2D inimigo in ataque) 
            {
                Debug.Log("acertou um inimigo");
            }
            podeBater = false;
            contadorDeGolpes++;
            StartCoroutine(AtrasoAtaque());
        } 
        else if (context.canceled && !EstaNoChao() && podeBater) 
        {
            Collider2D[] ataque = Physics2D.OverlapCircleAll(pontoAtaque.position, alcanceAtaque, camadaInimigos);
            foreach (Collider2D inimigo in ataque)
            {
                Debug.Log("acertou um inimigo");
            }
            podeBater = false;
            contadorDeGolpes++;
            StartCoroutine(AtrasoAtaque());
        }

        //if (contadorDeGolpes == 1) {
        //    // animação do golpe 1
        //} else if (contadorDeGolpes == 2) {
        //    // animação do golpe 2
        //}
        //else if (contadorDeGolpes == 3)
        //{
        //    // animação do golpe 3
        //    contadorDeGolpes = 0;
        //}
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(pontoAtaque.position, alcanceAtaque);
    }

    private bool EstaNoChao() 
    {
        if(Physics2D.OverlapCircle(checkChao.position, 0.1f, camadaChao)) ContadorDePulos = 0;
        return Physics2D.OverlapCircle(checkChao.position, 0.1f, camadaChao);
    }

    private void Virar() 
    {
        olharDireita = !olharDireita;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoEAtaques : MonoBehaviour
{
    [Header("Prefab")]
    private Rigidbody2D rb;
    private Animator anim;
    public GameObject player;

    [Header("Movimenta��o")]
    [SerializeField] private float velocidade = 8f;
    private float horizontal;
    private bool podeMover = true;

    [Header("Pulo")]
    public Transform checkChao;
    public LayerMask camadaChao;
    [SerializeField]  private float forcaDoPulo = 8f;
    private int ContadorDePulos = 0;
    private bool olharDireita = true;
    [SerializeField] private float tempoCoyote = 0.2f;
    private float contadortempoCoyote;

    [Header("Ataque")]
    public Transform pontoAtaque;
    public LayerMask camadaInimigos;
    [SerializeField]  private float alcanceAtaque = 1f;
    [SerializeField] [Range(0,1)] private float atrasoAtaque = 0.5f;
    private int contadorGolpesChao = 0;
    private int contadorGolpesAereos = 0;
    private bool podeBater = true;
    private bool estaAtacando = true;
    public GameObject escudo;
    public LayerMask camadaBala;

    [Header("Dash")]
    [SerializeField] private float forcaDash = 24f;
    [SerializeField] private float duracaoDash = 0.5f;
    [SerializeField] private float esperaDash = 2f;
    [SerializeField] private float gravidadeDash;
    [SerializeField] private bool podeDash = true;
    [SerializeField] private bool estaDashando = false;
    private TrailRenderer tr;
    private float gravidadeNormal;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
        gravidadeNormal = rb.gravityScale;
    }


    private void FixedUpdate()
    {
        // n�o permite virar nem pular nem nada enquanto est� no dash
        if (estaDashando) return;

        // movimentacao
        if (podeMover) rb.velocity = new Vector2(horizontal * velocidade, rb.velocity.y);

        // trigando anima��o de correr
        if (!estaAtacando) {
            if (rb.velocity.x != 0) AnimacaoCorrer();
            else AnimacaoIdle();
        }

        // Flip
        if (!olharDireita && horizontal > 0) Virar();
        else if (olharDireita && horizontal < 0) Virar();

        // Coyote Efect e reset do ataque a�reo
        if (EstaNoChao()) 
        { 
            contadortempoCoyote = tempoCoyote;
            contadorGolpesAereos = 0;
        } 
        else contadortempoCoyote -= Time.deltaTime;


        // Aumentando gravidade do pulo
        if (rb.velocity.y < 0) rb.velocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;

    }
    // ===========================================================================      MOVIMENTA��O        ===================================================================================
    public void Movimentacao(InputAction.CallbackContext context) 
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    // ============================================================================      PULO        ===========================================================================================
    public void Pular(InputAction.CallbackContext context) 
    {
        
        if (context.performed && contadortempoCoyote > 0) 
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            AnimacaoPular();
            rb.velocity = new Vector2 (rb.velocity.x, forcaDoPulo);
            ContadorDePulos++;
        }
        if (context.canceled && rb.velocity.y > 0f ) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            ContadorDePulos++;
            contadortempoCoyote = 0;
            Debug.Log(ContadorDePulos);
        }
        
    }

    // ===========================================================================      ATAQUE       ===========================================================================================
    private IEnumerator AtrasoAtaque() 
    {
        yield return new WaitForSeconds(atrasoAtaque);
        podeBater = true;
        estaAtacando = false;
        podeMover = true;
        escudo.SetActive(false);
    }

    //  
    //  Dois poss�veis tipos de Ataques -> No ch�o e a�reo
    //  No ch�o � poss�vel fazer um combro de 3 ataques
    //  No ar s� � poss�vel dar um ataque 
    //

    
    public void Ataque(InputAction.CallbackContext context) 
    {
        estaAtacando = true;
        podeMover = false;

        if (context.canceled && EstaNoChao() && podeBater) 
        {
            Collider2D[] ataque = Physics2D.OverlapCircleAll(pontoAtaque.position, alcanceAtaque, camadaInimigos );
            foreach (Collider2D inimigo in ataque) 
            {
                Debug.Log("acertou um inimigo");
            }
            Collider2D[] parry = Physics2D.OverlapCircleAll(pontoAtaque.position, alcanceAtaque, camadaBala);
            foreach (Collider2D bala in parry) 
            {
                bala.GetComponent<Bala>().Refletida();
            }
            escudo.SetActive(true);
            podeBater = false;
            AnimacaoAtaque();
            contadorGolpesChao++;
            StartCoroutine(AtrasoAtaque());
        } 
        else if (context.canceled && !EstaNoChao() && podeBater && contadorGolpesAereos < 1) 
        {
            Collider2D[] ataque = Physics2D.OverlapCircleAll(pontoAtaque.position, alcanceAtaque, camadaInimigos);
            foreach (Collider2D inimigo in ataque)
            {
                Debug.Log("acertou um inimigo");
            }
            Collider2D[] parry = Physics2D.OverlapCircleAll(pontoAtaque.position, alcanceAtaque, camadaBala);
            foreach (Collider2D bala in parry) 
            {
                bala.GetComponent<Bala>().Refletida();
            }
            escudo.SetActive(true);
            podeBater = false;
            contadorGolpesAereos++;
            AnimacaoAtaque();
            StartCoroutine(AtrasoAtaque());
        }
    }

    // ===========================================================================      DASH        =============================================================================================
    public void Dashar(InputAction.CallbackContext context) 
    {
        if ( context.performed && podeDash) StartCoroutine(Dash());
    }

    private IEnumerator Dash() 
    {
        podeDash = false;
        estaDashando = true;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(  transform.localScale.x * forcaDash, 0f);
        tr.emitting = true;
        AnimacaoDash();
        yield return new WaitForSeconds(duracaoDash);
        tr.emitting = false;
        rb.gravityScale = gravidadeNormal;
        estaDashando = false;
        yield return new WaitForSeconds(esperaDash);
        podeDash = true;

    }
    // ===========================================================================      Morte                         ============================================================================

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Bala" || other.gameObject.tag == "Laser" ) Morte();
    }

    private void Morte()
    {
        Respawn.instance.RespawnPlayer(player);
    }

    // ===========================================================================      SUPORTE E VERIFICA��ES      ============================================================================
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(pontoAtaque.position, alcanceAtaque);
    }

    private bool EstaNoChao()
    {
        if (Physics2D.OverlapCircle(checkChao.position, 0.1f, camadaChao)) {
            ContadorDePulos = 0;
            if (!estaAtacando) {
                if (rb.velocity.x != 0) AnimacaoCorrer();
                else AnimacaoIdle();
            }
        } 
        return Physics2D.OverlapCircle(checkChao.position, 0.1f, camadaChao);
    }

    private void Virar() 
    {
        olharDireita = !olharDireita;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    // ===========================================================================        ANIMA��ES        =====================================================================================
    private void AnimacaoAtaque() 
    {
        if (EstaNoChao())
        {
            if (contadorGolpesChao == 1)
            {
                anim.Play("Atk1");// anima��o do golpe 1
            }
            else if (contadorGolpesChao == 2)
            {
                anim.Play("Atk2");// anima��o do golpe 2
            }
            else if (contadorGolpesChao == 3)
            {
                anim.Play("Atk3");// anima��o do golpe 3
                contadorGolpesChao = 0;
            }
        }
        else 
        { 
            if (contadorGolpesAereos == 1) 
            {
                anim.Play("AtkJump");// anima��o golpe a�reo
            }
        }

    }

    private void AnimacaoDash() 
    {
        anim.Play("Dash");// anima��o do Dash
    }

    private void AnimacaoCorrer()
    {
        if (podeMover) anim.Play("Run"); // anima��o de correr
    }

    private void AnimacaoPular()
    {
        anim.Play("Jump");// anima��o de pular
    }

    private void AnimacaoIdle() 
    {
        anim.Play("Idle");// Anima��o idle
    }
}

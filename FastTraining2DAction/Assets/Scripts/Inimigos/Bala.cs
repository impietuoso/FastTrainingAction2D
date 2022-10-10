using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float velocidade = 20f;

    public Transform playerPosicao;
    private int direcao = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if ((rb.transform.position.x - playerPosicao.position.x) < 0) direcao = 1;
        else direcao = -1;

        rb.velocity = new Vector2(direcao * velocidade, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(Desabilitar());
    }

    private IEnumerator Desabilitar() 
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }

}

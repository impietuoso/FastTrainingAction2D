using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoPlaceHolder : MonoBehaviour
{
    [Header("movimentação")]
    private float movimentacaoX;
    public float velocidade = 5;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movimentacaoX = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movimentacaoX * velocidade, 0);
    }
}

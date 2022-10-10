using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoMetralhadora : MonoBehaviour
{
    public Transform posicaoBala;

    private bool podeAtirar = true;
    [SerializeField] private bool recarregando = true;

    private int contador = 0;

    private void Update()
    {
        if (!recarregando) if (podeAtirar) StartCoroutine(Atirar());

        if (contador == 20) StartCoroutine(Recarregar());
    } 

    private IEnumerator Atirar()
    {
        podeAtirar = false;
        GameObject bala = BalasPool.instance.PegarBala();

        if (bala != null)
        {
            bala.transform.position = posicaoBala.position;
            bala.SetActive(true);
        }
        contador++;
        yield return new WaitForSeconds(0.25f);
        podeAtirar = true;
    }

    private IEnumerator Recarregar()
    {
        recarregando = true;
        contador = 0;
        yield return new WaitForSeconds(5f);
        recarregando = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoComum : MonoBehaviour
{
    public Transform posicaoBala;

    private bool podeAtirar = false;

    private void Update()
    {
        if(podeAtirar) StartCoroutine(Atirar());
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

        yield return new WaitForSeconds(1);
        podeAtirar = true;
    }
}

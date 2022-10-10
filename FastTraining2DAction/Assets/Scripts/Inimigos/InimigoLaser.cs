using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoLaser : MonoBehaviour
{
    public Transform posicaoLaser;

    [SerializeField] private bool podeAtirar = true;
    [SerializeField] private bool recarregando = false;

    private void Update()
    {
        if (!recarregando) if (podeAtirar) StartCoroutine(Atirar());
    }

    private IEnumerator Atirar()
    {
        
        podeAtirar = false;
        GameObject laser = BalasPool.instance.PegarLaser();

        if (laser != null)
        {
            laser.transform.position = posicaoLaser.position - new Vector3(4.5f, 0, 0);
            laser.SetActive(true);
        }
        
        yield return new WaitForSeconds(1f);
        laser.SetActive(false);
        StartCoroutine(AtirarLaserGrande());
        
    }
    private IEnumerator AtirarLaserGrande() 
    {
        GameObject laserGrande = BalasPool.instance.PegarLaserGrande();

        if (laserGrande != null)
        {
            laserGrande.transform.position = posicaoLaser.position - new Vector3(4.5f, 0, 0);
            laserGrande.SetActive(true);
        }

        yield return new WaitForSeconds(4);
        laserGrande.SetActive(false);
        StartCoroutine(Recarregar());
    }
    private IEnumerator Recarregar()
    {
        recarregando = true;
        yield return new WaitForSeconds(5f);
        podeAtirar = true;
        recarregando = false;
    }
}

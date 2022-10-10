using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalasPool : MonoBehaviour
{
    public static BalasPool instance;

    [Header("Bala")]
    public GameObject balaPrefab;
    private List<GameObject> balasPool = new List<GameObject>();
    private int qtd = 00;

    [Header("Laser")]
    public GameObject laserPrefab;
    private List<GameObject> laserPool = new List<GameObject>();
    private int qtdLaser = 3;
    public GameObject laserGrandePrefab;
    private List<GameObject> laserGrandePool = new List<GameObject>();
    private int qtdLaserGrande = 3;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < qtd; i++) 
        {
            GameObject bala = Instantiate(balaPrefab);
            bala.SetActive(false);
            balasPool.Add(bala);
        }

        for (int i = 0; i < qtdLaser; i++)
        {
            GameObject laser = Instantiate(laserPrefab);
            laser.SetActive(false);
            laserPool.Add(laser);
        }
        for (int i = 0; i < qtdLaserGrande; i++)
        {
            GameObject laserGrande = Instantiate(laserGrandePrefab);
            laserGrande.SetActive(false);
            laserGrandePool.Add(laserGrande);
        }
    }

    public GameObject PegarBala() 
    {
        for (int i = 0; i < balasPool.Count; i++) 
        {
            if (!balasPool[i].activeInHierarchy) return balasPool[i];
        }

        return null;
    }

    public GameObject PegarLaser()
    {
        for (int i = 0; i < laserPool.Count; i++)
        {
            if (!laserPool[i].activeInHierarchy) return laserPool[i];
        }

        return null;
    }
    public GameObject PegarLaserGrande()
    {
        for (int i = 0; i < laserGrandePool.Count; i++)
        {
            if (!laserGrandePool[i].activeInHierarchy) return laserGrandePool[i];
        }

        return null;
    }
}

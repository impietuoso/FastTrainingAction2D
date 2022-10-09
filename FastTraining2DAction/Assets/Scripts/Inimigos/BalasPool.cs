using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalasPool : MonoBehaviour
{
    public static BalasPool instance;

    private List<GameObject> balasPool = new List<GameObject>();
    private int qtd = 100;

    public GameObject balaPrefab;

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
    }

    public GameObject PegarBala() 
    {
        for (int i = 0; i < balasPool.Count; i++) 
        {
            if (!balasPool[i].activeInHierarchy) return balasPool[i];
        }

        return null;
    }
}

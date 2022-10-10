using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public static Respawn instance;

    public GameObject playerPrefab;
    public Transform[] pontosSpawn = new Transform[15];
    private Transform spawnAtual;
    public int contadorFase = 0;

    private void Awake()
    {
        if (instance != null) instance = this;
    }
    

    public void RespawnPlayer(GameObject player)
    {
        spawnAtual = pontosSpawn[contadorFase];
        player.SetActive(false);
        player.transform.position = spawnAtual.position;
        player.SetActive(true);

    }

    private void OnEnable()
    {
        // animação vivo
    }

    private void OnDisable()
    {
        // animacao morto
    }
}

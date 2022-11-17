using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ShooterGameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, Vector2.zero, Quaternion.identity);
    }

   
}

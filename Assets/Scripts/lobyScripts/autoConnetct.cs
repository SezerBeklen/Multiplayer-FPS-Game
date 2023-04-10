using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class autoConnetct : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    
    public void JoinLocal()
    {
        networkManager.networkAddress = "localhost";
        networkManager.StartClient();
    } 
    
    public void Host()
    {

        networkManager.StartHost();
    }
}

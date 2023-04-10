using UnityEngine;
using Mirror;



public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayer = "RemotePlayer";

    [SerializeField]
    private GameObject playerUIprefab;

    [HideInInspector] public GameObject playerUIýnstance;





    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponent();
            AssignRemoteLayer();
           
        }
        else
        {
            playerUIýnstance = Instantiate(playerUIprefab);
            GetComponent<Player>().Setup();
        }


        

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
     
        GameManager.RegisterPlayer(netId,player);
    }

    public void DisableComponent()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }

    

    }

    public void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayer);
    }



    private void OnDisable()
    {
        Destroy(playerUIýnstance);
        if (isLocalPlayer)
        {
            GameManager._Instance.SetSceneCameraActive(true);
        }
        GameManager.UnregisterPlayer(transform.name);
    }









}

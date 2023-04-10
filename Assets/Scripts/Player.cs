using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;

    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }



    [SerializeField]
    private float maxHealth = 1;
    
    [SyncVar]
    public float currentHealth;

    [SerializeField]
    private Behaviour[] disableOndeath;

  

    private bool[] wasEnabledOnstart;
    private bool firstSetup = true;

    
    
    public void Setup()
    {
        if (isLocalPlayer)
        {
            GetComponent<PlayerSetup>().playerUIýnstance.SetActive(true);

        }
       
        CmdBroadcastNewPlayerSetup();
    }

    [Command]
    void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }
    [ClientRpc]
    void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabledOnstart = new bool[disableOndeath.Length];

            for (int i = 0; i < disableOndeath.Length; i++)
            {
                wasEnabledOnstart[i] = disableOndeath[i].enabled;
            }
            firstSetup = false;
        }
       
        SetDefaults();
    }


    private void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        if (isLocalPlayer)
        {
            PlayerUI.instanceUI.fill.fillAmount = 1;
        }
        
        for (int i = 0; i < disableOndeath.Length; i++)
        {
            disableOndeath[i].enabled = wasEnabledOnstart[i];
        }

      
        Collider col = GetComponent<Collider>();
        if(col != null)
        {
            col.enabled = true;
        }
  

    }


     private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);

        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);
        Setup();

        
    }

    [ClientRpc]
    public void RpcTakeDamage(float amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= amount;
        if (isLocalPlayer)
        {
            PlayerUI.instanceUI.fill.fillAmount = currentHealth;
            
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log(transform.name + "now" + currentHealth + " live points");
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOndeath.Length; i++)
        {
            disableOndeath[i].enabled = false;

        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log(transform.name + "Died");

        StartCoroutine(Respawn());

    }





}//clas

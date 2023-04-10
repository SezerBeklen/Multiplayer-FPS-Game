using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ShotScript : NetworkBehaviour
{
    public Camera fpsCam;
    public MouseLook mouse;
    public float range;
    public float fireRate;
    public bool canFire = true;
    public LayerMask mask;
    

    [SerializeField]
    public PlayerWeapon weapon;

    [SerializeField]
    private Animator anim;

    [Header("Recoil")]
    private float Vrecoil = 0.1f;
    private float Hrecoil = 0.1f;

    [Header("EFFECTS")]
    public ParticleSystem MuzzleFlash;
    public GameObject[] BulletEffect;

    [Header("SOUNDS")]
    public AudioSource reloadSound;
    public AudioSource bodyHitSource;
    public AudioSource ZeroAmmoSound;
    public AudioSource weaponSounds;
     
    [Header("Ammo Counter")]
    public float ammoDeposit;
    public float ammo;
    public float Maxammo;
    private float timeToFire;

    public void Update()
    {
        PlayerUI.instanceUI.AmmoText.text = +ammo + "\n" + ammoDeposit;
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= timeToFire && canFire)
        {
            shoot();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {          
            mouse.AddRecoil(0, 0);
        }
        #region CamZoom
        if (Input.GetKey(KeyCode.Mouse1))
        {
            fpsCam.fieldOfView = 40;
        }
        else
        {
            fpsCam.fieldOfView = 60;
        }
        #endregion
        
        #region ReloadandChekFire
        if (ammoDeposit != 0 && ammo != 40) 
        {
            
            if (Input.GetKeyDown(KeyCode.R))
            {
              
                canFire = false;
                StartCoroutine(Reload());


                return;
            }
           

        
          
        }
        #endregion
       
    }


    [Command]
    void CmdOnhit2(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffects2(pos, normal);
    }

    //tüm oyuncularda bullet efekti oluþur
    [ClientRpc]
    void RpcDoHitEffects2(Vector3 pos, Vector3 normal)
    {
        Instantiate(BulletEffect[0], pos, Quaternion.LookRotation(normal));

    }



    [Command]
    void CmdOnhit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffects(pos,normal);
    }

    //tüm oyuncularda bullet efekti oluþur
    [ClientRpc]
    void RpcDoHitEffects(Vector3 pos, Vector3 normal)
    {
        Instantiate(BulletEffect[1], pos, Quaternion.LookRotation(normal));
        if (isLocalPlayer)
        {
            bodyHitSource.Play();
        }
        
    }


    //Sunucuyu ateþ etme komutu hakkýnda bilgilendirme
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffects();
    }

    //tüm oyuncularda ateþ efekti oluþur
    [ClientRpc]
    void RpcDoShootEffects()
    {
        MuzzleFlash.Play();
        if (isLocalPlayer)
        {
            weaponSounds.Play();
        }
        
    }
    

    [Client]
    public void shoot()
    {
       
        if (ammo <= 0 )
        {
             
            canFire = false;
            ZeroAmmoSound.Play();
        }
        else
        {
            canFire = true;
            ammo--;
        }
        
        
        if (ammo > 0)
        {
            timeToFire = Time.time + 1f / fireRate;

            
            float h = Random.Range(-Hrecoil, Hrecoil);
            mouse.AddRecoil(h, Vrecoil);

            if (!isLocalPlayer)
            {
                return;
            }

            CmdOnShoot();
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, mask))
            {
                
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    CmdOnhit(hit.point, hit.normal);
                    
                    CmdPlayerShot(hit.collider.name, weapon.damage);
                }
                else
                {
                    CmdOnhit2(hit.point, hit.normal);
                    
                }


            }

        }

        if (ammo == 0)
        {
            mouse.AddRecoil(0, 0);
        }


    }

    [Command]
    private void CmdPlayerShot(string playerId, float damage)
    {

        Debug.Log(playerId + " touched ");
        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage);

    }

    IEnumerator Reload()
    {

        reloadSound.Play();
        CmdOnReload();
        
        
        yield return new WaitForSeconds(0f);
        canFire = true;
        if (ammoDeposit ==0 && ammo == 0)
        {
            canFire = false;

        }
            
        float ammoToAdd;
        ammoToAdd = Maxammo - ammo;

        if ( ammoDeposit <= ammoToAdd)
        {
            ammo += ammoDeposit;
            ammoDeposit = 0;
        }
        else if (  ammoDeposit> ammoToAdd)
        {
            ammoDeposit -= ammoToAdd;
            ammo += ammoToAdd;  
        }
    }

    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload()
    {
       
        anim.SetTrigger("reload");
        
        
    }
}

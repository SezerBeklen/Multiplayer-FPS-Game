using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class fpsControler : NetworkBehaviour
{
    
    private AudioSource source;
    private Vector3 velocity;
    private float timer;
    private Rigidbody rb;

    
    public AudioClip[] StepSounds;
    public AudioSource Jumpsource_;
    public bool isMoving;
    public bool ÝsGrounded;
    public float distance = 0.3f;
    public float Jumpspeed;
    public int _Speed;
    public float JumpHeight;
    public float gravity;
    public float timeBetweenSteps;
    public Animator movementAnimator;
    public Transform ground;
    public LayerMask mask;
   






    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
       Cursor.lockState = CursorLockMode.Locked;

      

       
    }

    public void Update()
    {
       
      if (!isLocalPlayer)
       {
            
            return;
            
       }
        
        #region Movement

        float horizontal = Input.GetAxis("Horizontal") * _Speed;
        float vertical = Input.GetAxis("Vertical") * _Speed;

        Vector3 moveH = transform.right * horizontal ;
        Vector3 moveV = transform.forward * vertical ;
        Vector3 velo = (moveH + moveV);
        rb.AddForce(velo * _Speed);
        #region AnimatiomMovementControl
        if (horizontal > 0)
        {
            movementAnimator.SetFloat("Horizontal", 1);
        }
        else if (horizontal < 0) 
        {
            movementAnimator.SetFloat("Horizontal", -1);
        }
        else if (horizontal == 0)
        {
            movementAnimator.SetFloat("Horizontal", 0);
        }
        if (vertical > 0)
        {
            movementAnimator.SetFloat("Vertical", 1);
        }
        else if (vertical < 0)
        {
            movementAnimator.SetFloat("Vertical", -1);
        } 
        else if (vertical == 0)
        {
            movementAnimator.SetFloat("Vertical", 0);
        }
        #endregion

        #endregion

        #region FootSteps

        if (horizontal != 0 || vertical != 0)
            isMoving = true;
        else
            isMoving = false;
        if (isMoving)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = timeBetweenSteps;

                source.clip = StepSounds[Random.Range(0, StepSounds.Length)];
                source.pitch = Random.Range(0.85f, 1.15f);
                source.Play();
                if (!ÝsGrounded)
                    source.Stop();
            }

        }

        else
        {
            timer = timeBetweenSteps;
        }
 

        #endregion

        #region Jump 
        if (Input.GetKeyDown(KeyCode.Space) && ÝsGrounded)
        {
            
            velocity.y += Mathf.Sqrt(JumpHeight * -3.0f * gravity);
            Jumpsource_.Play();
            movementAnimator.SetBool("jump", true);
        }
        else if(Input.GetKeyUp(KeyCode.Space) && !ÝsGrounded)
        {
            movementAnimator.SetBool("jump", false);
        }

        #endregion

        #region Gravity 
        ÝsGrounded = Physics.CheckSphere(ground.position, distance, mask);
        if(ÝsGrounded && velocity.y < 0)
        {
            velocity.y = 0f;

        }

        velocity.y += gravity * Time.deltaTime;
        rb.AddForce(velocity* Jumpspeed);
        #endregion

    }


    
    
   
    



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] Canvas deathCanvas;
    [SerializeField] AudioClip DeathSOund;
    [SerializeField] AudioClip JumpSound;

    // States 
    bool isAlive = true;

    // Cached component references
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    float GravityScaleInit;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        GravityScaleInit = myRigidbody.gravityScale;
        deathCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        Run();
        Jump();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void Run()
    {
        float controlThrow = Input.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        print(playerVelocity);
        bool HorizSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("running", HorizSpeed);
    }

    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("ground")))
        {
            myAnimator.SetBool("climb", false);
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            AudioSource.PlayClipAtPoint(JumpSound, Camera.main.transform.position);
            Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
             myRigidbody.velocity += jumpVelocity;
        }
    }

    private void ClimbLadder()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("ladders")))
        {
            myAnimator.SetBool("climb", false);
            myRigidbody.gravityScale = GravityScaleInit;
            return;
            
        }
        
        float controlThrow = Input.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, controlThrow * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;
        bool VertSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("climb", VertSpeed);
        
    }

    private void FlipSprite()
    {
        bool HorizSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (HorizSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    private void Die()
    {
        
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("enemy", "hazards")))
        {
            AudioSource.PlayClipAtPoint(DeathSOund, Camera.main.transform.position);
            deathCanvas.enabled = true;
            isAlive = false;
            myAnimator.SetTrigger("dead");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }

        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("water")))
        {
            AudioSource.PlayClipAtPoint(DeathSOund, Camera.main.transform.position);
            deathCanvas.enabled = true;
            isAlive = false;
            myAnimator.SetTrigger("dead");
            myRigidbody.gravityScale = 0.009f;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    public Text life;

    private int scoreValue = 0;
    private int liveValue = 3;
    private bool facingRight = true;
    private bool isOnGround;

    public GameObject winTextObject;
    public GameObject loseTextObject;
    public AudioClip victoryMusic;
    public AudioClip defeatMusic;
    public AudioClip backgroundMusic;
    public AudioSource musicSource;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        musicSource.clip = backgroundMusic;
        musicSource.Play();
        musicSource.loop = true;
        SetCountText();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if(facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if(facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if(isOnGround == true)
        {
            anim.SetInteger("State", 0);
        }
        else if(isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void SetCountText()
    {
        score.text = "Score: " + scoreValue.ToString();
        life.text = "Lives: " + liveValue.ToString();


        if(scoreValue == 8)
        {
           winTextObject.SetActive(true);
           musicSource.loop = false;
           musicSource.clip = victoryMusic;
           musicSource.Play();
           musicSource.Stop();
        }
        else if(liveValue == 0)
        {
            loseTextObject.SetActive(true);
            musicSource.loop = false;
            musicSource.clip = defeatMusic;
            musicSource.Play();
            Destroy(gameObject, 5);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            Destroy(collision.collider.gameObject);

            SetCountText();

            if(scoreValue == 4)
            {
                transform.position = new Vector3(61.6f, 0.0f, 0.0f);
                liveValue = 3;
                SetCountText();
            }
        }
        else if (collision.collider.tag == "Enemy")
        {
            liveValue -= 1;
            Destroy(collision.collider.gameObject);

            SetCountText();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors.  You can also create a public variable for it and then edit it in the inspector.
            }
        }
    }
}
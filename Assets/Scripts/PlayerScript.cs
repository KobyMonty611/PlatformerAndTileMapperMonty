using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    //Text Objects
    public Text scoreText;
    public Text livesText;
    public GameObject winTextObject;
    public GameObject loseTextObject;

    //Interger Values
    private int scoreValue = 0;
    private int lives;
    private int i = 0;

    //Sound and Animation
    public AudioClip backgroundMusic; //Basic
    public AudioClip winSound; //Win
    public AudioClip loseSound; //Lose
    public AudioSource musicSource;
    public AudioSource resultSource;

    //Animation
    Animator anim;
    private bool facingRight = true;

    //GroundCheck
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        scoreText.text = "Score: " + scoreValue.ToString();
        lives = 3;
        livesText.text = "Lives: " + lives.ToString();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        musicSource.clip = backgroundMusic;
        musicSource.Play();
        musicSource.loop = true;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //MOVEMENT UPDATES
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        //FLIPPING ANIMATIONS
         if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
            else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }


        //ANIMATIONS
        if(vertMovement > 0 && isOnGround== false)
        {
            anim.SetInteger("State", 1);
        }
        else if(hozMovement != 0 && isOnGround == true)
        {
            anim.SetInteger("State", 2);
        }
        else if(vertMovement == 0 && isOnGround == true)
        {
            anim.SetInteger("State", 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //COIN AND ENEMY FUNCTIONS
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            scoreText.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }
        else if (collision.collider.tag == "Enemy")
        {
            lives = lives - 1;
            livesText.text = "Lives: " + lives.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (scoreValue >= 8)
        {
            // Set the text value of your 'winText'
            winTextObject.SetActive(true);
            rd2d.gameObject.SetActive(false);
            musicSource.Stop();

            //Play victory sound
            resultSource.clip = winSound;
            resultSource.Play();
        }


        //LIVES AND SCORE FUNCTIONS
        if (lives == 0)
        {
            // Set the text value of your 'loseText'
            loseTextObject.SetActive(true);
            winTextObject.SetActive(false);
            rd2d.gameObject.SetActive(false);
            musicSource.Stop();

            //Play victory sound
            resultSource.clip = loseSound;
            resultSource.Play();
        }

        if (scoreValue == 4 && i == 0)
        {
            transform.position = new Vector3(100.0f, 3.0f, 0.0f);
            lives = 3;
            livesText.text = "Lives: " + lives.ToString();
            i++;
        }

    }

    //FUNCTION FOR FLIPPING SPRITES
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetInteger("State", 1);
            }
        }
    }

}
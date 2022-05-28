using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    public Transform enemyLocation;
    public Text scoreText;
    private int score = 0;
    private bool countScoreState = false;
    public CharacterController2D controller;
    private Animator marioAnimator;
    private bool faceRightState = true;
    private AudioSource marioAudioSource;

    float horizontalMove = 0f;
    bool jump = false;

    public float runSpeed = 40f;
    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        marioAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!onGroundState && countScoreState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {
                countScoreState = false;
                score++;
            }
        }
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;  // amplify speed by runSpeed

        if (Input.GetButtonDown("Jump")){
            onGroundState = false;
            countScoreState = true; //check if Gomba is underneath
            jump = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;

            if(Mathf.Abs(marioBody.velocity.x) > 1.0){
                marioAnimator.SetTrigger("onSkid");
            }
        }
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;

            if(Mathf.Abs(marioBody.velocity.x) > 1.0){
                marioAnimator.SetTrigger("onSkid");
            }
        }

        // mapping mario's speed to xSpeed
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }
    // FixedUpdate may be called once per frame. See documentation for details.
    void  FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime,false,jump);
        jump = false;
    }
    private bool onGroundState = true;

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGroundState = true; // back on ground
            countScoreState = false; // reset score state
            scoreText.text = "Score: " + score.ToString();
            marioAnimator.SetBool("onGround", onGroundState);
        };
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void playJumpSound(){
        marioAudioSource.PlayOneShot(marioAudioSource.clip);
    }
}

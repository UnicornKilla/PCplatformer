using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerCntrl : MonoBehaviour {

    public float speed = 5.0f;
    public float jumpforce = 1.0f;
    public Rigidbody2D rb;
    public Animator charAnim;
    public SpriteRenderer sprite;
    bool OnGround;
    public int lives = 3;
    public Text countText;
    private int count;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        charAnim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start ()
    {
        count = 0;
        setCount();
	}
    
    void setCount()
    {
        countText.text = count.ToString();
    }

    void Move()
    {
        Vector3 tempvector = Vector3.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + tempvector, speed * Time.deltaTime);
        if (tempvector.x<0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpforce, ForceMode2D.Impulse);
    }
    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        OnGround = colliders.Length > 1;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            Destroy(other.gameObject);
            count ++;
            setCount();
        }

        if (other.tag == "Enemy")
        {
           lives--;
            setLose();
        }

        if (other.tag == "Life")
        {
            lives++;
            Destroy(other.gameObject);
        }
    }
    private void FixedUpdate()
    {
        CheckGround();
    }

    void Update ()
    {
		if (Input.GetButton("Horizontal"))
        {
            Move();
            charAnim.SetInteger("State", 1);
        }
        if (OnGround && Input.GetButton("Jump"))
        {
            Jump();
            charAnim.SetInteger("State", 2);
        }
        if (!Input.anyKey)
        {
            charAnim.SetInteger("State", 0);
        }
	}

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 50), "Life = "  + lives);
    }

    void setLose()
    {
        if (lives == 0)
        {
            SceneManager.LoadScene("Lose");
        }
    } 

}

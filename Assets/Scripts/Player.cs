using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    public Transform groundCheck;
    bool isGrounded;
    Animator anim;
    int currHp;
    int maxHp = 3;
    public Main main;
    bool isHit = false;
    public bool blueKey = false;
    bool canTP = true;
    public bool inWater = false;


    // Перед первым кадром
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currHp = maxHp;
    }

    // Каждый кадр
    void Update()
    {
        Jump();
        if (inWater)
        {
            anim.SetInteger("State", 4);
            if (Input.GetAxis("Horizontal") != 0)
            {
                Flip();
            }
        }
        else
        {
            CheckGround();
            if (Input.GetAxis("Horizontal") == 0 && isGrounded)
            {
                anim.SetInteger("State", 1);
            }
            else
            {
                Flip();
                if (isGrounded)
                {
                    anim.SetInteger("State", 2);
                }
            }
        }
    }

    // Физика
    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }

    // Поворот
    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
        }

        if (!isGrounded)
            anim.SetInteger("State", 3);

    }

    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
    }

    public void RecountHp(int deltaHp)
    {
        currHp += deltaHp;
        print(currHp);
        if (deltaHp < 0)
        {
            StopCoroutine(OnHit());
            isHit = true;
            StartCoroutine(OnHit());
        }
        if (currHp <= 0)
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }

    IEnumerator OnHit()
    {
        if (isHit)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f,
            GetComponent<SpriteRenderer>().color.g - 0.04f,
            GetComponent<SpriteRenderer>().color.b - 0.04f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f,
            GetComponent<SpriteRenderer>().color.g + 0.04f,
            GetComponent<SpriteRenderer>().color.b + 0.04f);
        }

        if (GetComponent<SpriteRenderer>().color.g <= 0)
        {
            isHit = false;
        }
        if (GetComponent<SpriteRenderer>().color.g >= 1)
        {
            StopCoroutine(OnHit());
        }

        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlueKey")
        {
            Destroy(collision.gameObject);
            blueKey = true;
        }
        if (collision.gameObject.tag == "Door")
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTP)
            {
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                canTP = false;
                StartCoroutine(TPWait());
            }
            else if (blueKey)
            {
                collision.gameObject.GetComponent<Door>().Unlock();
            }
        }
    }

    IEnumerator TPWait()
    {
        yield return new WaitForSeconds(1f);
        canTP = true;
    }
}

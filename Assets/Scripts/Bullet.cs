using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 3f;
    private float TimeToDisable = 10f;


    void Start()
    {
        StartCoroutine(SetDisable());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopCoroutine(SetDisable());
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime); 
    }

    IEnumerator SetDisable()
    {
        yield return new WaitForSeconds(TimeToDisable);
        gameObject.SetActive(false);
    }
}

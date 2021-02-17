using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barnacle : MonoBehaviour
{
    public float speed = 4f;
    bool isWait = false;
    bool isHidden = false;
    public float waitTime = 4f;
    public Transform point;

    void Start()
    {
        point.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }


    void Update()
    {
        if (!isWait)
        {
            transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
            if (transform.position == point.transform.position)
            {
                if (isHidden)
                {
                    point.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                    isHidden = false;
                }
                else
                {
                    point.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                    isHidden = true;
                }
                isWait = true;
                StartCoroutine(Waiting());
            }
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        isWait = false;
    }
}
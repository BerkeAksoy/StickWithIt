using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    private Vector3 startPos, endPos;
    public float SPEED;
    private bool switching;


    void Start()
    {
        startPos = transform.position;
        endPos = startPos + Vector3.right * 8;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (switching)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, SPEED * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, SPEED * Time.deltaTime);
        }

        if (transform.position == startPos)
        {
            switching = true;
        }
        else if (transform.position == endPos)
        {
            switching = false;
        }
    }
}

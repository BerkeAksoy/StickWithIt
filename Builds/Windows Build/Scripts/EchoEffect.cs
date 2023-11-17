using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{

    private float spawnRate = 0.2f, spawnTimer = 0;
    private Ball ball;
    public GameObject echo;
    // Start is called before the first frame update
    void Start()
    {
        ball = GetComponent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ball.CanMove)
        {
            PlayEcho();
        }
    }

    private void PlayEcho()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnRate)
        {
            GameObject echoIns = Instantiate(echo, transform.position, Quaternion.identity);
            Destroy(echoIns, 1f);
            spawnTimer = 0;
        }
    }
}

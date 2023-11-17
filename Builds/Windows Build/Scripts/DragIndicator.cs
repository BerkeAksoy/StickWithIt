using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIndicator : MonoBehaviour
{

    Vector3 startPos, startDrawPos, endPos, endDrawPos, camOffset;
    private Camera drawCam, mainCam;
    LineRenderer lineRenderer;
    private float maxDist = 2f, forceMul = 4f;
    private static Vector3 force;
    private static float activeDist;
    private int dif;
    private float t = 0;
    public GameObject arrow;

    private Ball ball;

    public static Vector3 Force { get => force;}
    public static float ActiveDist { get => activeDist;}

    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        drawCam = GameObject.Find("cam").GetComponent<Camera>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        camOffset = -drawCam.transform.position;

        dif = GameManager.Instance.getDifficulty();
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.IsPlayable())
        {
            DrawDragIndicator();
        }
    }

    private void DrawDragIndicator()
    {
        //Start Draw
        StartDraw();

        //Aim and Drag
        if (dif != 2)
        {
            AimAndDrag();
        }
        else
        {
            JustDrag();
        }

        //Shoot
        Shoot();
    }

    private void slerpIt()
    {
        t += Time.deltaTime / 4f; // Divided by 0.5 to make it 0.5 seconds.

        //rot = Mathf.Lerp(359f, 0f,t);
        //a = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 360), t);

        //Quaternion a = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 360), t);

        //directioner.transform.rotation = Quaternion.Euler(0,0,rot);

        arrow.transform.Rotate(0, 0, -360 * Time.deltaTime / 2);

        if (t >= 1f)
        {
            t = 0;
        }
    }

    private void StartDraw()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // refresh position
            if (dif == 2)
            {
                arrow.SetActive(true);
            }

            lineRenderer.enabled = true;
            lineRenderer.numCapVertices = 1;
            lineRenderer.positionCount = 2;
            startPos = drawCam.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            startDrawPos = mainCam.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            lineRenderer.SetPosition(0, startDrawPos);
            lineRenderer.useWorldSpace = true;

            arrow.transform.position = startDrawPos;
        }
    }

    private void AimAndDrag()
    {
        if (Input.GetMouseButton(0))
        {
            // For Force Calculating
            endPos = drawCam.ScreenToWorldPoint(Input.mousePosition) + camOffset;

            Vector3 dir = endPos - startPos;
            float dist = Mathf.Clamp(Vector3.Distance(startPos, endPos), 0, maxDist);
            endPos = startPos + (dir.normalized * dist);

            // For Drawing
            endDrawPos = mainCam.ScreenToWorldPoint(Input.mousePosition) + camOffset;

            Vector3 dirDraw = endDrawPos - startDrawPos;
            float distDraw = Mathf.Clamp(Vector3.Distance(startDrawPos, endDrawPos), 0, maxDist);
            endDrawPos = startDrawPos + (dirDraw.normalized * distDraw);

            lineRenderer.SetPosition(1, endDrawPos);

            // Force Calculation
            activeDist = Vector3.Distance(startPos, endPos);
            //          Direction
            force = (startPos - endPos) * activeDist * forceMul;
        }
    }

    private void JustDrag()
    {
        if (Input.GetMouseButton(0))
        {
            slerpIt();
            // For Force Calculating
            endPos = drawCam.ScreenToWorldPoint(Input.mousePosition) + camOffset;

            Vector3 dir = endPos - startPos;
            float dist = Mathf.Clamp(Vector3.Distance(startPos, endPos), 0, maxDist);
            endPos = startPos + (dir.normalized * dist);

            // For Drawing
            endDrawPos = mainCam.ScreenToWorldPoint(Input.mousePosition) + camOffset;

            Vector3 dirDraw = endDrawPos - startDrawPos;
            float distDraw = Mathf.Clamp(Vector3.Distance(startDrawPos, endDrawPos), 0, maxDist);
            endDrawPos = startDrawPos + (dirDraw.normalized * distDraw);

            lineRenderer.SetPosition(1, endDrawPos);

            // Force Calculation
            activeDist = Vector3.Distance(startPos, endPos);

            force = arrow.transform.up * activeDist * forceMul * 2;
            Debug.Log("Arrow forward: " + arrow.transform.up + " Force: "+ force);

            if (Vector3.Distance(startPos, endPos) < 1f)
            {
                arrow.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                arrow.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonUp(0))
        {
            arrow.SetActive(false);
            lineRenderer.enabled = false;

            if (Vector3.Distance(startPos, endPos) > 1f)
            {
                
                ball.ReleaseBall(force);
            }
            else
            {
                // Cancel Shot
            }
        }
    }


}

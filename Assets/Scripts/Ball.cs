using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private Rigidbody2D myRB;
    LineRenderer trajectoryLR;
    private bool canMove, landed, canUndo, died = false;
    private Transform currMovingPlatform;
    private Vector3 beginPos;
    public ParticleSystem myPS;
    private float moveTimer = 0;
    private GameManager gm;
    private UIManager uim;
    private Vector3 previousLocation;
    private int previousJumpCount;

    public bool CanMove { get => canMove;}
    public bool CanUndo { get => canUndo;}
    public bool Died { get => died; set => died = value; }

    private void Start()
    {
        gm = GameManager.Instance;
        uim = UIManager.Instance;
        myRB = GetComponent<Rigidbody2D>();
        trajectoryLR = gameObject.GetComponent<LineRenderer>();

        beginPos = transform.position;
    }

    private void Update()
    {
        if (!died && !gm.Paused) {
            if (!canMove)
            {
                if (landed)
                {
                    canMove = true;
                    if (canUndo)
                    {
                        uim.EnableUndoButton();
                    }
                }
                else
                {
                    VelocityChecker();
                }
            }
            else
            {
                if (gm.getDifficulty() == 0)
                {
                    drawTrajectory();
                }
            }
        }
    }

    private void VelocityChecker()
    {
        if (myRB.velocity == new Vector2(0, 0) && !landed)
        {
            moveTimer += Time.deltaTime;

            if (moveTimer >= 0.5f)
            {
                moveTimer = 0;
                landed = true;
            }
        }
        else
        {
            moveTimer = 0;
        }
    }

    private void drawTrajectory()
    {
        if (Input.GetMouseButtonDown(0))
        {
            trajectoryLR.enabled = true;
            trajectoryLR.numCapVertices = 1;
            trajectoryLR.positionCount = 2;
            trajectoryLR.useWorldSpace = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (!trajectoryLR.enabled)
            {
                trajectoryLR.enabled = true;
            }

            if (DragIndicator.ActiveDist > 1.2f)
            {
                Vector2[] trajectory = plot(400);

                trajectoryLR.positionCount = trajectory.Length;

                for (int i = 0; i < trajectory.Length; i++)
                {
                    trajectoryLR.SetPosition(i, trajectory[i]);
                }
            }
            else
            {
                trajectoryLR.enabled = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            trajectoryLR.enabled = false;
        }
    }

    private Vector2[] plot(int steps)
    {
        Vector2[] results = new Vector2[steps];

        Vector2 pos = transform.position;

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        //                                // myRB.gravityScale
        Vector2 gravityAccel = Physics2D.gravity * 1 * timestep * timestep; // gt^2

        float drag = 1f - timestep * myRB.drag;
        Vector2 moveStep = DragIndicator.Force * timestep / myRB.mass;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }

    public void ReleaseBall(Vector2 force)
    {
        previousLocation = transform.position;
        previousJumpCount = gm.JumpCount;
        canUndo = true;

        myRB.AddForce(force, ForceMode2D.Impulse);
        canMove = false;
        landed = false;
        myRB.gravityScale = 1;

        gm.JumpCount--;
        gm.UpdateJumpCount();
    }

    private void Stick()
    {
        landed = true;
        myRB.gravityScale = 0;
    }

    public void Die()
    {
        died = true;
        uim.YouDied();
    }

    public void UndoMove()
    {
        transform.position = previousLocation;
        gm.JumpCount = previousJumpCount;
        gm.UpdateJumpCount();

        canUndo = false;
        uim.DisableUndoButton();
    }

    public bool IsPlayable()
    {
        if(died || gm.Paused || !canMove)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(myPS, transform.position, Quaternion.identity);

        if (collision.gameObject.tag.Equals("Mov_Sticky_Plat"))
        {
            currMovingPlatform = collision.gameObject.transform;
            transform.SetParent(currMovingPlatform);

            Stick();
        }

        if (collision.gameObject.tag.Equals("Sticky_Ground"))
        {
            Stick();
        }

        if (collision.gameObject.tag.Equals("Hazard"))
        {
            Die();
        }

        if (collision.gameObject.tag.Equals("Exit")){
            UIManager.Instance.Restart();
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag.Equals("Mov_Sticky_Plat"))
        {
            currMovingPlatform = null;
            transform.SetParent(null);
        }
    }


}

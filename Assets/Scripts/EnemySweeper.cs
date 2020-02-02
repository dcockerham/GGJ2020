using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySweeper : EnemyMinion
{
    public enum SweeperState
    {
        DELAYED,
        SWEEPING_DOWN,
        SWEEPING_UP,
        END
    }

    public Transform target;
    public Vector3 leftReturnTarget;
    public Vector3 rightReturnTarget;
    public float targetYDistance; // Distance above target.
    public float targetXVariance; // Random variance of target x position.
    public float sweepBackDistance; // Distance from target before flying back up.
    public float sweepDelay; // Delay before beginning a sweep.
    public float sweepVariance; // Random varience of delay.

    public SweeperState state;
    private float delayTime;
    private Vector3 targetVector;
    private Vector3 returnTarget;
    private float originalXSpeed;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        state = SweeperState.DELAYED;
        float delayTime = Mathf.Max(0f, sweepDelay) + Random.Range(-sweepVariance, sweepVariance);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        // Don't follow if target is destroyed.
        if (target == null)
        {
            state = SweeperState.END;
        }

        if (state != SweeperState.END)
        {
            SweeperStep();
        }
    }

    private void SweeperStep()
    {
        switch (state)
        {
            case SweeperState.DELAYED:
                {
                    delayTime -= Time.deltaTime;
                    if (delayTime <= 0)
                    {
                        // Set up sweep.
                        originalXSpeed = (moveSpeedX == 0) ? moveSpeedY : moveSpeedX;
                        targetVector = target.position;
                        targetVector.y += targetYDistance;
                        targetVector.x += Random.Range(-targetXVariance, targetXVariance);
                        returnTarget = (target.position.x > transform.position.x) ? rightReturnTarget : leftReturnTarget;
                        canMove = true;
                        ignoringBoundaries = true;
                        state = SweeperState.SWEEPING_DOWN;
                    }
                }
                break;
            case SweeperState.SWEEPING_DOWN:
                {
                    float nextX = Mathf.SmoothDamp(transform.position.x, targetVector.x, ref moveSpeedX, Time.deltaTime, Mathf.Abs(originalXSpeed));
                    float nextY = Mathf.SmoothDamp(transform.position.y, targetVector.y, ref moveSpeedY, Time.deltaTime, 6 * Mathf.Abs(originalXSpeed));
                    if (Vector3.Distance(targetVector, new Vector3(nextX, nextY)) <= sweepBackDistance)
                    {
                        state = SweeperState.SWEEPING_UP;
                        ignoringBoundaries = false;
                    }
                }
                break;
            case SweeperState.SWEEPING_UP:
                {
                    float nextX = Mathf.SmoothDamp(transform.position.x, returnTarget.x, ref moveSpeedX, Time.deltaTime);
                    float nextY = Mathf.SmoothDamp(transform.position.y, returnTarget.y, ref moveSpeedY, Time.deltaTime);
                    if (Vector3.Distance(returnTarget, new Vector3(nextX, nextY)) <= Mathf.Epsilon)
                    {
                        state = SweeperState.END;
                        moveSpeedX = originalXSpeed;
                    }
                }
                break;
        }
    }
}

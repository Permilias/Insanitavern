using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    Rigidbody rb;
    Collider col;
    Vector3 currentTarget;

    public float speed;
    public float detectionDistance;
    public float detectionAngle;
    public float tryAngle;

    LayerMask mask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        dirs = new Vector2[5];
        rayDirs = new Vector3[5];
        rays = new Ray[5];

        mask = LayerMask.GetMask("Obstacle"); ;
    }

    public void SetTarget(Vector3 _target)
    {
        currentTarget = _target;
    }



    Vector2 target, pos, diff, startPos, pushForce;
    Vector2[] dirs;
    Vector3 rayStart;
    Vector3[] rayDirs;
    RaycastHit rayHit;
    Ray[] rays;

    public Vector3 vel;

    float lowest;
    int lowestIndex;
    bool foundAngle, turnLeft;

    public void Push(Vector2 force)
    {
        print("pushed !");
        pushForce = force;
    }

    public bool debugs;
    bool turningLeft, decided;

    private void FixedUpdate()
    {
        //direction
        target = new Vector2(currentTarget.x, currentTarget.z);
        pos = new Vector2(transform.position.x, transform.position.z);
        diff = target - pos;
        dirs[0] = diff.normalized;
        if(pushForce.magnitude > 0)
        {
            if (debugs) print("applying pushforce " + pushForce.x + "," + pushForce.y);
            dirs[0] += pushForce;
            pushForce = Vector2.MoveTowards(pushForce, Vector2.zero, Registry.settings.pushResist);
            
        }

        if (debugs) print("dir = " + dirs[0].x + "," + dirs[0].y);

        dirs[1] = Utility.AddRot(dirs[0], -detectionAngle*2);
        dirs[2] = Utility.AddRot(dirs[0], -detectionAngle);
        dirs[3] = Utility.AddRot(dirs[0], detectionAngle*2);
        dirs[4] = Utility.AddRot(dirs[0], detectionAngle);

        foundAngle = false;
        startPos = pos - (dirs[0]*Registry.settings.rayStartBackMult);
        rayStart = new Vector3(startPos.x, 0.01f, startPos.y);
        int tryCount = 0;
        decided = false;

        while (!foundAngle)
        {
            foundAngle = true;
            lowest = 9999;
            lowestIndex = -1;
           
            for (int i = 0; i < 5; i++)
            {
                float distance = i == 0 ? detectionDistance * 1.5f : detectionDistance;

                rayDirs[i] = new Vector3(dirs[i].x, 0.01f, dirs[i].y);
                rays[i] = new Ray(rayStart, rayDirs[i] * distance);
                Color col = Color.green;



                if (Physics.Raycast(rays[i], out rayHit, distance, mask))
                {
                    if(rayHit.distance < lowest)
                    {
                        lowest = rayHit.distance;
                        lowestIndex = i;
                    }

                    foundAngle = false;
                    col = Color.red;
                }

                Debug.DrawRay(rayStart, rayDirs[i] * distance, col, 0);
            }

            tryCount++;
            if(tryCount > 100)
            {
                Debug.Log("prout");
                foundAngle = true;
            }

            if (!foundAngle)
            {
                if(decided)
                {
                    if (turningLeft)
                    {
                        TurnDirLeft();
                    }
                    else
                    {
                        TurnDirRight();
                    }
                }
                else
                {
                    if (lowestIndex == 0)
                    {
                        if (turningLeft)
                        {
                            TurnDirLeft();
                        }
                        else
                        {
                            TurnDirRight();
                        }
                    }
                    else if (lowestIndex >= 1)
                    {
                        if (lowestIndex >= 3)
                        {

                            TurnDirRight();

                        }
                        else
                        {
                            TurnDirLeft();
                        }
                    }
                }

            }
        }

        //apply velocity
        vel = new Vector3(dirs[0].x, 0, dirs[0].y);

        //still correction
        if (diff.sqrMagnitude < 0.2f) vel = Vector3.zero;

        rb.velocity = vel * speed;

        //y correction
        if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    

    void TurnDirLeft()
    {
        for (int i = 0; i < 5; i++)
        {
            dirs[i] = Utility.AddRot(dirs[i], tryAngle);
        }

        if(!decided)
        {
            decided = true;
            turningLeft = true;
        }
    }

    void TurnDirRight()
    {
        for (int i = 0; i < 5; i++)
        {
            dirs[i] = Utility.AddRot(dirs[i], -tryAngle);
        }

        if (!decided)
        {
            decided = true;
            turningLeft = false;
        }
    }
}


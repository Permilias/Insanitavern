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

    Vector2 AddRot(Vector2 baseVector, float rotAdded)
    {
        float _rot = Mathf.Deg2Rad * rotAdded;
        return new Vector2((Mathf.Cos(_rot) * baseVector.x) - (Mathf.Sin(_rot) * baseVector.y), (Mathf.Sin(_rot) * baseVector.x) + (Mathf.Cos(_rot) * baseVector.y));
    }

    Vector2 target, pos, diff, startPos, pushForce;
    Vector2[] dirs;
    Vector3 rayStart;
    Vector3[] rayDirs;
    RaycastHit rayHit;
    Ray[] rays;

    public Vector3 vel;

    float lowest;
    bool foundAngle, turnLeft;

    public void Push(Vector2 force)
    {
        print("pushed !");
        pushForce = force;
    }

    public bool debugs;

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

        dirs[1] = AddRot(dirs[0], -detectionAngle*2);
        dirs[2] = AddRot(dirs[0], -detectionAngle);
        dirs[3] = AddRot(dirs[0], detectionAngle*2);
        dirs[4] = AddRot(dirs[0], detectionAngle);

        foundAngle = false;
        startPos = pos - (dirs[0]*Registry.settings.rayStartBackMult);
        rayStart = new Vector3(startPos.x, 0.1f, startPos.y);
        int tryCount = 0;

        while (!foundAngle)
        {
            foundAngle = true;
            lowest = 9999;
            for (int i = 0; i < 5; i++)
            {
                rayDirs[i] = new Vector3(dirs[i].x, 0.1f, dirs[i].y);
                rays[i] = new Ray(rayStart, rayDirs[i] * detectionDistance);
                Color col = Color.green;



                if (Physics.Raycast(rays[i], out rayHit, detectionDistance, mask))
                {
                    foundAngle = false;
                    col = Color.red;

                    
                    if (i >= 3)
                    {
                        turnLeft = true;
                    }
                    else
                    {
                        turnLeft = false;
                    }


                }

                Debug.DrawRay(rayStart, rayDirs[i] * detectionDistance, col, 0);
            }

            tryCount++;
            if(tryCount > 100)
            {
                Debug.Log("prout");
                foundAngle = true;
            }

            if (!foundAngle)
            {
                if (turnLeft) TurnDirLeft(); else TurnDirRight();
            }
            else
            {

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
            dirs[i] = AddRot(dirs[i], tryAngle);
        }
    }

    void TurnDirRight()
    {
        for (int i = 0; i < 5; i++)
        {
            dirs[i] = AddRot(dirs[i], -tryAngle);
        }
    }
}


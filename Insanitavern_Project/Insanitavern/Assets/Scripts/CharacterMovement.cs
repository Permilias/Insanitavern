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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        pushResistance = 0.1f;
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

    Vector2 target, pos, diff, dir, leftDir, rightDir, startPos, pushForce;
    float pushResistance;
    Vector3 rayStart, rayDir, leftRayDir, rightRayDir;
    RaycastHit rayHit;
    Ray ray, leftRay, rightRay;

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
        dir = diff.normalized;
        if(pushForce.magnitude > 0)
        {
            if (debugs) print("applying pushforce " + pushForce.x + "," + pushForce.y);
            dir += pushForce;
            pushForce = Vector2.MoveTowards(pushForce, Vector2.zero, pushResistance);
            
        }

        if (debugs) print("dir = " + dir.x + "," + dir.y);

        leftDir = AddRot(dir, -detectionAngle);
        rightDir = AddRot(dir, detectionAngle);
        foundAngle = false;
        startPos = pos - (dir/0.75f);
        rayStart = new Vector3(startPos.x, 0.1f, startPos.y);

        while (!foundAngle)
        {
            rayDir = new Vector3(dir.x, 0.1f, dir.y);
            leftRayDir = new Vector3(leftDir.x, 0.1f, leftDir.y);
            rightRayDir = new Vector3(rightDir.x, 0.1f, rightDir.y);

            ray = new Ray(rayStart, rayDir * detectionDistance);
            leftRay = new Ray(rayStart, leftRayDir * detectionDistance);
            rightRay = new Ray(rayStart, rightRayDir * detectionDistance);

            Color centerCol = Color.green;
            Color leftCol = Color.green;
            Color rightCol = Color.green;

            turnLeft = false;

            foundAngle = true;
            LayerMask mask = LayerMask.GetMask("Obstacle");
            if (Physics.Raycast(ray, out rayHit, detectionDistance, mask))
            {
                foundAngle = false;
                centerCol = Color.red;
            }

            if (Physics.Raycast(leftRay, out rayHit, detectionDistance, mask))
            {
                lowest = rayHit.distance;
                turnLeft = true;
                foundAngle = false;
                leftCol = Color.red;
            }

            if (Physics.Raycast(rightRay, out rayHit, detectionDistance, mask))
            {
                if (rayHit.distance < lowest)
                {
                    turnLeft = false;
                    lowest = rayHit.distance;
                }
                foundAngle = false;
                rightCol = Color.red;
            }

            if(!foundAngle)
            {
                if (turnLeft) TurnDirLeft(); else TurnDirRight();
            }
            else
            {
                Debug.DrawRay(rayStart, rayDir * detectionDistance, centerCol, 0);
                Debug.DrawRay(rayStart, leftRayDir * detectionDistance, leftCol, 0);
                Debug.DrawRay(rayStart, rightRayDir * detectionDistance, rightCol, 0);
            }



        }

        //apply velocity
        vel = new Vector3(dir.x, 0, dir.y);

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
        dir = AddRot(dir, tryAngle);
        leftDir = AddRot(dir, -detectionAngle);
        rightDir = AddRot(dir, detectionAngle);
    }

    void TurnDirRight()
    {
        dir = AddRot(dir, -tryAngle);
        leftDir = AddRot(dir, -detectionAngle);
        rightDir = AddRot(dir, detectionAngle);
    }
}


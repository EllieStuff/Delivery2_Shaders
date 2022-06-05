using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAgentScript : MonoBehaviour
{
    [SerializeField]
    float
        minSpeed = 5,
        maxSpeed = 8,
        perceptionRadius = 2.5f,
        avoidanceRadius = 1,
        maxSteerForce = 8,
        alignWeight = 2,
        cohesionWeight = 1,
        separateWeight = 2.5f,
        targetWeight = 2,
        boundsRadius = 0.27f,
        avoidCollisionWeight = 10,
        collisionAvoidDst = 5;
    [SerializeField] LayerMask obstacleMask;

    public Vector3 Position { get { return transform.position; } }
    public Vector3 Forward { get { return transform.forward; } }
    Vector3 vel;

    [HideInInspector]
    public Vector3 avgFlockHeading;
    [HideInInspector]
    public Vector3 avgAvoidanceHeading;
    [HideInInspector]
    public Vector3 centreOfFlockmates;
    [HideInInspector]
    public int numFlockmates;

    Transform target;

    public void Init(Transform _target)
    {
        target = _target;
        vel = transform.forward * minSpeed;
    }

    private void Update()
    {
        Vector3 acc = SteerTowards(target.position - Position) * targetWeight;
        if (numFlockmates > 0)
        {
            acc += SteerTowards(avgFlockHeading) * alignWeight; // Aligment Force
            centreOfFlockmates /= numFlockmates;
            acc += SteerTowards(centreOfFlockmates - Position) * cohesionWeight; // Cohesion Force
            acc += SteerTowards(avgAvoidanceHeading) * separateWeight;  // Separation Force
        }

        if (IsHeadingForCollision())
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * avoidCollisionWeight;
            acc += collisionAvoidForce;
        }

        vel += acc * Time.deltaTime;
        float speed = vel.magnitude;
        Vector3 dir = vel / speed;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        vel = dir * speed;

        transform.position += vel * Time.deltaTime;
        transform.forward = dir;
    }

    bool IsHeadingForCollision()
    {
        RaycastHit hit;
        if (Physics.SphereCast(Position, boundsRadius, Forward, out hit, collisionAvoidDst, obstacleMask))
            return true;
        else
            return false;
    }

    Vector3 ObstacleRays()
    {
        Vector3[] raysDirections = BoidManagerScript.RaysDirections;

        for (int i = 0; i < raysDirections.Length; i++)
        {
            Vector3 dir = target.TransformDirection(raysDirections[i]);
            Ray ray = new Ray(Position, dir);
            if (!Physics.SphereCast(ray, boundsRadius, collisionAvoidDst, obstacleMask))
            {
                return dir;
            }
        }

        return Forward;
    }

    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * maxSpeed - vel;
        return Vector3.ClampMagnitude(v, maxSteerForce);
    }

}
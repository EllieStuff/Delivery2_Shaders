using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManagerScript : MonoBehaviour
{
    public Transform target;
    public ComputeShader boidCmptShader;
    public float awarenessDist = 2.5f, avoidanceDist = 1;
    public int viewDirectionsAmount = 300;

    BoidAgentScript[] boids;
    static Vector3[] raysDirections;
    public static Vector3[] RaysDirections { get { return raysDirections; } }

    void Start()
    {
        boids = FindObjectsOfType<BoidAgentScript>();
        foreach (BoidAgentScript boidAgent in boids)
            boidAgent.Init(target);

        CalculateRaysDirections(viewDirectionsAmount);
    }

    void Update()
    {
        if (boids == null) return;

        BoidInfo[] boidsData = new BoidInfo[boids.Length];
        for (int i = 0; i < boids.Length; i++)
        {
            boidsData[i].pos = boids[i].Position;
            boidsData[i].dir = boids[i].Forward;
        }

        var boidsBuffer = new ComputeBuffer(boids.Length, BoidInfo.Size);
        boidsBuffer.SetData(boidsData);

        int kernelHandle = boidCmptShader.FindKernel("CSMain");
        boidCmptShader.SetBuffer(kernelHandle, "_boids", boidsBuffer);
        boidCmptShader.SetInt("_boidsLength", boids.Length);
        boidCmptShader.SetFloat("_awarenessDist", awarenessDist);
        boidCmptShader.SetFloat("_avoidanceDist", avoidanceDist);

        int threadGroups = Mathf.CeilToInt(boids.Length / 1024.0f);
        boidCmptShader.Dispatch(kernelHandle, threadGroups, 1, 1);

        boidsBuffer.GetData(boidsData);

        for (int i = 0; i < boids.Length; i++)
        {
            boids[i].avgFlockHeading = boidsData[i].flockHeading;
            boids[i].centreOfFlockmates = boidsData[i].flockCentre;
            boids[i].avgAvoidanceHeading = boidsData[i].avoidanceHeading;
            boids[i].numFlockmates = boidsData[i].numFlockmates;
        }

        boidsBuffer.Release();
    }

    void CalculateRaysDirections(int _viewDirectionsAmount)
    {
        raysDirections = new Vector3[_viewDirectionsAmount];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < _viewDirectionsAmount; i++)
        {
            float t = (float)i / _viewDirectionsAmount;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            raysDirections[i] = new Vector3(x, y, z);
        }

    }


    public struct BoidInfo
    {
        public Vector3 pos;
        public Vector3 dir;

        public Vector3 flockHeading;
        public Vector3 flockCentre;
        public Vector3 avoidanceHeading;
        public int numFlockmates;

        public static int Size
        {
            get
            {
                return sizeof(float) * 3 * 5 + sizeof(int);
            }
        }
    }
}
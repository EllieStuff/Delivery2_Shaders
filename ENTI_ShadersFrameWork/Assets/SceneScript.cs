using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour
{
    public Material blurMat;
    //public MeshRenderer vertexAnimColumn;

    private Material savedMat;
    private Color bloomColor;
    private float bloomIntensity;
    private float intensity;
    private float blend;
    private Material savedVertexAnimMat;

    [SerializeField] private float timer = 0;
    [SerializeField] float speed = 0.01f;
    [SerializeField] float blurAnimMaxTime = 3;
    [SerializeField] private bool incrementOrDecrement;
    private Color EndColor;

    private void Start()
    {
        savedMat = new Material(blurMat);
        //savedVertexAnimMat = new Material(vertexAnimColumn.material);
        //vertexAnimColumn.material = savedVertexAnimMat;

        EndColor = Color.white;

        bloomColor = savedMat.GetColor("_BloomColor");
        bloomIntensity = savedMat.GetFloat("_BloomGlow");
        intensity = savedMat.GetFloat("_intensity");
        blend = savedMat.GetFloat("_blend");

        GetComponent<Image>().material = savedMat;
    }

    // Update is called once per frame
    void Update()
    {
        if(!incrementOrDecrement)
        {
            bloomColor = Color.Lerp(bloomColor, EndColor, Time.deltaTime * 0.5f);
            
            if (intensity < blurMat.GetFloat("_intensity"))
                intensity += Time.deltaTime * speed;

            timer += Time.deltaTime;
            if (timer >= blurAnimMaxTime)
                incrementOrDecrement = true;
        }
        else
        {
            bloomColor = Color.Lerp(bloomColor, blurMat.GetColor("_BloomColor"), Time.deltaTime * 0.5f);
            
            if(intensity > 0.0f)
                intensity -= Time.deltaTime * speed;

            timer -= Time.deltaTime;
            if (timer <= 0)
                incrementOrDecrement = false;
        }

        savedMat.SetColor("_BloomColor", bloomColor);
        savedMat.SetFloat("_BloomGlow", bloomIntensity);
        savedMat.SetFloat("_intensity", intensity);
        savedMat.SetFloat("_blend", blend);

        //savedVertexAnimMat.SetFloat("_time", Time.timeSinceLevelLoad);
    }
}

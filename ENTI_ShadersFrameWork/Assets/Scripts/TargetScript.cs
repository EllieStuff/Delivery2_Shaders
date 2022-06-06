using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomizePosition());
    }


    IEnumerator RandomizePosition()
    {
        while (true)
        {
            transform.position = new Vector3(Random.Range(-15, 15), Random.Range(5, 20), Random.Range(-15, 15));
            yield return new WaitForSeconds(Random.Range(5, 10));
        }
    }

}

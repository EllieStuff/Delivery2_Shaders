using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StormManager : MonoBehaviour
{
    const float RESPAWN_MARGIN = 2.0f;
    const float ISLAND_MARGIN = 70.0f;
    const float TRANSITION_SPEED = 2.0f;

    [SerializeField] Transform stormPoint1, stormPoint2;
    [SerializeField] Transform storm;
    [SerializeField] Transform land;
    [SerializeField] float stormSpeed = 10;
    [Space]
    [SerializeField] Image postProImage;
    [SerializeField] Image darkImage;
    [SerializeField] Material defaultMat, rainingMat;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip defaultClip, rainingClip;

    Vector3 moveDir;
    bool raining = false;

    // Start is called before the first frame update
    void Start()
    {
        storm.position = stormPoint1.position;
        moveDir = (stormPoint2.position - stormPoint1.position).normalized;

        postProImage.material = defaultMat;
    }

    // Update is called once per frame
    void Update()
    {
        storm.position += moveDir * stormSpeed * Time.deltaTime;
        //Debug.Log("Distance to land: " + Vector3.Distance(storm.position, land.position));
        if (!raining && Vector3.Distance(storm.position, land.position) < ISLAND_MARGIN)
        {
            Debug.Log("Started raining");
            raining = true;
            StartCoroutine(MaterialTransition(rainingMat, rainingClip));
        }
        else if(raining && Vector3.Distance(storm.position, land.position) > ISLAND_MARGIN)
        {
            Debug.Log("Stopped raining");
            raining = false;
            StartCoroutine(MaterialTransition(defaultMat, defaultClip));
        }


        if (Vector3.Distance(storm.position, stormPoint2.position) < RESPAWN_MARGIN)
            storm.position = stormPoint1.position;
    }


    IEnumerator MaterialTransition(Material _newMat, AudioClip _newClip)
    {
        float timer = 0, maxTime = TRANSITION_SPEED;
        while(timer < maxTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            darkImage.color = Color.Lerp(Color.clear, Color.black, timer / maxTime);
        }

        postProImage.material = _newMat;
        audioSource.clip = _newClip;
        audioSource.Play();

        timer = 0;
        while (timer < maxTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            darkImage.color = Color.Lerp(Color.black, Color.clear, timer / maxTime);
        }
    }
}

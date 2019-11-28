using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringManager : MonoBehaviour
{
    public float timeBeforeDie;
    public float speedUp;
    public float scaleMultiplier;
    private float timeCount = 0;

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        if (transform.localScale.x < 2)
        {
            transform.localScale *= scaleMultiplier;
        }
        transform.Translate(0, speedUp * Time.deltaTime, 0);
        if (timeCount >= timeBeforeDie)
        {
            Destroy(gameObject);
        }
    }
}

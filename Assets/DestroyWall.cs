using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Vector3 projection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -1);
                transform.GetChild(i).GetComponent<Rigidbody>().AddForce(projection * 500);
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerMove>().isDashing)
            {
                other.gameObject.GetComponent<PlayerMove>().Scoring();
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    Vector3 projection = new Vector3(Random.Range(-1f, 1f) * 1000, Random.Range(0f, 1f)* 1000, 1f * 2500);
                    transform.GetChild(i).GetComponent<Rigidbody>().AddForce(projection);
                }
            }
            else
            {
                other.gameObject.GetComponent<PlayerMove>().Die();
            }

        }

    }
}

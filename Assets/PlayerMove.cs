using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speedChangeLane;
    public float seuil;
    public Transform[] lanes;

    private int indexCurrentLane = 1;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (indexCurrentLane >= 1)
            {
                indexCurrentLane--;
                rb.velocity = new Vector3(-speedChangeLane, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (indexCurrentLane <= 1)
            {
                indexCurrentLane++;
                rb.velocity = new Vector3(speedChangeLane, 0, 0);
            }
        }
        if (Vector3.Distance(transform.position, lanes[indexCurrentLane].position) < seuil)
        {
            rb.velocity = Vector3.zero;
            transform.position = lanes[indexCurrentLane].position;
        }
    }
}

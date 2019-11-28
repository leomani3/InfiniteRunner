using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speed;
    public float cameraKick;
    public float speedFollow;
    public float speedOffset;
    public Vector3 offSet;
    private GameObject player;
    public bool dash;
    private float ZOffSet;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (dash)
        {
            ZOffSet += speedOffset * Time.deltaTime;
            if (ZOffSet >= cameraKick)
            {
                dash = false;
            }
        }
        else
        {
            if (ZOffSet > 0)
            {
                ZOffSet -= speedFollow*(ZOffSet / cameraKick) * Time.deltaTime;
            }
        }

        Vector3 desiredPosition = player.transform.position + offSet;
        transform.position = new Vector3(transform.position.x, transform.position.y, Vector3.Lerp(transform.position, desiredPosition, speed).z - ZOffSet);
    }
}
    
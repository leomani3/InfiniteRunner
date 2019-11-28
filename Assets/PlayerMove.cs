using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float speedChangeLane;
    public float seuil;
    public Transform[] lanes;
    public bool isDashing;
    public float speedDash;
    public float moveSpeed;
    public List<GameObject> moveWithDash;
    public GameObject dieMenu;
    public CameraFollow cam;
    public GameObject scoringPrefab;
    public List<GameObject> comboBars;
    public Material comboMat;
    public Material emptyComboMat;

    private int indexCurrentLane = 1;
    private Rigidbody rb;
    private bool changingLane;
    private Vector3 playerLastPos;
    private float currentSpeed;
    private int comboCount = 0;
    private bool powerOn = false;
    private GameObject spawner;
    // Start is called before the first frame update
    void Start()
    {
        comboMat.SetColor("_EmissionColor", new Color(0, 0, 0, 1));
        rb = GetComponent<Rigidbody>();
        playerLastPos = transform.position;
        currentSpeed = moveSpeed;
        spawner = GameObject.Find("WallSpawner");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().scoreCount += (int)transform.position.z - (int)playerLastPos.z;
        GameObject.Find("WallSpawner").GetComponent<WallSpawner>().AddDistance(transform.position.z - playerLastPos.z);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (indexCurrentLane >= 1)
            {
                changingLane = true;
                indexCurrentLane--;
                rb.velocity = new Vector3(-speedChangeLane, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (indexCurrentLane <= 1)
            {
                changingLane = true;
                indexCurrentLane++;
                rb.velocity = new Vector3(speedChangeLane, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && !powerOn)
        {
            if (!changingLane && !isDashing)
            {
                StartCoroutine("Dash");
            }
        }
        if (changingLane && Mathf.Abs(transform.position.x - lanes[indexCurrentLane].transform.position.x) < seuil)
        {
            changingLane = false;
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(lanes[indexCurrentLane].position.x, transform.position.y, transform.position.z);
        }

        //bouger le niveau avec le joueur
        for (int i = 0; i < moveWithDash.Count; i++)
        {
            moveWithDash[i].transform.position = new Vector3(moveWithDash[i].transform.position.x, moveWithDash[i].transform.position.y, moveWithDash[i].transform.position.z + (transform.position.z - playerLastPos.z));
        }
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, currentSpeed);

        playerLastPos = transform.position;
    }

    private IEnumerator Dash()
    {
        //StartCoroutine("DashCamera");
        cam.dash = true;
        isDashing = true;
        currentSpeed = moveSpeed + speedDash;
        transform.localScale *= 1.5f;
        yield return new WaitForSeconds(0.1f);

        if (!powerOn)
        {
            isDashing = false;
            currentSpeed = moveSpeed;
            transform.localScale /= 1.5f;
        }
    }

    private IEnumerator DashCamera()
    {
        cam.speed = 0.1f;
        yield return new WaitForSeconds(2f);
        cam.speed = 1f;
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Scoring()
    {

        if (comboCount < 9)
        {
            comboBars[comboCount].transform.GetChild(0).GetComponent<MeshRenderer>().material = comboMat;
            comboBars[comboCount].transform.GetChild(1).GetComponent<MeshRenderer>().material = comboMat;
            comboMat.SetColor("_EmissionColor", new Color(comboMat.GetColor("_EmissionColor").r, comboMat.GetColor("_EmissionColor").g + 0.05f, comboMat.GetColor("_EmissionColor").b));
            comboCount++;
        }
        else if(comboCount >= 9 && !powerOn)
        {
            comboBars[comboCount].transform.GetChild(0).GetComponent<MeshRenderer>().material = comboMat;
            comboBars[comboCount].transform.GetChild(1).GetComponent<MeshRenderer>().material = comboMat;
            comboMat.SetColor("_EmissionColor", new Color(comboMat.GetColor("_EmissionColor").r, comboMat.GetColor("_EmissionColor").g + 0.05f, comboMat.GetColor("_EmissionColor").b));
            StartCoroutine("Power");
        }
       

        GameObject.Find("GameManager").GetComponent<GameManager>().scoreCount += 200;
        Instantiate(scoringPrefab, transform.position, scoringPrefab.transform.rotation, transform);
    }

    public IEnumerator Power()
    {
        if (isDashing)
        {
            currentSpeed -= speedDash;
        }
        StartCoroutine("DecrementComboBar");
        powerOn = true;
        isDashing = true;
        currentSpeed *= 5;
        transform.localScale *= 3f;
        spawner.GetComponent<WallSpawner>().PowerOn();
        yield return new WaitForSeconds(5f);
        transform.localScale = new Vector3(1, 1, 1);
        currentSpeed = moveSpeed;
        comboCount = 0;
        powerOn = false;
        comboMat.SetColor("_EmissionColor", new Color(0, 0, 0, 1));
        yield return new WaitForSeconds(0.5f);
        isDashing = false;
        //ResetComboBars();
    }

    public void ResetComboBars()
    {
        for (int i = 0; i < comboBars.Count; i++)
        {
            comboBars[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = emptyComboMat;
            comboBars[i].transform.GetChild(1).GetComponent<MeshRenderer>().material = emptyComboMat;
        }
    }

    public IEnumerator DecrementComboBar()
    {
        yield return new WaitForSeconds(0.5f);
        comboBars[9].transform.GetChild(0).GetComponent<MeshRenderer>().material = emptyComboMat;
        comboBars[9].transform.GetChild(1).GetComponent<MeshRenderer>().material = emptyComboMat;
        yield return new WaitForSeconds(0.5f);
        comboBars[8].transform.GetChild(0).GetComponent<MeshRenderer>().material = emptyComboMat;
        comboBars[8].transform.GetChild(1).GetComponent<MeshRenderer>().material = emptyComboMat;
        yield return new WaitForSeconds(0.5f);
        comboBars[7].transform.GetChild(0).GetComponent<MeshRenderer>().material = emptyComboMat;
        comboBars[7].transform.GetChild(1).GetComponent<MeshRenderer>().material = emptyComboMat;
        yield return new WaitForSeconds(0.5f);
        comboBars[6].transform.GetChild(0).GetComponent<MeshRenderer>().material = emptyComboMat;
        comboBars[6].transform.GetChild(1).GetComponent<MeshRenderer>().material = emptyComboMat;
        yield return new WaitForSeconds(0.5f);
        comboBars[5].transform.GetChild(0).GetComponent<MeshRenderer>().material = emptyComboMat;
        comboBars[5].transform.GetChild(1).GetComponent<MeshRenderer>().material = emptyComboMat;
        yield return new WaitForSeconds(0.5f);
        comboBars[4].transform.GetChild(0).GetComponent<MeshRenderer>().material = emptyComboMat;
        comboBars[4].transform.GetChild(1).GetComponent<MeshRenderer>().material = emptyComboMat;
        yield return new WaitForSeconds(0.5f);
        comboBars[3].transform.GetChild(0).GetComponent<MeshRenderer>().material = emptyComboMat;
        comboBars[3].transform.GetChild(1).GetComponent<MeshRenderer>().material = emptyComboMat;
        yield return new WaitForSeconds(0.5f);
        comboBars[2].transform.GetChild(0).GetComponent<MeshRenderer>().material = emptyComboMat;
        comboBars[2].transform.GetChild(1).GetComponent<MeshRenderer>().material = emptyComboMat;
        yield return new WaitForSeconds(0.5f);
        comboBars[1].transform.GetChild(0).GetComponent<MeshRenderer>().material = emptyComboMat;
        comboBars[1].transform.GetChild(1).GetComponent<MeshRenderer>().material = emptyComboMat;
        yield return new WaitForSeconds(0.5f);
        comboBars[0].transform.GetChild(0).GetComponent<MeshRenderer>().material = emptyComboMat;
        comboBars[0].transform.GetChild(1).GetComponent<MeshRenderer>().material = emptyComboMat;
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public List<GameObject> obstaclePrefabs;
    public List<Transform> spawnPoints;

    private float distanceBetweenSpawn;
    private float distanceCount = 0;
    private bool powerOn = false;


    public void AddDistance(float d)
    {
        distanceCount += d;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (distanceCount >= distanceBetweenSpawn)
        {
            List<GameObject> objectsToSpawn = new List<GameObject>();
            int nbToSpawn = Random.Range(1, 4);
            for (int i = 0; i < nbToSpawn; i++)
            {
                if (powerOn)
                {
                    objectsToSpawn.Add(obstaclePrefabs[0]);
                }
                else
                {
                    if (nbToSpawn == 3 && i == 0)
                    {
                        objectsToSpawn.Add(obstaclePrefabs[0]);
                    }
                    else
                    {
                        objectsToSpawn.Add(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)]);
                    }
                }
            }

            List<int> positionsUsed = new List<int>();
            for (int i = 0; i < nbToSpawn; i++)
            {
                int rdm;
                do
                {
                    rdm = Random.Range(0, spawnPoints.Count);
                } while (positionsUsed.Contains(rdm));

                positionsUsed.Add(rdm);
                Vector3 rdmSpawnPoint = spawnPoints[rdm].position;
                Instantiate(objectsToSpawn[i], rdmSpawnPoint, Quaternion.Euler(0, 90, 0));
            }

            distanceBetweenSpawn = Random.Range(5, 80);
            distanceCount = 0;
        }
    }

    public void PowerOn()
    {
        StartCoroutine("Power");
    }

    public IEnumerator Power()
    {
        powerOn = true;
        GameObject[] murs = GameObject.FindGameObjectsWithTag("Mur");
        for (int i = 0; i < murs.Length; i++)
        {
            Instantiate(obstaclePrefabs[0], murs[i].transform.position, murs[i].transform.rotation);
            Destroy(murs[i].gameObject);
        }
        yield return new WaitForSeconds(5f);
        powerOn = false;
    }
}

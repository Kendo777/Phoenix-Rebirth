using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] myAsteroids;
    public Transform[] spawnPoints;

    void Start()
    {

        Transform[] asteroidPositions = new Transform[myAsteroids.Length];
        RandomPoint(asteroidPositions);

        for(int i = 0; i < myAsteroids.Length; i++)
        {
            Instantiate(myAsteroids[i], asteroidPositions[i].position,Quaternion.identity);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void RandomPoint(Transform [] asteroidPositions)
    {
        int index;
        for (int i = 0; i < asteroidPositions.Length; i++)
        {
            index = Random.Range(0, spawnPoints.Length - 1);

            while (!ArrayExist(index, asteroidPositions))
            {
                index = Random.Range(0, spawnPoints.Length - 1);
            }
            asteroidPositions[i] = spawnPoints[index];

        }
    }
    private bool ArrayExist(int index, Transform[] asteroidPositions)
    {
        for (int j = 0; j < asteroidPositions.Length; j++)
        {
            if (asteroidPositions[j] == spawnPoints[index])
            {
                return false;
            }
        }
        return true;
    }
}

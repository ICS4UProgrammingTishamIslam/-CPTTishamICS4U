using UnityEngine;

public class Spawn : MonoBehaviour
{
    //variables for what enemy and where to send it
    public GameObject spawn;
    public Transform destination;

    //variables for wait times
    public float startWait;
    private float nextSpawn;

    public float intervalWait;
    public float intervalLength;
    private float intervalCounter;

    public float waveWait;

    private void Start()
    {
        nextSpawn = Time.time + startWait;
    }

    // Update is called once per frame
    void Update()
    {
        //check if enough time has passed to spawn
        if (Time.time > nextSpawn)
        {
            //spawn an enemy, then calculate the time to the next spawn
            SpawnEnemy();
            if (intervalCounter < intervalLength)
            {
                nextSpawn += intervalWait;
            }
            else
            {
                nextSpawn += waveWait;
                intervalCounter = 0;
            }
        }
    }

    public void SpawnEnemy()
    {
        //spawn an enemy here
        Instantiate(spawn, transform.position, Quaternion.identity, gameObject.transform);
    }
}

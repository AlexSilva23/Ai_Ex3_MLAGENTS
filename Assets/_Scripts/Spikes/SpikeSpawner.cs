using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    public GameObject objectToIntantiate;
    Collider coll;
    float timeBetweenSpiked = 2f;
    public GameObject spike;
    public AgentDodge agent;
    void Start()
    {
        coll = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        timeBetweenSpiked += Time.deltaTime;
        if (timeBetweenSpiked > Random.Range(.75f, 2f))
        {
            timeBetweenSpiked = 0;
            SpawnNewSpike();
        }

    }

    void SpawnNewSpike()
    {
        spike = Instantiate(objectToIntantiate, RandomPointInBounds(coll.bounds), Quaternion.identity);
        spike.transform.eulerAngles = new Vector3(90, 0, 0);
        spike.GetComponent<SpikeScript>().agent = agent;
        agent.AddSpike(spike);
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;
public class AgentDodge : Agent
{
    [SerializeField] private List<GameObject> spikes;
    public SpikeSpawner SpikeScript;
    Vector3 initialPos;
    public Transform floor;
    public BufferSensorComponent bSensor;

    int fails;
    public TextMeshPro FailsText;

    private void Start()
    {
        initialPos = transform.localPosition;

    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = initialPos;
        for (int i = 0; i < spikes.Count; i++)
        {
            Destroy(spikes[i]);
        }
        spikes.Clear();
        SetReward(.1f);
    }

    private void Update()
    {
        FailsText.text = "Fails: " + fails.ToString();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        //for (int i = 0; i < spikes.Count; i++)
        //{
        //    Transform transformSpike = spikes[i].GetComponent<Transform>();
        //    Rigidbody rb = spikes[i].GetComponent<Rigidbody>();
        //    float[] obs = { transform.position.x - transformSpike.position.x, transformSpike.localPosition.x, transformSpike.localPosition.y, rb.velocity.y };
        //    bSensor.AppendObservation(obs);
        //}

        foreach (GameObject pyke in spikes)
        {
            Transform transformSpike = pyke.transform;
            Rigidbody RBSpike = pyke.GetComponent<Rigidbody>();
            float[] obs = { transform.position.x - transformSpike.position.x, transformSpike.localPosition.x, transformSpike.localPosition.y, RBSpike.velocity.y };
            bSensor.AppendObservation(obs);
        }
}

public override void OnActionReceived(ActionBuffers actions)
{
    float moveX = actions.ContinuousActions[0];
    transform.localPosition += new Vector3(moveX, 0, 0) * Time.deltaTime * 5;

    if (transform.position.y < floor.transform.position.y - 2)
    {
        Debug.Log("Fell off");
        SetReward(-1f);
        fails++;
        EndEpisode();
    }
}

public override void Heuristic(in ActionBuffers actionsOut)
{
    ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
    continuousActions[0] = Input.GetAxisRaw("Horizontal");
}

private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Spike"))
    {
        Debug.Log("Got hit!");
        fails++;
        SetReward(-1f);
        EndEpisode();
    }
}

public void AddSpike(GameObject spike)
{
    spikes.Add(spike);
}

public void RemoveSpike(GameObject spike)
{
    spikes.Remove(spike);
}
}

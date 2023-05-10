using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoteToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    public float moveSpeed;
    [SerializeField] private Material WinMaterial;
    [SerializeField] private Material LoseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.localPosition;
    }
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-8f, -1f), 1.5f, Random.Range(-4f, 4.5f));
        targetTransform.localPosition = new Vector3(Random.Range(3f, 7.7f), 1.2f, Random.Range(-4f, 4.4f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+1f);
            floorMeshRenderer.material = WinMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<Walls>(out Walls wall))
        {
            SetReward(-1f);
            floorMeshRenderer.material = LoseMaterial;
            EndEpisode();
        }

    }
}

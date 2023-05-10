using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [HideInInspector] public AgentDodge agent;
    private void OnCollisionEnter(Collision collision)
    {
        agent.RemoveSpike(gameObject);
        Destroy(gameObject);
    }
}

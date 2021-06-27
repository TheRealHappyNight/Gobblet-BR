using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent : Unity.MLAgents.Agent
{
    [SerializeField] private Transform target;
    private Rigidbody _rigidbody;

    [SerializeField] private Material loseMat;
    [SerializeField] private Material winMat;
    [SerializeField] private MeshRenderer floorRenderer;
    
    private void Awake()
    {
        floorRenderer.material = loseMat;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actionBuffers = actionsOut.DiscreteActions;
        
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")))
        {
            case -1: 
                actionBuffers[0] = 1; 
                break;
            case 0:
                actionBuffers[0] = 0; 
                break;
            case 1: 
                actionBuffers[0] = 2;
                break;
                
        }
        
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical")))
        {
            case -1: 
                actionBuffers[1] = 1; 
                break;
            case 0:
                actionBuffers[1] = 0; 
                break;
            case 1: 
                actionBuffers[1] = 2;
                break;
                
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.DiscreteActions[0];
        float moveZ = actions.DiscreteActions[1];

        // float moveSpeed = 1f;
        // transform.position += new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

        Vector3 addForce = new Vector3(0, 0, 0);

        switch (moveX)
        {
            case 0: 
                addForce.x = 0f;
                break;
            case 1:
                addForce.x = -1f;
                break;
            case 2:
                addForce.x = +1f;
                break;
        }
        
        switch (moveZ)
        {
            case 0: 
                addForce.z = 0f;
                break;
            case 1:
                addForce.z = -1f;
                break;
            case 2:
                addForce.z = +1f;
                break;
        }

        float moveSpeed = 5f;
        _rigidbody.velocity = addForce * moveSpeed + new Vector3(0, _rigidbody.velocity.y, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 dirToReward = (target.localPosition - transform.localPosition).normalized;
        
        sensor.AddObservation(dirToReward.x);
        sensor.AddObservation(dirToReward.z);
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reward"))
        {
            SetReward(1f);
            floorRenderer.material = winMat;
            EndEpisode();
        }

        if (other.CompareTag("Obstacles"))
        {
            SetReward(-1f);
            floorRenderer.material = loseMat;
            EndEpisode();
        }
    }
}

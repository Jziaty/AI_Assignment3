using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script is the modification of the implementation of the Boids behaviors from http://www.unifycommunity.com/wiki/index.php?title=Flocking
/// </summary>

public class Flock : MonoBehaviour 
{
	internal FlockController controller;
    private const string flockTag = "flockMember";

    public ArrayList flocksInRange = new ArrayList();

    void Update()
    {
        if (controller)
        {
            Vector3 relativePos = steer() * Time.deltaTime;

			if(relativePos != Vector3.zero)
                GetComponent<Rigidbody>().velocity = relativePos;

            // Enforce minimum and maximum speeds for the boids
            float speed = GetComponent<Rigidbody>().velocity.magnitude;
            if (speed > controller.maxVelocity)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * controller.maxVelocity;
            }
            else if (speed < controller.minVelocity)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * controller.minVelocity;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(flockTag))
        {
            flocksInRange.Add(other.GetComponent<Flock>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(flockTag))
        {
            flocksInRange.Remove(other.GetComponent<Flock>());
        }
    }

    //Calculate flock steering Vector based on the Craig Reynold's algorithm (Cohesion, Alignment, Follow leader and Seperation)
    private Vector3 steer()
	{
		Vector3 center = controller.flockCenter - transform.localPosition;			                // cohesion
		Vector3 velocity = controller.flockVelocity - GetComponent<Rigidbody>().velocity; 			// alignment
		Vector3 separation = Vector3.zero; 											                // separation

        Vector3 follow = controller.target.localPosition - transform.localPosition;                 // follow leader

        foreach (Flock flock in flocksInRange)
		{
            if (flock != this) 
            {
                Vector3 relativePos = transform.localPosition - flock.transform.localPosition;
				separation += relativePos / (relativePos.sqrMagnitude);		
			}
		}

        // randomize
		Vector3 randomize = new Vector3( (Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);
		
		randomize.Normalize();
						
		return (controller.cohesionWeight*center + 
				controller.alignmentWeight*velocity + 
				controller.separationWeight*separation + 
				controller.followWeight*follow + 
				controller.randomizeWeight*randomize);
	}	
}
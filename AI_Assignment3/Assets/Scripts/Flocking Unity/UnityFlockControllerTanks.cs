using UnityEngine;
using System.Collections;


public class UnityFlockControllerTanks : MonoBehaviour {
	public Vector3 bound;
	public float speed = 20.0f;

	private Vector3 initialPosition;
	private Vector3 nextMovementPoint;

	private UnityEngine.AI.NavMeshAgent[] navAgents;

	// Use this for initialization
	void Start()
	{
		initialPosition = transform.position;
		CalculateNextMovementPoint();

		navAgents = FindObjectsOfType(typeof(UnityEngine.AI.NavMeshAgent)) as UnityEngine.AI.NavMeshAgent[];
	}


	void UpdateTargets ( Vector3 targetPosition  )
	{
		foreach(UnityEngine.AI.NavMeshAgent agent in navAgents) 
		{
			agent.destination = targetPosition;
		}
	}

	void CalculateNextMovementPoint()
	{
		float posX = Random.Range(initialPosition.x - bound.x, initialPosition.x + bound.x);
		float posY = Random.Range(initialPosition.y - bound.y, initialPosition.y + bound.y);
		float posZ = Random.Range(initialPosition.z - bound.z, initialPosition.z + bound.z);

		nextMovementPoint = initialPosition + new Vector3(posX, posY, posZ);
	}

	void setTankDirection() {

		// Get the targetObject in the game (Sphere)
		GameObject targetObject = GameObject.FindGameObjectWithTag("Sphere");
		//Vector to have the ray cast downwards from the sphere object
		Vector3 down = transform.TransformDirection (Vector3.down);
		//Get the position of the object.
		Vector3 positie = targetObject.transform.position;

		RaycastHit hitInfo;

		//If the ray collides with a collider (terrain in this case)
		//Calculate the new target for the tanks en update the position
		if (Physics.Raycast(positie, down, out hitInfo)) 
		{
			Vector3 navTarget = hitInfo.point;
			Debug.DrawLine (positie, navTarget, Color.red);
			UpdateTargets(navTarget);				
		}
	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextMovementPoint - transform.position), 1.0f * Time.deltaTime);

		if (Vector3.Distance (nextMovementPoint, transform.position) <= 50.0f) 
		{
			CalculateNextMovementPoint ();
			Debug.Log ("Next point calculated");
		}
		setTankDirection ();

	}
}

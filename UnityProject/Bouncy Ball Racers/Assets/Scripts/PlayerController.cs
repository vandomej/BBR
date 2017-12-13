using UnityEngine;
using System.Collections;

//IMPORTANT NOTE: EVERYTHING THAT THE BALL BOUNCES OFF OF,
//HAS TO HAVE A NON-KINEMATIC RIGIDBODY ATTACHED.
public class PlayerController : MonoBehaviour {
	public SphereCollider Collider;
	public Vector3 CurrentAcceleration;
	public float Bounciness = 0.5f;
	public float MaxBounceGain = 0.5f;
	public float MaxBounceSpeed = 50.0f;
	public float MaxSpeed = 200.0f;

	public Vector3 m_currentVelocity;
	public float timeSinceJump;
	public float timeSinceBounce;

	// Use this for initialization
	void Start () {
		m_currentVelocity = new Vector3(0, 0, 0);
		timeSinceJump = 1.0f;
		timeSinceBounce = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//Incrementing timers every frame
		timeSinceJump += Time.deltaTime;
		timeSinceBounce += Time.deltaTime;

		//Handling jump (bounce) input
		if (Input.GetButtonDown("Jump")) {
			//If the player bounced within the last .25 seconds, then apply velocity multiplier, otherwise start the jump timer
			if (timeSinceBounce < 0.25 && m_currentVelocity.magnitude < MaxBounceSpeed) {
				m_currentVelocity *= 1 + (Mathf.Cos(timeSinceBounce * 2 * Mathf.PI) * MaxBounceGain);
			} else {
				timeSinceJump = 0.0f;
			}
		}
	
		m_currentVelocity += CurrentAcceleration * Time.deltaTime;
		m_currentVelocity = Vector3.ClampMagnitude(m_currentVelocity, MaxSpeed);

		//have to manually detect collisions and apply bounce
		RaycastHit hit;
		if (Physics.SphereCast(this.transform.position, Collider.radius, m_currentVelocity * Time.deltaTime, out hit, m_currentVelocity.magnitude * Time.deltaTime)) {
			//Move player to collision point, rather than moving into object
			this.transform.Translate(m_currentVelocity * (hit.distance * .999f * Time.deltaTime));
			timeSinceBounce = 0.0f;
			
			//Reflection vector
			Vector3 contactNormal = Vector3.Normalize(hit.normal);
			m_currentVelocity += (-2 * (Vector3.Dot(m_currentVelocity, contactNormal))) * contactNormal;

			//Applying bounce values
			float currentBounce = 1 + (
				(timeSinceJump < 0.25 && m_currentVelocity.magnitude < MaxBounceSpeed) ? 
				Mathf.Cos(timeSinceJump * 2 * Mathf.PI) * MaxBounceGain : 0
				);
			m_currentVelocity *= Bounciness * (currentBounce);
		} else {
			this.transform.Translate(m_currentVelocity * Time.deltaTime);
		}
	}
}

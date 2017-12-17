using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	#region public

	public SphereCollider Collider;
	public Vector3 CurrentAcceleration;

	public float Bounciness = 0.6f;
	public float MaxBounceGain = 1.3f;

	public float MaxSpeed = 50.0f;

	public float VerticalSpeed = 10.0f;

	public float HorizontalSpeed = 5.0f;

	public float RotationSpeed = 1.0f;

	#endregion

	#region private

	private Vector3 m_currentVelocity;
	private float m_timeSinceJump;
	private float m_timeSinceBounce;
	private float m_vert;
	private float m_hort;
	private float m_currentBounce;
	private Vector3 m_position;
	private RaycastHit m_sphereHit;
	private RaycastHit m_rayHit;
	private Vector3 m_movementVector;
	private Vector3 m_reflection;

	#endregion

	// Use this for initialization
	void Start () {
		m_currentVelocity = Vector3.zero;
		m_timeSinceJump = 1.0f;
		m_timeSinceBounce = 1.0f;
		m_vert = 0.0f;
		m_hort = 0.0f;
		m_currentBounce = 0.0f;
		m_position = this.transform.position;
		m_movementVector = Vector3.zero;
		m_reflection = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		m_timeSinceBounce += Time.deltaTime;
		m_timeSinceJump += Time.deltaTime;
		m_position = this.transform.position;

		m_vert = Input.GetAxis("Vertical");
		m_hort = Input.GetAxis("Horizontal");

		//Handling jump (bounce) input
		if (Input.GetButtonDown("Jump")) {
			//If the player bounced within the last .25 seconds, then apply velocity multiplier, otherwise start the jump timer
			if (m_timeSinceBounce < 0.25) {
				m_currentVelocity.y *= 1 + (Mathf.Cos(m_timeSinceBounce * 2 * Mathf.PI) * MaxBounceGain);
			} else {
				m_timeSinceJump = 0.0f;
			}
		}

		//m_currentVelocity = Vector3.RotateTowards(m_currentVelocity, this.transform)\
		m_currentVelocity = Quaternion.AngleAxis(m_hort * RotationSpeed, Vector3.up) * m_currentVelocity;
		m_currentVelocity += CurrentAcceleration * Time.deltaTime;
		//m_currentVelocity.x = Mathf.Clamp(m_currentVelocity.x, -MaxAxisSpeed, MaxAxisSpeed);
		//m_currentVelocity.y = Mathf.Clamp(m_currentVelocity.y, -MaxAxisSpeed, MaxAxisSpeed);
		m_currentVelocity = Vector3.ClampMagnitude(m_currentVelocity, MaxSpeed);

		this.transform.LookAt(new Vector3(m_position.x + m_currentVelocity.x, m_position.y, m_position.z + m_currentVelocity.z));
		m_movementVector = (m_currentVelocity + this.transform.TransformDirection(new Vector3(m_hort * HorizontalSpeed, 0, m_vert * VerticalSpeed))) * Time.deltaTime;

		if (Physics.SphereCast(m_position, Collider.radius * .9f, m_movementVector, out m_sphereHit, m_movementVector.magnitude)) {
			this.MoveTransform(m_movementVector * m_sphereHit.distance);

			m_reflection = Vector3.Reflect(m_movementVector, m_sphereHit.normal);

			m_currentBounce = 1 + (
				(m_timeSinceJump < 0.25) ? 
				Mathf.Cos(m_timeSinceJump * 2 * Mathf.PI) * MaxBounceGain : 0
				);

			m_currentVelocity = m_reflection / Time.deltaTime;
			m_currentVelocity.y *= m_currentBounce * Bounciness;
			m_timeSinceBounce = 0.0f;
		} else {
			this.MoveTransform(m_movementVector);
		}
	}

	public void MoveTransform(Vector3 movement) {
		if (Physics.Raycast(m_position, movement, out m_rayHit, movement.magnitude)) {
			this.transform.Translate((movement * -.2f) + (m_rayHit.normal * Collider.radius * 1.2f), Space.World);
		} else {
			this.transform.Translate(movement, Space.World);
		}
	}
}

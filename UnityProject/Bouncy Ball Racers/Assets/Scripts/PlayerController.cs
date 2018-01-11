using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	#region public

	public SphereCollider Collider;
	public Vector3 CurrentAcceleration;

	public float Bounciness = 0.6f;
	public float MaxBounceGain = 2.0f;

	public float MaxSpeed = 60.0f;

	public float MaxBounceHeight = 50.0f;

	public float VerticalSpeed = 17.0f;

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

	private bool isGrounded;

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
		isGrounded = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isGrounded) {
			this.RollLoop();
		} else {
			this.BounceLoop();
		}
		
	}

	public void BounceLoop() {
		//At some point do raycasts directly downward a certain distance to get the distance in height that the ball is,
		//Then if the max height of the ball at some point is too small, set isGrounded to true.
		m_timeSinceBounce += Time.deltaTime;
        m_timeSinceJump += Time.deltaTime;
        m_position = this.transform.position;

        m_vert = Input.GetAxis("Vertical");
        m_hort = Input.GetAxis("Horizontal");

        //Handling jump (bounce) input
        if (Input.GetButtonDown("Jump"))
        {
            //If the player bounced within the last .25 seconds, then apply velocity multiplier, otherwise start the jump timer
            if (m_timeSinceBounce < 0.25)
            {
				m_currentBounce = Bounciness + ((MaxBounceGain - Bounciness) * Mathf.Cos(m_timeSinceBounce * 2 * Mathf.PI));
                if (Mathf.Abs(m_currentVelocity.y) < MaxBounceHeight)
                {
					m_currentVelocity.y = Mathf.Clamp(m_currentVelocity.y * m_currentBounce, float.MinValue, MaxBounceHeight);
                }

                m_timeSinceBounce = 0.26f;
            }
            else
            {
                m_timeSinceJump = 0.0f;
            }
        }

        m_currentVelocity = Quaternion.AngleAxis(m_hort * RotationSpeed, Vector3.up) * m_currentVelocity;
        m_currentVelocity += CurrentAcceleration * Time.deltaTime;

        this.transform.LookAt(new Vector3(m_position.x + m_currentVelocity.x, m_position.y, m_position.z + m_currentVelocity.z));
		Vector3 temp = m_currentVelocity + this.transform.TransformDirection(new Vector3(m_hort * HorizontalSpeed, 0, m_vert * VerticalSpeed));
		temp.y = 0.0f;
		if (temp.magnitude < MaxSpeed)
		{
			m_movementVector = temp;
			m_movementVector.y = m_currentVelocity.y;
        }
		else
		{
			m_movementVector = m_currentVelocity;
		}
        
        if (Physics.SphereCast(m_position, Collider.radius * .9f, m_movementVector * Time.deltaTime, out m_sphereHit, m_movementVector.magnitude * Time.deltaTime))
        {
            this.MoveTransform(m_movementVector * m_sphereHit.distance * Time.deltaTime);

            m_reflection = Vector3.Reflect(m_movementVector, m_sphereHit.normal);

            m_currentVelocity = m_reflection;
			
			if (m_timeSinceJump < 0.25)
			{
				m_currentBounce = Bounciness + ((MaxBounceGain - Bounciness) * Mathf.Cos(m_timeSinceJump * 2 * Mathf.PI));
				if (Mathf.Abs(m_currentVelocity.y) < MaxBounceHeight)
				{
					m_currentVelocity.y = Mathf.Clamp(m_currentVelocity.y * m_currentBounce, float.MinValue, MaxBounceHeight);
				}
			}
			else
			{
				m_currentBounce = Bounciness;
                m_timeSinceBounce = 0.0f;
				m_currentVelocity.y *= m_currentBounce;
			}
        }
        else
        {
            this.MoveTransform(m_movementVector * Time.deltaTime);
        }
	}

	public void RollLoop() {
		//Raycast directly downward a certain distance. If there is no ground, then the ball is no longer grounded
		//If there is, project a vector onto that plane and roll along the plane.
	}

	public void MoveTransform(Vector3 movement) {
		if (Physics.Raycast(m_position, movement, out m_rayHit, movement.magnitude)) {
			this.transform.Translate((movement * -.2f) + (m_rayHit.normal * Collider.radius * 1.2f), Space.World);
		} else {
			this.transform.Translate(movement, Space.World);
		}
	}
}

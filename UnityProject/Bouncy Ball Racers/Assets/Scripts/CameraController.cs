using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {
	
	public Transform PlayerTransform;
	public Vector3 Displacement = Vector3.zero;
	public Vector3 FocalPoint = Vector3.zero;
	public float elasticity;
	public float cameraHeightClamp = 5.0f;
	
	private Vector3 m_targetPosition;
	private float m_focalPointHeight;

	// Use this for initialization
	void Start () {
		m_targetPosition = this.transform.position = 
		PlayerTransform.position +
		PlayerTransform.TransformVector(Displacement);

		this.transform.LookAt(
		PlayerTransform.position +
		PlayerTransform.TransformVector(FocalPoint));

		m_focalPointHeight = FocalPoint.y;
	}

	public void ChangeHeight (float heightChange) {
		FocalPoint.y = m_focalPointHeight =
			Mathf.Clamp(m_focalPointHeight + heightChange, -cameraHeightClamp, cameraHeightClamp);
	}
	
	// Update is called once per frame
	void Update () {
		m_targetPosition = 
		PlayerTransform.position +
		PlayerTransform.TransformVector(Displacement);

		this.transform.position = 
		Vector3.Lerp(this.transform.position, m_targetPosition, Time.deltaTime * elasticity);
		

		this.transform.LookAt(
		PlayerTransform.position +
		PlayerTransform.TransformVector(FocalPoint));
	}
}

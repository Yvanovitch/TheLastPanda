using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingInertia : MonoBehaviour {

	private float m_velocity = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A))
			SetInertia (1);
		
		if (m_velocity == 0)
			return;
		if (m_velocity <= 0.0001f)
			m_velocity = 0;

		this.transform.rotation *= Quaternion.Euler (0, m_velocity, 0);
		m_velocity -= 0.9f * Time.deltaTime;
	}

	public void SetInertia (float velocity) {
		m_velocity = velocity;
	}
}

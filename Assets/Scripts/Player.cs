using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	private static Player s_Instance;
	public static Player Get() { return s_Instance; }

	public event UnityAction OnDeath;

	private Rigidbody2D m_RigidBody;

	[SerializeField]
	private float m_Speed = 2;

	[SerializeField]
	private GameObject m_DeathParticals;

	private float m_StartHeight;

	private void Awake()
	{
		if (s_Instance)
		{
			Destroy(gameObject);
			return;
		}

		s_Instance = this;
	}

	void Start()
	{
		m_RigidBody = GetComponent<Rigidbody2D>();
		m_StartHeight = transform.position.y;
	}

	private void FixedUpdate()
	{
		float x = Input.GetAxisRaw("Horizontal");
		Vector3 pos = transform.position;
		pos.x += x * m_Speed * Time.deltaTime;
		pos.y = m_StartHeight;
		transform.position = pos;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Die();
	}

	private void Die()
	{
		OnDeath.Invoke();
		GameObject particle = Instantiate(m_DeathParticals);
		particle.transform.position = transform.position;
		Destroy(gameObject);
	}
}

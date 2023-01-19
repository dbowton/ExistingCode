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
	private int lives = 3;

	[SerializeField]
	private GameObject m_DeathParticals;
	public GameObject m_HitParticals;

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
		if (collision.gameObject.tag == "Wall") return;

		lives--;

        if (lives < 0)
		{
			GameObject particle = Instantiate(m_HitParticals);
			particle.transform.position = transform.position;
		}
		
        if (lives<= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		OnDeath.Invoke();
		GameObject particle = Instantiate(m_DeathParticals);
		particle.transform.position = transform.position;
		Destroy(gameObject);
	}
}


// walls don't kill and 3 lives 
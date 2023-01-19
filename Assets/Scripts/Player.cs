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
	private float xInput;

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

	private void Update()
    {
		xInput = Input.GetAxisRaw("Horizontal");
	}

	private void FixedUpdate()
	{
		Vector3 pos = transform.position;
		pos.x += xInput * m_Speed * Time.deltaTime;
		pos.y = m_StartHeight;
		transform.position = pos;
	}

	public void OnTriggerEnter2D(Collider2D col)
    {
		switch (col.gameObject.tag)
		{
			case "BasicEnemy":
				Hit();
				break;
			case "MustMoveEnemy":
				if (xInput == 0) Hit();
				break;
			case "StayStillEnemy":
				if (xInput > 0) Hit();
				break;	
		}
	}

    private void Hit()
	{
		lives--;

		if(lives > 0)
        {
			GameObject hitParticle = Instantiate(m_HitParticals);
			hitParticle.transform.position = transform.position;
        } 
		else
        {
			OnDeath.Invoke();
			GameObject particle = Instantiate(m_DeathParticals);
			particle.transform.position = transform.position;
			Destroy(gameObject);
		}
	}
}
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
	private SpriteRenderer m_SpriteRenderer ;
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
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
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
				if (xInput != 0) Hit();
				break;
			case "InstakillEnemy":
				Die();
				break;
			case "HealthBackEnemy":
				if (lives < 3) lives++;
				updateColor();
				break;
			case "KeyEnemy":
				col.gameObject.GetComponent<KeyEnemy>().Activate();
				break;
			case "VictoryEnemy":
				PlayerPrefs.SetInt("Unlocked Level", Mathf.Max(PlayerPrefs.GetInt("Unlocked Levels"), GameManager.Get().currentLevel + 1));
				Die();
				break;
		}
	}

    private void Hit()
	{
		lives--;
		updateColor();

		if(lives > 0)
        {
			GameObject hitParticle = Instantiate(m_HitParticals);
			hitParticle.transform.position = transform.position;
        } 
		else
        {
			Die();
		}
	}

	private void updateColor()
	{
		if(lives ==1) m_SpriteRenderer.material.color = new Color (207/255f,43/255f,79/255f);
		if(lives ==2) m_SpriteRenderer.material.color = new Color(241/255f,142/255f,164/255f);
		if(lives ==3) m_SpriteRenderer.material.color = Color.white;
		
	}

	private void Die()
	{
		OnDeath.Invoke();
		GameObject particle = Instantiate(m_DeathParticals);
		particle.transform.position = transform.position;
		Destroy(gameObject);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[ExecuteInEditMode]
public class MapController : MonoBehaviour, INotificationReceiver
{
	private static MapController s_Instance;
	public static MapController Get() { return s_Instance; }

	[SerializeField]
	private Transform m_Track;
	[SerializeField]
	private GameObject m_LeftWall;
	[SerializeField]
	private GameObject m_RightWall;

	[SerializeField]
	private AudioVisualizer m_Visualizer;

	[SerializeField]
	private float m_SpawnHeight;
	[SerializeField]
	private float m_Speed = 1;


	[SerializeField]
	private float m_MaxAMP = 450;
	[SerializeField]
	private float m_MinAMP = 100;

	[SerializeField]
	private float m_MaxWallDist = 10;
	[SerializeField]
	private float m_MinWallDist = 6;

	[SerializeField, Range(0,1)]
	private float m_Closed = 0.5f;

	[SerializeField]
	private GameObject m_DeathMenu;

	[SerializeField]
	private GameObject m_VictoryMenu;

	private PlayableDirector m_Director;

	[SerializeField]
	private float m_DecayTime;
	private float m_DecayTimer;
	private bool m_PlayerDead = false;
	private bool m_PlayerWon = false;
	public bool IsPlayerDead() { return m_PlayerDead; }
	public bool HasPlayerWon() { return m_PlayerWon; }


	private void Start()
	{
		if(s_Instance)
		{
			Destroy(gameObject);
			return;
		}

		s_Instance = this;

		if(Player.Get())
			Player.Get().OnDeath += OnPlayerDeath;

		if (Player.Get())
			Player.Get().OnWin += OnPlayerWon;

		m_Director = GetComponent<PlayableDirector>();
	}

	private void Update()
	{
		m_Track.position = new Vector3(0, (float)(m_Director.time * m_Speed), 0);

		UpdateClosed();

		if((m_PlayerDead || m_PlayerWon) && UnityEditor.EditorApplication.isPlaying)
		{
			m_DecayTimer -= Time.deltaTime;
			if (m_DecayTimer < 0) m_DecayTime = 0;
			if(m_PlayerDead && m_Director.state == PlayState.Playing)
				m_Director.playableGraph.GetRootPlayable(0).SetSpeed(Mathf.Clamp(m_DecayTimer / m_DecayTime, 0, 1));

			if (m_DecayTimer <= 0)
			{
				if(m_PlayerDead)
					m_DeathMenu.SetActive(true);
				else
					m_VictoryMenu.SetActive(true);
			}
		}
	}

	private void UpdateClosed()
	{
		// walls
		float wallDist = Mathf.Lerp(m_MinWallDist, m_MaxWallDist, m_Closed);
		m_LeftWall.transform.position = new Vector3(-wallDist, 0, 0);
		m_RightWall.transform.position = new Vector3(wallDist, 0, 0);

		// visualizer
		float amp = Mathf.Lerp(m_MaxAMP, m_MinAMP, m_Closed);
		m_Visualizer.SetAMP(amp);
	}

	private void OnPlayerDeath()
	{
		m_DecayTimer = m_DecayTime;
		m_PlayerDead = true;
	}

	private void OnPlayerWon()
	{
		m_DecayTimer = m_DecayTime;
		m_PlayerDead = true;
	}

	public void OnNotify(Playable origin, INotification notification, object context)
	{
		if(notification is EnemyMarker)
		{
			EnemyMarker em = notification as EnemyMarker;
			foreach(var ep in em.m_SpawnProps)
			{
				GameObject eg = Instantiate(ep.prefab, m_Track);
				eg.transform.position = new Vector3(ep.location, m_SpawnHeight - (float)(em.time / m_Speed), 0);
			}
		}
	}

	public void ClearTrack()
	{
		while(m_Track.childCount != 0)
			DestroyImmediate(m_Track.GetChild(0).gameObject);
	}
}

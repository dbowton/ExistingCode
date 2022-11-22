using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class PauseMenu : MonoBehaviour
{
	private static PauseMenu s_Instance;
	public static PauseMenu Get() { return s_Instance; }

	[SerializeField]
	private GameObject m_Menu;

	[SerializeField]
	private PlayableDirector m_Director;

	private bool m_IsPaused = false;

	public bool IsPaused { get { return m_IsPaused; } }

	private void Start()
	{
		if(s_Instance)
		{
			Destroy(gameObject);
			return;
		}

		s_Instance = this;
	}

	public void OnRestart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnContinue()
	{
		m_IsPaused = false;
		m_Menu.SetActive(false);
		m_Director.Play();
		RetroEffect.Get().StartFlicker();
	}

	public void OnMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void OnExit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && !MapController.Get().IsPlayerDead())
		{
			m_IsPaused = !m_IsPaused;
			m_Menu.SetActive(m_IsPaused);
			if (m_IsPaused)
				m_Director.Pause();
			else
				m_Director.Play();
			RetroEffect.Get().StartFlicker();
		}
	}
}

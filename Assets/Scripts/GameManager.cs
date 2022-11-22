using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager s_Instance;
	public static GameManager Get() { return s_Instance; }

	private bool m_Loaded = false;
	private float m_LoadMainMenuTime = 0;

	void Start()
	{
		if(s_Instance)
		{
			Destroy(gameObject);
			return;
		}

		s_Instance = this;
		DontDestroyOnLoad(gameObject);
		SceneManager.activeSceneChanged += OnSceneChange;
		m_LoadMainMenuTime = Time.time + 1;
	}

	private void OnSceneChange(Scene o, Scene n)
	{
		if(MainMenu.Get())
			MainMenu.Get().gameObject.SetActive(true);
	}

	private void Update()
	{
		if ((Time.time >= m_LoadMainMenuTime || Input.anyKeyDown) && !m_Loaded)
		{
			if(MainMenu.Get())
				MainMenu.Get().gameObject.SetActive(true);
			m_Loaded = true;
		}

	}

}

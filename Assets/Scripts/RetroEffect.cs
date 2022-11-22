using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RetroEffect : MonoBehaviour
{
	private static RetroEffect s_Instance;
	public static RetroEffect Get() { return s_Instance; }

	[SerializeField]
	private Material m_Material;

	[SerializeField]
	private float m_MaxTime = 0.3f;
	private float m_Timer = 0;
	private float m_YOffset = 0;
	private float m_Brightness = 1;

	[SerializeField, Range(0, 10)]
	private float m_SmallSpeed = 1;
	[SerializeField, Range(0, 5000)]
	private float m_SmallFrequancy = 2500;
	[SerializeField, Range(0, 2)]
	private float m_SmallLow = 0.5f;
	[SerializeField, Range(0, 2)]
	private float m_SmallHigh = 1;

	[SerializeField, Range(0, 10)]
	private float m_LargeSpeed = 2;
	[SerializeField, Range(0, 5000)]
	private float m_LargeFrequancy = 500;
	[SerializeField, Range(0, 2)]
	private float m_LargeLow = 0.5f;
	[SerializeField, Range(0, 2)]
	private float m_LargeHigh = 1;

	[SerializeField]
	bool m_FlickerOnStart = false;

	[SerializeField]
	private float m_JiterRate = 0;
	[SerializeField]
	private float m_JiterAmmout = 0;

	[System.Serializable]
	private struct FlickerStage
	{
		public float percent;
		public float offset;
	}

	[SerializeField]
	private List<FlickerStage> m_FlickerStages;

	private float QueryFliker(float percent)
	{
		for(int i = m_FlickerStages.Count-1; i >= 0; i--)
		{
			if(percent < m_FlickerStages[i].percent)
				return m_FlickerStages[i].offset;
		}

		return 0.0f;
	}

	private void Start()
	{
		if(s_Instance)
		{
			Destroy(gameObject);
			return;
		}
		s_Instance = this;

		if (m_FlickerOnStart)
			StartFlicker();
	}

	private void Update()
	{
		m_Timer -= Time.deltaTime;
		m_Timer = Mathf.Clamp(m_Timer, 0, m_MaxTime);
		m_YOffset = QueryFliker(m_Timer/m_MaxTime);
		m_YOffset += (2 * Mathf.PerlinNoise(Time.time * m_JiterRate, 0) - 1) * m_JiterAmmout;

		m_Brightness = 1f - (m_Timer / m_MaxTime) * 1.5f;
		m_Brightness = Mathf.Clamp01(m_Brightness);

		//m_Material.SetTexture("_MainTex", source);

		m_Material.SetFloat("smallSpeed", m_SmallSpeed);
		m_Material.SetFloat("smallFrequency", m_SmallFrequancy);
		m_Material.SetFloat("smallLow", m_SmallLow);
		m_Material.SetFloat("smallHigh", m_SmallHigh);

		m_Material.SetFloat("largeSpeed", m_LargeSpeed);
		m_Material.SetFloat("largeFrequency", m_LargeFrequancy);
		m_Material.SetFloat("largeLow", m_LargeLow);
		m_Material.SetFloat("largeHigh", m_LargeHigh);

		m_Material.SetFloat("yOff", m_YOffset);
		m_Material.SetFloat("brightness", m_Brightness);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		source.wrapMode = TextureWrapMode.Repeat;
		
		Graphics.Blit(source, destination, m_Material);
	}

	public void StartFlicker()
	{
		m_Timer = m_MaxTime;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class AudioVisualizer : MonoBehaviour
{
	private AudioSource m_Source;

	[SerializeField]
	private Transform m_LeftVisualizer;
	[SerializeField]
	private Transform m_RightVisualizer;

	private float[] m_LeftSamples = new float[512];
	private float[] m_RightSamples = new float[512];

	[SerializeField, Range(0,1)]
	private float m_MaxFreqP;

	[SerializeField, Range(0, 1000)]
	private float m_AMP = 700;

	[SerializeField]
	private VisualEffect m_ParticalSystem;
	private Texture2D m_FFTSampleTexture;

	void Start()
	{
		m_Source = GetComponent<AudioSource>();
		m_FFTSampleTexture = new Texture2D(m_LeftVisualizer.childCount, 1, TextureFormat.RFloat, false);
		m_ParticalSystem.SetTexture(Shader.PropertyToID("FFTSamples"), m_FFTSampleTexture);
	}

	void Update()
	{
		if (!PauseMenu.Get() || !PauseMenu.Get().IsPaused)
		{
			m_Source.GetSpectrumData(m_LeftSamples, 0, FFTWindow.BlackmanHarris);
			m_Source.GetSpectrumData(m_RightSamples, 1, FFTWindow.BlackmanHarris);

			float[] bands = UpdateVisualizer(m_LeftSamples, m_LeftVisualizer);
			UpdateVisualizer(m_RightSamples, m_RightVisualizer);

			m_FFTSampleTexture.SetPixelData<float>(bands, 0);
			m_FFTSampleTexture.Apply();
		}
	}

	private float[] UpdateVisualizer(float[] data, Transform visualizer)
	{
		int elements = visualizer.childCount;
		float[] avrages = new float[elements];
		int samples = (int)(data.Length * (m_MaxFreqP)) / elements;
		for(int i = 0; i < avrages.Length; i++)
		{
			float val = 0;
			for(int j = (i*samples); j < (i+1)*samples; j++)
				val += data[j];
			avrages[i] = val/samples;
		}

		for (int i = 0; i < elements; i++)
		{
			RectTransform bar = (RectTransform)visualizer.GetChild(i);
			Vector2 size = bar.sizeDelta;
			size.x = Mathf.Sqrt(avrages[i]) * m_AMP * Mathf.Log((i+1)*100, 2);
			bar.sizeDelta = size;
		}

		return avrages;
	}

	public void SetAMP(float amp)
	{
		m_AMP = amp;
	}
}

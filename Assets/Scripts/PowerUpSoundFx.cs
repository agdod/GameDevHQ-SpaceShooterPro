using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSoundFx : MonoBehaviour
{
	[SerializeField] private AudioClip _powerUpSoundFx;

	[SerializeField] private AudioSource _audioSourceFx;

	private void Start()
	{
		Debug.Log("init _audioSource componet");
		_audioSourceFx = GetComponent<AudioSource>();
		if (_audioSourceFx == null)
		{
			Debug.LogError("No AudioSource attached to component.");
		}
	}

	public void PlaySoundFX()
	{
		_audioSourceFx.clip = _powerUpSoundFx;
		_audioSourceFx.Play();
	}
}

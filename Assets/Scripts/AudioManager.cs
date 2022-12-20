using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	// Audio players components.
	public AudioSource EffectsSource;
	public AudioSource MusicSource;
	// Random pitch adjustment range.
	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;

	// Play a single clip through the sound effects source.
	public void PlayEffect(AudioClip clip)
	{
        EffectsSource.PlayOneShot(clip);
	}
	// Play a single clip through the music source.
	public void PlayMusic(AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		MusicSource.clip = clips[randomIndex];
		MusicSource.Play();
	}
	// Play a random clip from an array, and randomize the pitch slightly.
	public void RandomSoundEffect(params AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
		EffectsSource.pitch = randomPitch;
		EffectsSource.clip = clips[randomIndex];
        EffectsSource.Play();
	}
	public void StopMusic()
	{
		MusicSource.Stop();
	}
	public void SetVolume(float volume)
	{
		EffectsSource.volume = volume;
		MusicSource.volume = volume;
	}
}

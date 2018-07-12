using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public static class SoundHandler
	{
		#region Fields

		private static AudioSource audioPlayer = null;

		private static string audioPlayerName = "SoundHandler AudioPlayer";

		#endregion
		#region Properties

		public static AudioSource Source
		{
			get { return audioPlayer; } 
		}

		#endregion
		#region Methods

		public static void initialize()
		{
			if(audioPlayer == null)
			{
				// See if there already is an audio player in scene:
				GameObject go = GameObject.Find(audioPlayerName);

				if(go == null)
				{
					// Create a new audio source, if none was found:
					go = new GameObject(audioPlayerName);
					audioPlayer = go.AddComponent<AudioSource>();
				}
				else
				{
					audioPlayer = go.GetComponent<AudioSource>();
				}

				// Make sure no more sounds are playing:
				audioPlayer.Stop();
			}
		}
		public static void shutdown()
		{
			if(audioPlayer != null)
			{
				audioPlayer.Stop();
				Object.Destroy(audioPlayer.gameObject);
			}
		}

		public static void playOneShot(AudioClip clip, float volume = 1.0f)
		{
			if(clip != null)
			{
				audioPlayer.PlayOneShot(clip, volume);
			}
		}

		public static void playBackgroundMusic(AudioClip track)
		{
			// Stop any active tracks:
			audioPlayer.Stop();

			// Assign the new track and play it looped:
			audioPlayer.clip = track;
			audioPlayer.loop = true;
			audioPlayer.Play();
		}

		#endregion
	}
}

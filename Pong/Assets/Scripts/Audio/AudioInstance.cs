using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioInstance : MonoBehaviour
    {
        public AudioType m_instanceType = AudioType.MUSIC;

        private AudioSource m_audioSource = null;

        private float m_baseVolume = 1;

        // Start is called before the first frame update
        void Start()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_baseVolume = m_audioSource.volume;

            CalculateAudioLevel();
        }

        void CalculateAudioLevel()
        {
            // Retrieve the audio data and see if it is mute
            AudioData _audioData = AudioManager.GetAudioLevel(m_instanceType);
            float _audioLevel = _audioData.m_audioLevel * System.Convert.ToInt32(!_audioData.m_isMute);

            m_audioSource.volume = m_baseVolume * _audioLevel;
        }

        private void Update()
        {
            CalculateAudioLevel();
        }
    }
}
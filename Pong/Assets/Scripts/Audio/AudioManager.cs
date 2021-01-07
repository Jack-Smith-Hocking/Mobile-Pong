using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
using UnityEngine;

namespace Pong
{
    public enum AudioType
    {
        MUSIC,
        SFX
    }

    public struct AudioData
    {
        public bool m_isMute;
        public float m_audioLevel;

        public AudioData(bool isMute, float audioLevel)
        {
            m_isMute = isMute;
            m_audioLevel = audioLevel;
        }
    }

    public class AudioManager : MonoBehaviour, ISave
    {
        private static Dictionary<AudioType, AudioData> m_audioLevels = new Dictionary<AudioType, AudioData>();
        private static AudioManager Instance = null;

        private static string m_audioDataPath = ".txt";

        public AudioType m_managedAudioType = AudioType.MUSIC;

        private AudioData m_audioData = new AudioData(false, 1);

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            SaveManager.Instance.m_saveDatas.Add(this);
        }

        public void ToggleMuteStatus()
        {
            SetMuteStatus(!m_audioData.m_isMute);
        }
        public void SetMuteStatus(bool muteStatus)
        {
            m_audioData.m_isMute = muteStatus;

            m_audioLevels[m_managedAudioType] = m_audioData;
        }
        public void SetAudioLevel(float audioLevel)
        {
            m_audioData.m_audioLevel = audioLevel;

            m_audioLevels[m_managedAudioType] = m_audioData;
        }

        public static AudioData GetAudioLevel(AudioType audioType)
        {
            if (m_audioLevels.ContainsKey(audioType))
            {
                return m_audioLevels[audioType];
            }

            return new AudioData(false, 1);
        }

        public void Save()
        {
        }
    }
}
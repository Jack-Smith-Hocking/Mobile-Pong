using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Pong
{
    public enum AudioType
    {
        MUSIC,
        SFX
    }

    public struct AudioData
    {
        public static string Mute_State_Pref { get { return "Mute_State"; } }
        public static string Volume_Level_Pref { get { return "Volume_Level"; } }

        public bool m_isMute;
        public float m_audioLevel;

        public AudioData(bool isMute, float audioLevel)
        {
            m_isMute = isMute;
            m_audioLevel = audioLevel;
        }
    }

    public class AudioManager : MonoBehaviour, ISaveLoad
    {
        public Button m_muteButton = null;
        public Slider m_volumeSlider = null;

        public AudioType m_managedAudioType = AudioType.MUSIC;

        private AudioData m_audioData = new AudioData(false, 1);

        private void Awake()
        {
            SaveManager.m_managed.Add(this);

            SaveDefaultAudio();
            Load();
        }

        private void Start()
        {
            if (m_muteButton)
            {
                if (m_audioData.m_isMute)
                {
                    m_muteButton.onClick.Invoke();
                }

                m_muteButton.onClick.AddListener(() =>
                {
                    ToggleMuteStatus();
                });
            }
            if (m_volumeSlider)
            {
                m_volumeSlider.value = m_audioData.m_audioLevel;

                m_volumeSlider.onValueChanged.AddListener((float val) =>
                {
                    SetAudioLevel(val);
                });
            }
        }

        public void ToggleMuteStatus()
        {
            SetMuteStatus(!m_audioData.m_isMute);
        }
        public void SetMuteStatus(bool muteStatus)
        {
            m_audioData.m_isMute = muteStatus;

            Save();
        }
        public void SetAudioLevel(float audioLevel)
        {
            m_audioData.m_audioLevel = audioLevel;

            Save();
        }

        #region SaveLoad
        public void Save()
        {
            PlayerPrefs.SetString(m_managedAudioType.ToString(), m_managedAudioType.ToString());

            string _prefix = m_managedAudioType.ToString() + "_";

            PlayerPrefs.SetInt(_prefix + AudioData.Mute_State_Pref, System.Convert.ToInt32(m_audioData.m_isMute));
            PlayerPrefs.SetFloat(_prefix + AudioData.Volume_Level_Pref, m_audioData.m_audioLevel);

            PlayerPrefs.Save();
        }
        void SaveDefaultAudio()
        {
            if (!PlayerPrefs.HasKey(m_managedAudioType.ToString()))
            {
                PlayerPrefs.SetString(m_managedAudioType.ToString(), m_managedAudioType.ToString());

                string _prefix = m_managedAudioType.ToString() + "_";

                PlayerPrefs.SetInt(_prefix + AudioData.Mute_State_Pref, 0);
                PlayerPrefs.SetFloat(_prefix + AudioData.Volume_Level_Pref, 1f);
            }
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(m_managedAudioType.ToString()))
            {
                string _prefix = m_managedAudioType.ToString() + "_";

                m_audioData.m_isMute = System.Convert.ToBoolean(PlayerPrefs.GetInt(_prefix + AudioData.Mute_State_Pref));
                m_audioData.m_audioLevel = PlayerPrefs.GetFloat(_prefix + AudioData.Volume_Level_Pref);
            }
        }
        public static AudioData LoadAudioData(AudioType audioType)
        {
            AudioData _audioData = new AudioData(false, 1);

            string _prefix = audioType.ToString() + "_";
            _audioData.m_isMute = System.Convert.ToBoolean(PlayerPrefs.GetInt(_prefix + AudioData.Mute_State_Pref));
            _audioData.m_audioLevel = PlayerPrefs.GetFloat(_prefix + AudioData.Volume_Level_Pref);

            return _audioData;
        }
    }
    #endregion
}

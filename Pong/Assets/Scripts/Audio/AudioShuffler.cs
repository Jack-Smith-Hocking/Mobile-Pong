using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioShuffler : MonoBehaviour
{
    public List<AudioClip> m_clips = new List<AudioClip>();
    public bool m_allowFadIn = true;
    [Range(0, 1)] public float m_fadeInPercent;
    [Range(0, 1)] public float m_fadeOutPercent;

    private float m_baseVolume;

    private AudioSource m_audioSource;

    private AudioClip m_previousClip = null;

    public void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_baseVolume = m_audioSource.volume;

        ChangeClips(GetRandomClip());
    }

    void ChangeClips(AudioClip newClip)
    {
        if (newClip != null)
        {
            m_previousClip = m_audioSource.clip;
            m_audioSource.clip = newClip;

            if (m_allowFadIn)
            {
                float _fadeInTime = m_audioSource.clip.length * m_fadeInPercent;
                float _fadeIn = (m_audioSource.time / _fadeInTime);
                m_audioSource.volume = m_baseVolume * _fadeIn;
            }

            m_audioSource.Play();
        }
    }

    private void Update()
    {
        if (m_audioSource.clip != null)
        {
            if (m_allowFadIn)
            {
                float _fadeInTime = m_audioSource.clip.length * m_fadeInPercent;
                if (m_audioSource.time < _fadeInTime)
                {
                    float _fadeIn = (m_audioSource.time / _fadeInTime);
                    m_audioSource.volume = m_baseVolume * _fadeIn;
                }

                float _fadeOutTime = m_audioSource.clip.length * m_fadeOutPercent;
                if (m_audioSource.time > (m_audioSource.clip.length - _fadeOutTime))
                {
                    float _currentTime = m_audioSource.clip.length - m_audioSource.time;
                    float _fadeOut = (_currentTime / _fadeOutTime);
                    m_audioSource.volume = m_baseVolume * _fadeOut;
                }
            }

            if (!m_audioSource.isPlaying)
            {
                ChangeClips(GetRandomClip());
            }
        }
    }

    AudioClip GetRandomClip()
    {
        if (m_clips.Count == 1)
        {
            return m_clips[0];
        }
        else if (m_clips.Count == 0)
        {
            return null;
        }

        AudioClip _randClip = null;

        GeneralUtil.SafeDoWhile(
            // Action
            () =>
            {
                _randClip = m_clips[Random.Range(0, m_clips.Count)];
            },
            // Predicate
            () =>
            {
                return _randClip.Equals(m_previousClip);
            }, 100, false);

        return _randClip;
    }
}

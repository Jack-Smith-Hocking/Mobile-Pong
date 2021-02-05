using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pong
{
    public class HighScore : MonoBehaviour, ISaveLoad
    {
        public static string High_Score_Pref { get { return "High_Score"; } }

        public static HighScore Instance = null;

        public TextMeshProUGUI m_highscoreText;
        public int m_highScore = 0;

        public string m_scoreHolder;

        private void Awake()
        {
            SaveManager.m_managed.Add(this);

            if (Instance == null)
            {
                Instance = this;
            }
        }
        private void Start()
        {
            if (m_highscoreText)
            {
                m_highscoreText.text = "High Score: " + m_highScore.ToString();
            }
        }

        public void UpdateHighscore(int highscore)
        {
            if (highscore > m_highScore)
            {
                m_highScore = highscore;

                if (m_highscoreText)
                {
                    m_highscoreText.text = "High Score: " + m_highScore.ToString();
                }

                Save();
            }
        }

        #region SaveLoad
        public void Save()
        {
            string _prefPath = m_scoreHolder + "_" + High_Score_Pref;

            PlayerPrefs.SetInt(_prefPath, m_highScore);
        }

        public void Load()
        {
            LoadHighScore("");
        }
        public void LoadHighScore(string scoreHolder)
        {
            m_scoreHolder = scoreHolder;

            string _prefPath = m_scoreHolder + "_" + High_Score_Pref;

            if (PlayerPrefs.HasKey(_prefPath))
            {
                m_highScore = PlayerPrefs.GetInt(_prefPath);
            }
        }
        #endregion
    }
}
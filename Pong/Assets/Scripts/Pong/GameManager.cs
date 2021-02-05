using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Pong
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        public Dictionary<PlayerID, int> m_playerScores = new Dictionary<PlayerID, int>();

        public bool m_canWin = true;
        public int m_scoreToWin = 10;
        public TextMeshProUGUI m_winnerText = null;
        public UnityEvent m_onWinEvent;

        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                m_winnerText?.gameObject.SetActive(false);
            }
        }

        public void ChangeScoreToWin(string scoreToWin)
        {
            m_scoreToWin = int.Parse(scoreToWin);
        }

        #region PlayerOne
        public void DisplayScoreP1(TextMeshProUGUI score)
        {
            if (score != null)
            {
                if (m_playerScores.ContainsKey(PlayerID.PLAYER_ONE))
                {
                    score.text = m_playerScores[PlayerID.PLAYER_ONE].ToString();
                }
            }
        }
        public void IncrementP1()
        {
            IncrementScore(PlayerID.PLAYER_ONE, "Player One");

            HighScore.Instance.UpdateHighscore(m_playerScores[PlayerID.PLAYER_ONE]);
        }
        #endregion

        public void IncrementScore(PlayerID player, string playerName)
        {
            if (m_playerScores.ContainsKey(player))
            {
                m_playerScores[player] += 1;
            }
            else
            {
                m_playerScores[player] = 1;
            }

            if (m_scoreToWin > 0 && m_playerScores[player] >= m_scoreToWin)
            {
                if (m_canWin)
                {
                    WinGame(playerName);
                }
            }
        }

        private void WinGame(string winner)
        {
            m_winnerText.gameObject.SetActive(true);
            m_winnerText.text = winner + " Won!!!!!";

            m_onWinEvent.Invoke();

            Time.timeScale = 0;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pong
{
    public class PlayerScore : MonoBehaviour
    {
        public TextMeshProUGUI m_playerText;
        public string m_scorePrefix = "Score: ";
        public string m_playerName = "Player One";
        public PlayerID m_playerID = PlayerID.PLAYER_ONE;

        private int m_score = 0;

        private void Start()
        {
            m_playerText.text = m_scorePrefix + m_score;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.Equals(Ball.Instance.gameObject))
            {
                StartCoroutine(DelayedScoreUpdate());
            }
        }

        IEnumerator DelayedScoreUpdate()
        {
            yield return new WaitForEndOfFrame();

            GameManager.Instance.IncrementScore(m_playerID, m_playerName);
            m_playerText.text = m_scorePrefix + GameManager.Instance.m_playerScores[m_playerID];

            Ball.Instance.ResetBall();
            Ball.Instance.AddRandomForce(1, false);
        }
    }
}
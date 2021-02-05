using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pong
{
    public class MatchSetup : MonoBehaviour
    {
        public MatchSettings m_matchSettings;

        [Header("Score Details")]
        public TextMeshProUGUI m_scoreText = null;

        [Header("Player One")]
        public AIPaddle m_paddleOneAI = null;
        public PlayerPaddle m_paddleOnePlayer = null;

        [Header("Player Two")]
        public AIPaddle m_paddleTwoAI = null;
        public PlayerPaddle m_paddleTwoPlayer = null;

        public void Awake()
        {
            m_matchSettings = Settings.LoadMatchSettings();

            UpdatePaddle(m_paddleOneAI, m_paddleOnePlayer, m_matchSettings.m_playerOneState);
            UpdatePaddle(m_paddleTwoAI, m_paddleTwoPlayer, m_matchSettings.m_playerTwoState);
        }
        private void Start()
        {
            GameManager.Instance.m_scoreToWin = Settings.m_matchSettings.m_scoreToWin;

            if (m_scoreText != null)
            {
                m_scoreText.text = $"Score To Win: {Settings.m_matchSettings.m_scoreToWin}";
            }
        }

        void UpdatePaddle(AIPaddle ai, PlayerPaddle player, PaddleState paddleState)
        {
            if (ai != null && player != null)
            {
                switch (paddleState)
                {
                    case PaddleState.AI:
                        ai.enabled = true;
                        player.enabled = false;
                        break;
                 
                    case PaddleState.PLAYER:
                        ai.enabled = false;
                        player.enabled = true;
                        break;
                    
                    default:
                        break;
                }
            }
        }
    }
}
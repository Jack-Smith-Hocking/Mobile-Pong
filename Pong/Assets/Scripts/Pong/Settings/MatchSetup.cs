using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class MatchSetup : MonoBehaviour
    {
        public MatchSettings m_matchSettings;

        [Header("Player One")]
        public AIPaddle m_paddleOneAI = null;
        public PlayerPaddle m_paddleOnePlayer = null;

        [Header("Player Two")]
        public AIPaddle m_paddleTwoAI = null;
        public PlayerPaddle m_paddleTwoPlayer = null;

        public void Awake()
        {
            Settings.LoadMatchSettings();
            m_matchSettings = Settings.m_matchSettings;

            UpdatePaddle(m_paddleOneAI, m_paddleOnePlayer, m_matchSettings.m_playerOneState);
            UpdatePaddle(m_paddleTwoAI, m_paddleTwoPlayer, m_matchSettings.m_playerTwoState);
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
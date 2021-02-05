using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
using UnityEngine;

namespace Pong
{
    public enum PaddleState
    {
        AI = 0,
        PLAYER = 1
    }

    public class PaddleManager : MonoBehaviour
    {
        public static Dictionary<PlayerID, PaddleState> PaddleStates = new Dictionary<PlayerID, PaddleState>();
        public static bool m_hasCleared = false;

        public PlayerID m_playerID = PlayerID.PLAYER_ONE;
        public PaddleState m_paddleState = PaddleState.PLAYER;
        public bool m_isMenu = false;
        [Space]
        public AIPaddle m_paddleAI = null;
        public PlayerPaddle m_paddlePlayer = null;

        public void Start()
        {
            if (m_isMenu && !m_hasCleared)
            {
                PaddleStates.Clear();
                m_hasCleared = true;
            }
            else
            {
                m_hasCleared = false;
            }

            if (!PaddleStates.ContainsKey(m_playerID))
            {
                PaddleStates[m_playerID] = m_paddleState;
            }
            else
            {
                UpdatePaddleState();
            }
        }

        public void SetPaddleAI()
        {
            SetPaddleState(PaddleState.AI);
        }
        public void SetPaddlePlayer()
        {
            SetPaddleState(PaddleState.PLAYER);
        }

        public void SetPaddleState(PaddleState paddleState)
        {
            PaddleStates[m_playerID] = paddleState;
        }

        public void UpdatePaddleState()
        {
            bool _isAI = PaddleStates[m_playerID] == PaddleState.AI;

            if (m_paddleAI != null) m_paddleAI.enabled = _isAI;
            if (m_paddlePlayer != null) m_paddlePlayer.enabled = !_isAI;

            //StartCoroutine(GeneralUtil.DelayedActionEOF(() => { PaddleStates.Clear(); }));
        }
    }
}
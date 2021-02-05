using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Custom.Utility;

namespace Pong
{
    [System.Serializable]
    public struct MatchSettings
    {
        public static string P1_State_Pref { get { return "P1_State"; } }
        public static string P2_State_Pref { get { return "P2_State"; } }
        public static string Win_Score_Pref { get { return "Win_Score"; } }

        public PaddleState m_playerOneState;
        public PaddleState m_playerTwoState;
        [Space]
        public int m_scoreToWin;
    }

    public class Settings : MonoBehaviour, ISaveLoad
    {
        public static MatchSettings m_matchSettings = new MatchSettings();

        [Header("Match UI")]
        public Button m_playerOneState = null;
        public Button m_playerTwoState = null;
        public string m_playerText = "Player";
        public string m_aiText = "A.I";
        [Space]
        public TMP_InputField m_winScoreInput = null;

        private TextMeshProUGUI m_playerOneStateText = null;
        private TextMeshProUGUI m_playerTwoStateText = null;

        private void Start()
        {
            SaveManager.m_managed.Add(this);

            InitMatchData();
        }

        void InitMatchData()
        {
            Load();

            m_playerOneStateText = m_playerOneState.GetComponentInChildren<TextMeshProUGUI>();
            m_playerTwoStateText = m_playerTwoState.GetComponentInChildren<TextMeshProUGUI>();

            StateButtonListener(m_playerOneState, m_playerOneStateText, PlayerID.PLAYER_ONE);
            StateButtonListener(m_playerTwoState, m_playerTwoStateText, PlayerID.PLAYER_TWO);

            m_winScoreInput.text = m_matchSettings.m_scoreToWin.ToString();
            m_winScoreInput.onValueChanged.AddListener((string val) =>
            {
                m_matchSettings.m_scoreToWin = int.Parse(val);
            });
        }

        #region PaddleStateHandlers
        PaddleState FlipStateText(TextMeshProUGUI stateText, PaddleState paddleState)
        {
            if (stateText)
            {
                switch (paddleState)
                {
                    case PaddleState.AI:
                        stateText.text = m_playerText;
                        return PaddleState.PLAYER;

                    case PaddleState.PLAYER:
                        stateText.text = m_aiText;
                        return PaddleState.AI;

                    default:
                        break;
                }
            }

            return PaddleState.PLAYER;
        }
        void SetStateText(TextMeshProUGUI stateText, PaddleState paddleState)
        {
            if (stateText)
            {
                switch (paddleState)
                {
                    case PaddleState.AI:
                        stateText.text = m_aiText;
                        break;

                    case PaddleState.PLAYER:
                        stateText.text = m_playerText;
                        break;

                    default:
                        break;
                }
            }
        }
        void HandleStateText(TextMeshProUGUI stateText, PlayerID playerID, bool flip = true)
        {
            PaddleState _paddleState =
                (playerID == PlayerID.PLAYER_ONE) ? m_matchSettings.m_playerOneState : m_matchSettings.m_playerTwoState;

            if (flip)
            {
                _paddleState = FlipStateText(stateText, _paddleState);
                if (playerID == PlayerID.PLAYER_ONE)
                {
                    m_matchSettings.m_playerOneState = _paddleState;
                }
                else
                {
                    m_matchSettings.m_playerTwoState = _paddleState;
                }
            }
            else
            {
                SetStateText(stateText, _paddleState);
            }
        }
        void StateButtonListener(Button stateButton, TextMeshProUGUI stateText, PlayerID playerID)
        {
            if (stateButton != null)
            {
                HandleStateText(stateText, playerID, false);

                stateButton.onClick.AddListener(() =>
                {
                    HandleStateText(stateText, playerID);
                });
            }
        }
        #endregion

        #region MatchSettings
        #region Save
        public static void SaveDefault()
        {
            SaveMatchSettings(1, 1, 5);
        }
        public void Save()
        {
            SaveMatchSettings((int)m_matchSettings.m_playerOneState, (int)m_matchSettings.m_playerTwoState, m_matchSettings.m_scoreToWin);
        }
        public static void SaveMatchSettings(int playerOneState, int playerTwoState, int winScore)
        {
            PlayerPrefs.SetInt(MatchSettings.P1_State_Pref, playerOneState);
            PlayerPrefs.SetInt(MatchSettings.P2_State_Pref, playerTwoState);
            PlayerPrefs.SetInt(MatchSettings.Win_Score_Pref, winScore);

            PlayerPrefs.Save();
        }
        #endregion

        #region Load
        public void Load()
        {
            if (!PlayerPrefs.HasKey(MatchSettings.P1_State_Pref))
            {
                SaveDefault();
            }

            m_matchSettings = new MatchSettings();

            m_matchSettings.m_playerOneState = (PaddleState)PlayerPrefs.GetInt(MatchSettings.P1_State_Pref);
            m_matchSettings.m_playerTwoState = (PaddleState)PlayerPrefs.GetInt(MatchSettings.P2_State_Pref);

            m_matchSettings.m_scoreToWin = PlayerPrefs.GetInt(MatchSettings.Win_Score_Pref);
        }
        public static MatchSettings LoadMatchSettings()
        {
            if (!PlayerPrefs.HasKey(MatchSettings.P1_State_Pref))
            {
                SaveDefault();
            }

            MatchSettings _settings = new MatchSettings();

            _settings.m_playerOneState = (PaddleState)PlayerPrefs.GetInt(MatchSettings.P1_State_Pref);
            _settings.m_playerTwoState = (PaddleState)PlayerPrefs.GetInt(MatchSettings.P2_State_Pref);

            _settings.m_scoreToWin = PlayerPrefs.GetInt(MatchSettings.Win_Score_Pref);

            return _settings;
        }
        #endregion
        #endregion
    }
}
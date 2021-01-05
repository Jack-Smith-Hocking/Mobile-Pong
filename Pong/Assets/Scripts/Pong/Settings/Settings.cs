﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

namespace Pong
{
    [System.Serializable]
    public struct MatchSettings
    {
        public PaddleState m_playerOneState;
        public PaddleState m_playerTwoState;
        [Space]
        public int m_scoreToWin;
    }

    public class Settings : MonoBehaviour
    {
        public static Settings Instance = null;

        public static MatchSettings m_matchSettings = new MatchSettings();
        public static string m_matchFilePath = ".txt";

        [Header("Match UI")]
        public Button m_playerOneState = null;
        public Button m_playerTwoState = null;
        public string m_playerText = "Player";
        public string m_aiText = "A.I";
        [Space]
        public InputField m_winScoreInput = null;

        private TextMeshProUGUI m_playerOneStateText = null;
        private TextMeshProUGUI m_playerTwoStateText = null;

        public void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            InitMatchData();
        }

        void InitMatchData()
        {
            m_matchFilePath = Application.persistentDataPath + ".txt";
            LoadMatchSettings();

            m_playerOneStateText = m_playerOneState.GetComponentInChildren<TextMeshProUGUI>();
            m_playerTwoStateText = m_playerTwoState.GetComponentInChildren<TextMeshProUGUI>();

            StateButtonListener(m_playerOneState, m_playerOneStateText, PlayerID.PLAYER_ONE);
            StateButtonListener(m_playerTwoState, m_playerTwoStateText, PlayerID.PLAYER_TWO);
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
        private void OnApplicationQuit()
        {
            SaveMatchSettings();
        }

        public static void LoadDefaultMatchSettings()
        {
            m_matchSettings = new MatchSettings();

            m_matchSettings.m_playerOneState = PaddleState.PLAYER;
            m_matchSettings.m_playerTwoState = PaddleState.PLAYER;

            m_matchSettings.m_scoreToWin = 10;

            StaticSaveMatchSettings();
        }
        public static void LoadMatchSettings()
        {
            bool _loadedFile = false;
            m_matchSettings = LoadSettings<MatchSettings>(m_matchFilePath, out _loadedFile);

            if (!_loadedFile)
            {
                LoadDefaultMatchSettings();
            }
        }
        public static void StaticSaveMatchSettings()
        {
            SaveSettings<MatchSettings>(m_matchSettings, m_matchFilePath);
        }
        public void SaveMatchSettings()
        {
            SaveSettings<MatchSettings>(m_matchSettings, m_matchFilePath);
        }
        #endregion

        static T LoadSettings<T>(string path, out bool loaded)
        {
            T _returnVal = default;

            if (File.Exists(path))
            {
                loaded = true;

                using (var _read = new StreamReader(path))
                {
                    string _data = _read.ReadToEnd();
                    _returnVal = JsonUtility.FromJson<T>(_data);
                }

                return _returnVal;
            }

            loaded = false;
            return _returnVal;
        }
        static void SaveSettings<T>(T settings, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Create(path).Dispose();

            string _serialisedData = JsonUtility.ToJson(settings, false);

            using (var _write = new StreamWriter(path))
            {
                _write.Write(_serialisedData);
            }
        }
    }
}
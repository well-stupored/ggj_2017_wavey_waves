﻿using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Networking
{
    class ServerListingSession
    {
        public ServerDetails Server;
        public Action<FindServerResult> OnSelected;
    }

    class ServerListing : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _gameTypeText;
        [SerializeField] private Text _playerCountText;
        [SerializeField] private Text _pingText;
        [SerializeField] private Image _backgroundImage;

        [SerializeField] private Button _selectionButton;

        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _hightlightColor;

        private ServerListingSession _currentSession;

        protected void Awake()
        {
            _selectionButton.onClick.AddListener(OnSelectionButtonClicked);
        }

        public void StartSession(ServerListingSession session)
        {
            if (_currentSession != null) session.OnSelected(new FindServerResult {WasSuccessful = false});

            _currentSession = session;

            _nameText.text = _currentSession.Server.LobbyName;
            _gameTypeText.text = _currentSession.Server.LobbyDescription;
            _playerCountText.text = string.Format("? / {0}",
                _currentSession.Server.MaxPlayers.ToString(CultureInfo.InvariantCulture));
            _pingText.text = string.Format("{0} ms", "?");
        }

        private void OnSelectionButtonClicked()
        {
            _currentSession.OnSelected(new FindServerResult {ServerName = _currentSession.Server.LobbyName, WasSuccessful = true});
            Highlight();
        }

        public void Highlight()
        {
            _backgroundImage.color = _hightlightColor;
        }

        public void UnHighlight()
        {
            _backgroundImage.color = _normalColor;
        }
    }
}

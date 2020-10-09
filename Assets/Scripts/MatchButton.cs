using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public enum  ButtonStatus
    {
        Locked,
        Placed,
        Removed,
        Invisible
    }

    public class MatchButton: MonoBehaviour
    {
        [SerializeField] private Button _matchButton;
        [SerializeField] private Image _buttonImage;
        
        [JsonProperty]
        public ButtonStatus _currentButtonStatus;
        
        public ButtonStatus CurrentButtonStatus
        {
            get => _currentButtonStatus;
            set
            {
                _currentButtonStatus = value;
                switch (value)
                {
                    case ButtonStatus.Invisible:
                        _buttonImage.color = new Color(1f, 1f, 1f, 0.2f);
                        break;
                    case ButtonStatus.Locked:
                        _buttonImage.color = new Color(1f, 1f, 1f, 1f);
                        break;
                    case ButtonStatus.Removed:
                        _buttonImage.color = new Color(0f, 0f, 1f, 0.5f);
                        break;
                    case ButtonStatus.Placed:
                        _buttonImage.color = new Color(1f, 0f, 0f, 0.5f);
                        break;
                }
            }
        }

        public ButtonStatus _currentButtonSolutionStatus;

        public ButtonStatus CurrentButtonSolutionStatus
        {
            get => _currentButtonSolutionStatus;
            set
            {
                _currentButtonSolutionStatus = value;
                switch (value)
                {
                    case ButtonStatus.Invisible:
                        _buttonImage.color = new Color(1f, 1f, 1f, 0.2f);
                        break;
                    case ButtonStatus.Locked:
                        _buttonImage.color = new Color(1f, 1f, 1f, 1f);
                        break;
                    case ButtonStatus.Placed:
                        _buttonImage.color = new Color(0f, 1f, 0f, 0.5f);
                        break;
                    case ButtonStatus.Removed:
                        _buttonImage.color = new Color(1f, 0f, 0f, 0.5f);
                        break;
                }
            }
        }

        public int _column;
        public int _row;

        public int Column
        {
            get => _column;
            set => _column = value;
        }

        public int Row
        {
            get => _row;
            set => _row = value;
        }
        
        private void Start()
        {
            CurrentButtonStatus = ButtonStatus.Locked;
            CurrentButtonSolutionStatus = ButtonStatus.Locked;
            SetInteractiveMethod(false);
        }

        public void SetInteractiveMethod(bool isAnswerMode)
        {
            _matchButton.onClick.RemoveAllListeners();
            if (!isAnswerMode)
            {
                _matchButton.onClick.AddListener(OnButtonPressed);
                switch (_currentButtonStatus)
                {
                    case ButtonStatus.Invisible:
                        _buttonImage.color = new Color(1f, 1f, 1f, 0.2f);
                        break;
                    case ButtonStatus.Locked:
                        _buttonImage.color = new Color(1f, 1f, 1f, 1f);
                        break;
                    case ButtonStatus.Removed:
                        _buttonImage.color = new Color(1f, 0f, 0f, 0.5f);
                        break;
                    case ButtonStatus.Placed:
                        _buttonImage.color = new Color(0f, 0f, 1f, 0.5f);
                        break;
                }
            }
            else
            {
                _matchButton.onClick.AddListener(OnSolutionButtonPressed);
                switch (_currentButtonSolutionStatus)
                {
                    case ButtonStatus.Invisible:
                        _buttonImage.color = new Color(1f, 1f, 1f, 0.2f);
                        break;
                    case ButtonStatus.Locked:
                        _buttonImage.color = new Color(1f, 1f, 1f, 1f);
                        break;
                    case ButtonStatus.Placed:
                        _buttonImage.color = new Color(0f, 0f, 1f, 0.5f);
                        break;
                    case ButtonStatus.Removed:
                        _buttonImage.color = new Color(1f, 0f, 0f, 0.5f);
                        break;
                }
            }
        }
        
        private void OnButtonPressed()
        {
            int currentButtonStatusInt = (int) CurrentButtonStatus;
            currentButtonStatusInt++;
            currentButtonStatusInt %= 4;
            CurrentButtonStatus = (ButtonStatus)currentButtonStatusInt;

            switch (CurrentButtonStatus)
            {
                case ButtonStatus.Invisible:
                    _currentButtonSolutionStatus = ButtonStatus.Invisible;
                    break;
                case ButtonStatus.Locked:
                    _currentButtonSolutionStatus = ButtonStatus.Locked;
                    break;
                default:
                    if (_currentButtonSolutionStatus == ButtonStatus.Invisible
                        || _currentButtonSolutionStatus == ButtonStatus.Locked)
                    {
                        _currentButtonSolutionStatus = ButtonStatus.Removed;
                    }
                    break;
            }
            
        }
        
        private void OnSolutionButtonPressed()
        {
            if (CurrentButtonStatus == ButtonStatus.Invisible 
                || CurrentButtonStatus == ButtonStatus.Locked)
            {
                return;
            }

            if (CurrentButtonSolutionStatus == ButtonStatus.Placed)
            {
                CurrentButtonSolutionStatus = ButtonStatus.Removed;
            }
            else
            {
                CurrentButtonSolutionStatus = ButtonStatus.Placed;
            }
        }

        public MatchButtonData GetData()
        {
            MatchButtonData matchButtonData = new MatchButtonData();
            matchButtonData.Rotation = (int)transform.rotation.eulerAngles.z;
            matchButtonData.Column = _column;
            matchButtonData.Row = _row;
            matchButtonData.InitialStatus = _currentButtonStatus.ToString();
            matchButtonData.FinalStatus = _currentButtonSolutionStatus.ToString();

            return matchButtonData;
        }
    }
}
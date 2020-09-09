using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public enum  ButtonStatus
    {
        Locked,
        CanRemove,
        CanPlace,
        Invisible
    }
    
    public enum  ButtonSolutionStatus
    {
        Locked,
        IsAnswer,
        IsNotAnswer,
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
                    case ButtonStatus.CanPlace:
                        _buttonImage.color = new Color(0f, 0f, 1f, 0.5f);
                        break;
                    case ButtonStatus.CanRemove:
                        _buttonImage.color = new Color(1f, 0f, 0f, 0.5f);
                        break;
                }
            }
        }

        public ButtonSolutionStatus _currentButtonSolutionStatus;

        public ButtonSolutionStatus CurrentButtonSolutionStatus
        {
            get => _currentButtonSolutionStatus;
            set
            {
                _currentButtonSolutionStatus = value;
                switch (value)
                {
                    case ButtonSolutionStatus.Invisible:
                        _buttonImage.color = new Color(1f, 1f, 1f, 0.2f);
                        break;
                    case ButtonSolutionStatus.Locked:
                        _buttonImage.color = new Color(1f, 1f, 1f, 1f);
                        break;
                    case ButtonSolutionStatus.IsAnswer:
                        _buttonImage.color = new Color(0f, 1f, 0f, 0.5f);
                        break;
                    case ButtonSolutionStatus.IsNotAnswer:
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
            CurrentButtonSolutionStatus = ButtonSolutionStatus.Locked;
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
                    case ButtonStatus.CanPlace:
                        _buttonImage.color = new Color(0f, 0f, 1f, 0.5f);
                        break;
                    case ButtonStatus.CanRemove:
                        _buttonImage.color = new Color(1f, 0f, 0f, 0.5f);
                        break;
                }
            }
            else
            {
                _matchButton.onClick.AddListener(OnSolutionButtonPressed);
                switch (_currentButtonSolutionStatus)
                {
                    case ButtonSolutionStatus.Invisible:
                        _buttonImage.color = new Color(1f, 1f, 1f, 0.2f);
                        break;
                    case ButtonSolutionStatus.Locked:
                        _buttonImage.color = new Color(1f, 1f, 1f, 1f);
                        break;
                    case ButtonSolutionStatus.IsAnswer:
                        _buttonImage.color = new Color(0f, 1f, 0f, 0.5f);
                        break;
                    case ButtonSolutionStatus.IsNotAnswer:
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
                    _currentButtonSolutionStatus = ButtonSolutionStatus.Invisible;
                    break;
                case ButtonStatus.Locked:
                    _currentButtonSolutionStatus = ButtonSolutionStatus.Locked;
                    break;
                default:
                    if (_currentButtonSolutionStatus == ButtonSolutionStatus.Invisible
                        || _currentButtonSolutionStatus == ButtonSolutionStatus.Locked)
                    {
                        _currentButtonSolutionStatus = ButtonSolutionStatus.IsNotAnswer;
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

            if (CurrentButtonSolutionStatus == ButtonSolutionStatus.IsAnswer)
            {
                CurrentButtonSolutionStatus = ButtonSolutionStatus.IsNotAnswer;
            }
            else
            {
                CurrentButtonSolutionStatus = ButtonSolutionStatus.IsAnswer;
            }
        }

        public MatchButtonData GetData()
        {
            MatchButtonData matchButtonData = new MatchButtonData();
            matchButtonData._rotation = (int)transform.rotation.eulerAngles.z;
            matchButtonData._column = _column;
            matchButtonData._row = _row;
            matchButtonData._initialStatus = _currentButtonStatus.ToString();
            matchButtonData._finalStatus = _currentButtonSolutionStatus.ToString();

            return matchButtonData;
        }
    }
}
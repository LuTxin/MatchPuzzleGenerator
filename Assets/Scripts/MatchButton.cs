using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public enum  MatchStatus
    {
        Locked,
        Placed,
        Removed,
        Invisible
    }

    public class MatchButton: MonoBehaviour
    {
        [SerializeField] private Button _matchButton;
    
        [SerializeField] private Transform _stroke;
        [SerializeField] private Transform _concrete;
        [SerializeField] private Transform _invisible;
        [SerializeField] private Transform _indicator;
        
        
        [JsonProperty] [HideInInspector]
        public MatchStatus currentMatchStatus;
        
        public MatchStatus CurrentMatchStatus
        {
            get => currentMatchStatus;
            set
            {
                currentMatchStatus = value;
                switch (value)
                {
                    case MatchStatus.Invisible:
                        _invisible.gameObject.SetActive(true);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(false);
                        break;
                    case MatchStatus.Locked:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(true);
                        _indicator.gameObject.SetActive(false);
                        break;
                    case MatchStatus.Placed:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(true);
                        break;
                    case MatchStatus.Removed:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(true);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(false);
                        break;
                }
            }
        }

        [HideInInspector] public MatchStatus currentMatchSolutionStatus;

        public MatchStatus CurrentMatchSolutionStatus
        {
            get => currentMatchSolutionStatus;
            set
            {
                currentMatchSolutionStatus = value;
                switch (value)
                {
                    case MatchStatus.Invisible:
                        _invisible.gameObject.SetActive(true);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(false);
                        break;
                    case MatchStatus.Locked:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(true);
                        _indicator.gameObject.SetActive(false);
                        break;
                    case MatchStatus.Placed:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(true);
                        break;
                    case MatchStatus.Removed:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(true);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(false);
                        break;
                }
            }
        }

        [HideInInspector] public int _column;
        [HideInInspector]public int _row;

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
            CurrentMatchStatus = MatchStatus.Locked;
            CurrentMatchSolutionStatus = MatchStatus.Locked;
            SetInteractiveMethod(false);
        }

        public void SetInteractiveMethod(bool isAnswerMode)
        {
            _matchButton.onClick.RemoveAllListeners();
            if (!isAnswerMode)
            {
                _matchButton.onClick.AddListener(OnButtonPressed);
                switch (currentMatchStatus)
                {
                    case MatchStatus.Invisible:
                        _invisible.gameObject.SetActive(true);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(false);
                        break;
                    case MatchStatus.Locked:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(true);
                        _indicator.gameObject.SetActive(false);
                        break;
                    case MatchStatus.Placed:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(true);
                        break;
                    case MatchStatus.Removed:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(true);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(false);
                        break;
                }
            }
            else
            {
                _matchButton.onClick.AddListener(OnSolutionButtonPressed);
                switch (currentMatchSolutionStatus)
                {
                    case MatchStatus.Invisible:
                        _invisible.gameObject.SetActive(true);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(false);
                        break;
                    case MatchStatus.Locked:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(true);
                        _indicator.gameObject.SetActive(false);
                        break;
                    case MatchStatus.Placed:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(false);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(true);
                        break;
                    case MatchStatus.Removed:
                        _invisible.gameObject.SetActive(false);
                        _stroke.gameObject.SetActive(true);
                        _concrete.gameObject.SetActive(false);
                        _indicator.gameObject.SetActive(false);
                        break;
                }
            }
        }
        
        private void OnButtonPressed()
        {
            int currentButtonStatusInt = (int) CurrentMatchStatus;
            currentButtonStatusInt++;
            currentButtonStatusInt %= 4;
            CurrentMatchStatus = (MatchStatus)currentButtonStatusInt;

            switch (CurrentMatchStatus)
            {
                case MatchStatus.Invisible:
                    currentMatchSolutionStatus = MatchStatus.Invisible;
                    break;
                case MatchStatus.Locked:
                    currentMatchSolutionStatus = MatchStatus.Locked;
                    break;
                default:
                    if (currentMatchSolutionStatus == MatchStatus.Invisible
                        || currentMatchSolutionStatus == MatchStatus.Locked)
                    {
                        currentMatchSolutionStatus = MatchStatus.Removed;
                    }
                    break;
            }
            
        }
        
        private void OnSolutionButtonPressed()
        {
            if (CurrentMatchStatus == MatchStatus.Invisible 
                || CurrentMatchStatus == MatchStatus.Locked)
            {
                return;
            }

            if (CurrentMatchSolutionStatus == MatchStatus.Placed)
            {
                CurrentMatchSolutionStatus = MatchStatus.Removed;
            }
            else
            {
                CurrentMatchSolutionStatus = MatchStatus.Placed;
            }
        }

        public MatchButtonData GetData()
        {
            MatchButtonData matchButtonData = new MatchButtonData();
            matchButtonData.Rotation = (int)transform.rotation.eulerAngles.z;
            matchButtonData.Column = _column;
            matchButtonData.Row = _row;
            matchButtonData.InitialStatus = currentMatchStatus.ToString();
            matchButtonData.FinalStatus = currentMatchSolutionStatus.ToString();

            return matchButtonData;
        }
    }
}
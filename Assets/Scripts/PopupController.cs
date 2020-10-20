using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public enum InputFieldType
    {
        None,
        Single,
        Duo
    }
    
    public class PopupController: MonoBehaviour
    {
        [SerializeField] private InputField _inputField;
        [SerializeField] private InputField _inputField1; //left short input field
        [SerializeField] private InputField _inputField2; // right short input field
        [SerializeField] private Text _informationLabel;
        [SerializeField] private Button _okayButton;
        [SerializeField] private Button _cancelButton;

        public event EventHandler OnOkayButtonPressedEventHandler;
        public event EventHandler OnCancelButtonPressedEventHandler;

        public void Start()
        {
            _okayButton.onClick.AddListener(OnOkayButtonClicked);
            _cancelButton.onClick.AddListener(OnCancelButtonClicked);
        }

        public void ShowPopup(bool show, InputFieldType type, string text, EventHandler onOkayClicked = null,  EventHandler onCancelClicked = null)
        {
            _informationLabel.text = text;

            _inputField.gameObject.SetActive(type == InputFieldType.Single);
            _inputField1.gameObject.SetActive(type == InputFieldType.Duo);
            _inputField2.gameObject.SetActive(type == InputFieldType.Duo);

            if (onOkayClicked != null)
            {
                OnOkayButtonPressedEventHandler += onOkayClicked;
            }

            if (onCancelClicked != null)
            {
                OnCancelButtonPressedEventHandler += onCancelClicked;
            }
            
            gameObject.SetActive(show);
        }

        public void OnOkayButtonClicked()
        {
            gameObject.SetActive(false);
            if (OnOkayButtonPressedEventHandler != null)
            {
                OnOkayButtonPressedEventHandler.Invoke(this, new EventArgs());
            }
            CleanDelegate();
        }
        
        public void OnCancelButtonClicked()
        {
            gameObject.SetActive(false);
            if (OnCancelButtonPressedEventHandler != null)
            {
                OnCancelButtonPressedEventHandler.Invoke(this, new EventArgs());
            }
            CleanDelegate();
        }

        public string GetInputFieldText()
        {
            return _inputField.textComponent.text;
        }
        
        public string GetLeftInputFieldText()
        {
            return _inputField1.textComponent.text;
        }
        
        public string GetRightInputFieldText()
        {
            return _inputField2.textComponent.text;
        }

        private void CleanDelegate()
        {
            if (OnOkayButtonPressedEventHandler != null)
            {
                foreach (Delegate d in OnOkayButtonPressedEventHandler.GetInvocationList())
                {
                    OnOkayButtonPressedEventHandler -= (EventHandler) d;
                }
            }

            if (OnCancelButtonPressedEventHandler != null)
            {
                foreach (Delegate d in OnCancelButtonPressedEventHandler.GetInvocationList())
                {
                    OnCancelButtonPressedEventHandler -= (EventHandler) d;
                }
            }
        }
    }
}
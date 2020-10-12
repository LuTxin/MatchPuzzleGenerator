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

        public void ShowPopup(bool show, InputFieldType type, string text, EventHandler onOkayClicked)
        {
            _informationLabel.text = text;

            _inputField.gameObject.SetActive(type == InputFieldType.Single);
            _inputField1.gameObject.SetActive(type == InputFieldType.Duo);
            _inputField2.gameObject.SetActive(type == InputFieldType.Duo);

            OnOkayButtonPressedEventHandler += onOkayClicked;
            
            gameObject.SetActive(show);
        }

        public void OnOkayButtonClicked()
        {
            gameObject.SetActive(false);
            
            if (OnOkayButtonPressedEventHandler != null)
            {
                OnOkayButtonPressedEventHandler.Invoke(this, new EventArgs());
            }
        }
        
        public void OnCancelButtonClicked()
        {
            gameObject.SetActive(false);
            
            if (OnCancelButtonPressedEventHandler != null)
            {
                OnCancelButtonPressedEventHandler.Invoke(this, new EventArgs());
            }
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
    }
}
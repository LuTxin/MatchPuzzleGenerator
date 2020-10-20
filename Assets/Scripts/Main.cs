using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DefaultNamespace;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;

public class Main : MonoBehaviour
{
    //Control panel
    [SerializeField] private PopupController _popupController;
    [SerializeField] private InputField _rowInputField;
    [SerializeField] private InputField _columnInputField;
    [SerializeField] private InputField _matchNumInputField;
    [SerializeField] private InputField _handCapabilityInputField;
    [SerializeField] private Dropdown _typeDropDown;
    [SerializeField] private Button _generateFrameButton;
    [SerializeField] private Button _toggleAnswerButton;
    [SerializeField] private RectTransform _matchPanel;
    [SerializeField] private Button _exportJsonButton;
    [SerializeField] private Text _statusLabel;
    [SerializeField] private Button _loadButton;
    
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _hideInvisibleMatchButton;
    [SerializeField] private Text _hideInvisibleMatchButtonText;
    [SerializeField] private Button _resetInventoryButton;

    //main
    [SerializeField] private GameObject _initializationPanel;
    [SerializeField] private GameObject _processPanel;
    [SerializeField] private GameObject _matchObject;
    [SerializeField] private GameObject _inventoryMatchObject;
    
    //Inventory
    [SerializeField] private Transform inventoryPanel;
    
    
    // Start is called before the first frame update

    private string _gameType;
    private int _row;
    private int _column;
    private int _matchNum;
    private int _handCapability;
    private IFrameGenerater _frameGenerater;
    private bool _showInvisibleMatch = false;
    
    private List<MatchButton> _inventoryMatches;

    private Color InvisibleMatchColor
    {
        get { return _showInvisibleMatch ? Color.black : Color.clear; }
    }
    
    private string ShowInvisibleMatchButtonText
    {
        get { return _showInvisibleMatch ? "Hide Invisible Match" : "Show Invisible Match"; }
    }

    private void Start()
    {
        List<string> options = new List<string>();
        options.Add(MatchGeneraterConstants.Square);
        options.Add(MatchGeneraterConstants.Hourglass);
        options.Add(MatchGeneraterConstants.Barrel);
        _typeDropDown.AddOptions(options);
        _generateFrameButton.onClick.AddListener(OnGenerateFrameButtonClicked);
        _toggleAnswerButton.onClick.AddListener(ToggleAnswer);
        _exportJsonButton.onClick.AddListener(OnExportButtonClicked);
        _restartButton.onClick.AddListener(Restart);
        _hideInvisibleMatchButton.onClick.AddListener(ToggleInvisibleMatch);
        _resetInventoryButton.onClick.AddListener(OnResetInventoryButtonClicked);

        _inventoryMatches = new List<MatchButton>();
    }

    private void OnDestroy()
    {
        _generateFrameButton.onClick.RemoveListener(OnGenerateFrameButtonClicked);
        _toggleAnswerButton.onClick.RemoveListener(ToggleAnswer);
        _exportJsonButton.onClick.RemoveListener(OnExportButtonClicked);
        _restartButton.onClick.RemoveListener(Restart);
        _hideInvisibleMatchButton.onClick.RemoveListener(ToggleInvisibleMatch);
        _resetInventoryButton.onClick.RemoveListener(OnResetInventoryButtonClicked);
    }

    private void OnGenerateFrameButtonClicked()
    {
        StringBuilder warningBuilder = new StringBuilder();

        warningBuilder.Append(ParseRow(_rowInputField.text));
        warningBuilder.Append(ParseColumn(_columnInputField.text));
        warningBuilder.Append(ParseMatchNum(_matchNumInputField.text));
        warningBuilder.Append(ParseHandCapability(_handCapabilityInputField.text));
        
        if (warningBuilder.Length > 0)
        {
            _popupController.ShowPopup(true, InputFieldType.None, warningBuilder.ToString(), null);
        }
        
        _gameType = _typeDropDown.options[_typeDropDown.value].text;
        
        GenerateFrame();
        SetupInventory();
    }

    private string ParseRow(string input)
    {
        StringBuilder warningBuilder = new StringBuilder();
        
        if (!int.TryParse(input, out _row))
        {
            _row = MatchGeneraterConstants.MinRowNumber;
            warningBuilder.AppendLine("Row number invalid. Using " + _row + " instead.");
        }
        else if (_row < MatchGeneraterConstants.MinRowNumber)
        {
            _row = MatchGeneraterConstants.MinRowNumber;
            warningBuilder.AppendLine("Row cannot < " + MatchGeneraterConstants.MinRowNumber + ". Using " + _row + " instead");
        }
        else if (_row > MatchGeneraterConstants.MaxRowNumber)
        {
            _row = MatchGeneraterConstants.MaxRowNumber;
            warningBuilder.AppendLine("Row cannot >  "+ MatchGeneraterConstants.MaxRowNumber + ". Using " + _row + " instead");
        }

        return warningBuilder.ToString();
    }
    
    private string ParseColumn(string input)
    {
        StringBuilder warningBuilder = new StringBuilder();
        
        if (!int.TryParse(input, out _column))
        {
            _column = MatchGeneraterConstants.MinColumnNumber;
            warningBuilder.AppendLine("Column number invalid. Using " + _column + " instead.");
        }
        else if (_column < MatchGeneraterConstants.MinColumnNumber)
        {
            _column = MatchGeneraterConstants.MinColumnNumber;
            warningBuilder.AppendLine("Column cannot < " + MatchGeneraterConstants.MinColumnNumber + ". Using " + _column + " instead");
        }
        else if (_column > MatchGeneraterConstants.MaxColumnNumber)
        {
            _column = MatchGeneraterConstants.MaxColumnNumber;
            warningBuilder.AppendLine("Column cannot >  "+ MatchGeneraterConstants.MaxColumnNumber + ". Using " + _column + " instead");
        }

        return warningBuilder.ToString();
    }
    
    private string ParseMatchNum(string input)
    {
        StringBuilder warningBuilder = new StringBuilder();
        
        if (!int.TryParse(input, out _matchNum))
        {
            _matchNum = MatchGeneraterConstants.MinMatchNumNumber;
            warningBuilder.AppendLine("MatchNum number invalid. Using " + _matchNum + " instead.");
        }
        else if (_matchNum < MatchGeneraterConstants.MinMatchNumNumber)
        {
            _matchNum = MatchGeneraterConstants.MinMatchNumNumber;
            warningBuilder.AppendLine("MatchNum cannot < " + MatchGeneraterConstants.MinMatchNumNumber + ". Using " + _matchNum + " instead");
        }
        else if (_matchNum > MatchGeneraterConstants.MaxMatchNumNumber)
        {
            _matchNum = MatchGeneraterConstants.MaxMatchNumNumber;
            warningBuilder.AppendLine("MatchNum cannot >  "+ MatchGeneraterConstants.MaxMatchNumNumber + ". Using " + _matchNum + " instead");
        }

        return warningBuilder.ToString();
    }

    private string ParseHandCapability(string input)
    {
        StringBuilder warningBuilder = new StringBuilder();
        
        if (!int.TryParse(input, out _handCapability))
        {
            _handCapability =_matchNum;
            warningBuilder.AppendLine("HandCapability number invalid. Using " + _handCapability + " instead.");
        }
        else if (_handCapability < _matchNum)
        {
            _handCapability = _matchNum;
            warningBuilder.AppendLine("HandCapability cannot < initial match number. Using " + _handCapability + " instead");
        }
        else if (_handCapability > MatchGeneraterConstants.MaxHandCapabilityNumber)
        {
            _handCapability = MatchGeneraterConstants.MaxHandCapabilityNumber;
            warningBuilder.AppendLine("HandCapability cannot >  "+ MatchGeneraterConstants.MaxHandCapabilityNumber + ". Using " + _handCapability + " instead");
        }
        
        return warningBuilder.ToString();
    }

    private void GenerateFrame()
    {
        if (_frameGenerater != null)
        {
            _frameGenerater.CleanFrame();
        }

        _statusLabel.text = "Edit Initial Status";
        
        switch (_gameType)
        {
            case MatchGeneraterConstants.Square:
                _frameGenerater = new SquareFrameGenerater();
                _frameGenerater.GenerateFrame(_row, _column, _matchPanel, _matchObject, MatchGeneraterConstants.Square, InvisibleMatchColor);
                break;
            case MatchGeneraterConstants.Hourglass:
                _frameGenerater = new TriangleHourGlassFrameGenerater();
                _frameGenerater.GenerateFrame(_row, _column, _matchPanel, _matchObject, MatchGeneraterConstants.Hourglass, InvisibleMatchColor);
                break;
            case MatchGeneraterConstants.Barrel:
                _frameGenerater = new TriangleHourGlassFrameGenerater();
                _frameGenerater.GenerateFrame(_row, _column, _matchPanel, _matchObject, MatchGeneraterConstants.Barrel, InvisibleMatchColor);
                break;
        }

        ChangePanel(ControlPanelType.processing);
    }

    private void SetupInventory()
    {
        for (int i = 0; i < _handCapability; i++)
        {
            GameObject newMatch = GameObject.Instantiate(_inventoryMatchObject, Vector3.zero, Quaternion.identity, inventoryPanel);
            newMatch.transform.Rotate(Vector3.forward, 90);
            newMatch.GetComponent<Button>().enabled = false;
            newMatch.SetActive(true);
            
            _inventoryMatches.Add(newMatch.GetComponent<MatchButton>());
        }

        StartCoroutine(SetMatchStatus());
    }

    private void CleanInventory()
    {
        for (int i = 0; i < _inventoryMatches.Count; i++)
        {
            Destroy(_inventoryMatches[i].gameObject);
        }
        _inventoryMatches.Clear();
    }

    private IEnumerator SetMatchStatus()
    {
        yield return null;
        
        for (int i = 0; i < _matchNum; i++)
        {
            _inventoryMatches[i].CurrentMatchStatus = MatchStatus.Placed;
        }

        for (int i = _matchNum; i < _handCapability; i++)
        {
            _inventoryMatches[i].CurrentMatchStatus = MatchStatus.ForceHighlight;
        }
        
        yield return null;
    }
    

    private void ToggleAnswer()
    {
        if (_frameGenerater != null)
        {
            _frameGenerater.ToggleSetAnswerMode();
            if (_frameGenerater.IsAnswerMode())
            {
                _statusLabel.text = "Edit Final Status";
            }
            else
            {
                _statusLabel.text = "Edit Initial Status";
            }
        }
    }

    private void ExportJson(string fileName)
    {
        if (_frameGenerater != null)
        {
            MatchQuizData quizData = new MatchQuizData();
            quizData._matchesInfo = _frameGenerater.GetMatchButtonList();
            quizData._column = _column;
            quizData._row = _row;
            quizData._type = _gameType;
            quizData._matchNumber = _matchNum;
            quizData._handCapability = _handCapability;

            StringWriter stringWriter = new StringWriter();
            JsonWriter jsonWriter = new JsonTextWriter(stringWriter);
            JsonSerializer jsonSerializer = JsonSerializer.Create();
            jsonSerializer.Serialize(jsonWriter, quizData);

            string path = "./QuizData/" + fileName + ".json";
            StreamWriter streamWriter = new StreamWriter(path);
            streamWriter.Write(stringWriter.ToString());
            streamWriter.Flush();

            _statusLabel.text = "Json generated";

            streamWriter.Close();
            
            path = "./QuizData/" + fileName + ".png";
            ScreenCapture.CaptureScreenshot(path);

            try
            {
#if UNITY_STANDALONE_WIN
                System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,{0}", path.Replace('/', '\\')));
#elif UNITY_STANDALONE_OSX
                System.Diagnostics.Process.Start("open", string.Format("-R \"{0}\"", path));
#endif
            }
            catch
            {
                Debug.LogWarning("failed to open file location");
            }
        }
    }

    private void LoadJson(string fileName)
    {
        
    }
    
    public static float GetRestrictionFactor(float rectWidth, float rectHeight, float restrictionWidth, float restrictionHeight)
    {
        float scalingFactor = 0f;
            
        if (restrictionWidth < rectWidth && restrictionHeight > rectHeight)
        {
            scalingFactor = restrictionWidth / rectWidth;
        }
        else if (restrictionWidth > rectWidth && restrictionHeight < rectHeight)
        {
            scalingFactor = restrictionHeight / rectHeight;
        }
        else
        {
            float scaleFactorWidth  = restrictionWidth / rectWidth;
            float scaleFactorHeight = restrictionHeight / rectHeight;
            scalingFactor = Mathf.Min(scaleFactorWidth, scaleFactorHeight);
        }

        return scalingFactor;
    }
    
    private void ChangePanel(ControlPanelType type)
    {
        _initializationPanel.SetActive(type == ControlPanelType.Initialization);
        _processPanel.SetActive(type == ControlPanelType.processing);
        _hideInvisibleMatchButtonText.text = ShowInvisibleMatchButtonText;
    }

    private void Restart()
    {
        _frameGenerater.CleanFrame();
        CleanInventory();
        ChangePanel(ControlPanelType.Initialization);
    }

    private void ToggleInvisibleMatch()
    {
        _showInvisibleMatch = !_showInvisibleMatch;
        _frameGenerater.SetInvisibleMatchColor(InvisibleMatchColor);
        _hideInvisibleMatchButtonText.text = ShowInvisibleMatchButtonText;
    }
    
    private void OnResetInventoryButtonClicked()
    {
        _popupController.ShowPopup(true, InputFieldType.Duo, MatchGeneraterConstants.InputInventoryNumberString, OnResetInventoryFinished);
    }

    private void OnResetInventoryFinished(object sender, EventArgs arg)
    {
        StringBuilder warningBuilder = new StringBuilder();
        
        warningBuilder.Append(ParseMatchNum(_popupController.GetLeftInputFieldText()));
        warningBuilder.Append(ParseHandCapability(_popupController.GetRightInputFieldText()));
        
        if (warningBuilder.Length > 0)
        {
            _popupController.ShowPopup(true, InputFieldType.None, warningBuilder.ToString(), null);
        }

        CleanInventory();
        SetupInventory();
    }

    private void OnExportButtonClicked()
    {
        _popupController.ShowPopup(true, InputFieldType.Single, MatchGeneraterConstants.InputSaveNameString, OnExportPathConfirmed);
    }

    private void OnExportPathConfirmed(object sender, EventArgs arg)
    {

        if (!string.IsNullOrEmpty(_popupController.GetInputFieldText()))
        {
            ExportJson(_popupController.GetInputFieldText());
        }
        else
        {
            _popupController.ShowPopup(true, InputFieldType.None, MatchGeneraterConstants.FileNameError, null);
        }
    }

    private void OnLoadButtonClicked()
    {
        _popupController.ShowPopup(true, InputFieldType.Single, MatchGeneraterConstants.InputLoadNameString, OnLoadPathConfirmed);
    }

    private void OnLoadPathConfirmed(object sender, EventArgs arg)
    {
        if (!string.IsNullOrEmpty(_popupController.GetInputFieldText()))
        {
            LoadJson(_popupController.GetInputFieldText());
        }
    }
}

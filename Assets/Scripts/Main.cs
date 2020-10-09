using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;


public class Main : MonoBehaviour
{
    //Control panel
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

    private List<MatchButton> _inventoryMatches;
    
    private void Start()
    {
        List<string> options = new List<string>();
        options.Add(MatchGeneraterConstants.Square);
        options.Add(MatchGeneraterConstants.Hourglass);
        options.Add(MatchGeneraterConstants.Barrel);
        _typeDropDown.AddOptions(options);
        _generateFrameButton.onClick.AddListener(OnGenerateFrameButtonClicked);
        _toggleAnswerButton.onClick.AddListener(ToggleAnswer);
        _exportJsonButton.onClick.AddListener(ExportJson);
        
        _inventoryMatches = new List<MatchButton>();
    }

    private void OnDestroy()
    {
        _generateFrameButton.onClick.RemoveListener(OnGenerateFrameButtonClicked);
        _toggleAnswerButton.onClick.RemoveListener(ToggleAnswer);
        _exportJsonButton.onClick.RemoveListener(ExportJson);
    }

    private void OnGenerateFrameButtonClicked()
    {
        _row = 1;
        if (!int.TryParse(_rowInputField.text, out _row))
        {
            Debug.LogErrorFormat("row invalid. Using {0} instead:", _row);
        }
        
        _column = 1;
        if (!int.TryParse(_columnInputField.text, out _column))
        {
            Debug.LogErrorFormat("column invalid. Using {0} instead", _column);
        }

        _gameType = _typeDropDown.options[_typeDropDown.value].text;

        _matchNum = 0;
        if(!int.TryParse(_matchNumInputField.text, out _matchNum))
        {
            Debug.LogErrorFormat("_matchNum invalid. Using {0} instead", _matchNum);
        }

        _handCapability = 0;
        if(!int.TryParse(_handCapabilityInputField.text, out _handCapability))
        {
            Debug.LogErrorFormat("_handCapability invalid. Using {0} instead", _handCapability);
        }
        
        GenerateFrame();
        SetupInventory();
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
                _frameGenerater.GenerateFrame(_row, _column, _matchPanel, _matchObject, MatchGeneraterConstants.Square);
                break;
            case MatchGeneraterConstants.Hourglass:
                _frameGenerater = new TriangleHourGlassFrameGenerater();
                _frameGenerater.GenerateFrame(_row, _column, _matchPanel, _matchObject, MatchGeneraterConstants.Hourglass);
                break;
            case MatchGeneraterConstants.Barrel:
                _frameGenerater = new TriangleHourGlassFrameGenerater();
                _frameGenerater.GenerateFrame(_row, _column, _matchPanel, _matchObject, MatchGeneraterConstants.Barrel);
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

    private IEnumerator SetMatchStatus()
    {
        yield return null;
        
        for (int i = 0; i < _matchNum; i++)
        {
            _inventoryMatches[i].CurrentMatchStatus = MatchStatus.Placed;
        }

        for (int i = _matchNum; i < _handCapability; i++)
        {
            _inventoryMatches[i].CurrentMatchStatus = MatchStatus.Removed;
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

    private void ExportJson()
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

            StreamWriter streamWriter = new StreamWriter("/Users/lu.zhang/MatchPuzzleGenerator/QuizData/Generated");
            streamWriter.Write(stringWriter.ToString());
            streamWriter.Flush();

            _statusLabel.text = "Json generated";

            streamWriter.Close();
        }
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
    }
}

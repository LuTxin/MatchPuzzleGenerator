using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private InputField _rowInputField;
    [SerializeField] private InputField _columnInputField;
    [SerializeField] private InputField _matchNumInputField;
    [SerializeField] private Dropdown _typeDropDown;
    [SerializeField] private Button _generateFrameButton;
    [SerializeField] private Button _toggleAnswerButton;
    [SerializeField] private RectTransform _matchPanel;
    [SerializeField] private Button _exportJsonButton;
    
    [SerializeField] private GameObject _matchObject;
    // Start is called before the first frame update

    private const string _squareType = "Square";
    private const string _triangleHourglassType = "Triangle_hourglass";
    private const string _triangleBarrelType = "Triangle_barrel";
    
    private string _gameType;
    private int _row;
    private int _column;
    private int _matchNum;
    private IFrameGenerater _frameGenerater;

    private void Start()
    {
        _generateFrameButton.onClick.AddListener(OnGenerateFrameButtonClicked);
        _toggleAnswerButton.onClick.AddListener(ToggleAnswer);
        _exportJsonButton.onClick.AddListener(ExportJson);
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

        _matchNum = 1;
        if(!int.TryParse(_matchNumInputField.text, out _matchNum))
        {
            Debug.LogErrorFormat("column invalid. Using {0} instead", _matchNum);
        }

        GenerateFrame();
    }

    private void GenerateFrame()
    {
        if (_frameGenerater != null)
        {
            _frameGenerater.CleanFrame();
        }

        switch (_gameType)
        {
            case _squareType:
                _frameGenerater = new SquareFrameGenerater();
                _frameGenerater.GenerateFrame(_row, _column, _matchPanel, _matchObject);
                break;
            case _triangleHourglassType:
                break;
            case _triangleBarrelType:
                break;
        }
    }

    private void ToggleAnswer()
    {
        if (_frameGenerater != null)
        {
            _frameGenerater.ToggleSetAnswerMode();
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
            
            
            StringWriter stringWriter = new StringWriter();
            JsonWriter jsonWriter = new JsonTextWriter(stringWriter);
            JsonSerializer jsonSerializer = JsonSerializer.Create();
            jsonSerializer.Serialize(jsonWriter, quizData);

            StreamWriter streamWriter = new StreamWriter("/Users/lu.zhang/MatchGenerater/QuizData/untitled");
            streamWriter.Write(stringWriter.ToString());
            streamWriter.Flush();
            Debug.LogError("luzhang2 :" + streamWriter.ToString());
        }
    }
}

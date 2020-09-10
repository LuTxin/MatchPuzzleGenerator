using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class TriangleHourGlassFrameGenerater: IFrameGenerater
    {
        private List<MatchButton> _horizontalMatchButtons;
        private List<MatchButton> _slashMatchButtons;
        private List<MatchButton> _backslashMatchButtons;
        
        private RectTransform _panel;
        private bool _isAnswerMode;

        private int _row;
        private int _column;
        
        public void GenerateFrame(int row, int column, RectTransform panel, GameObject match)
        {
            _panel = panel;
            _row = row;
            _column = column;

            RectTransform matchRect = match.GetComponent<RectTransform>();
            float matchHeight = matchRect.sizeDelta.y;
            float matchWidth = matchRect.sizeDelta.x;
            
            int currentColumn = 0;
            float xOffset = 0;
            float yOffset = 0;

            float globalOffsetDeltaX = (matchHeight + matchWidth) * (int)Math.Ceiling((float)column/2f) + matchWidth;
            float globalOffsetDeltaY = row * (matchWidth + matchHeight * (float)Math.Cos(Math.PI / 6f)) + matchWidth;
            
            MatchButton matchButton = null;
            //draw horizontal matches
            _horizontalMatchButtons = new List<MatchButton>();
            int horizontalMatchRows = 1 + row;
            for (int i = 0; i < horizontalMatchRows; i++)
            {
                if (i % 2 == 0)
                {
                    currentColumn = (int)Math.Ceiling((float)column/2f);
                    xOffset = 0;
                    
                }
                else
                {
                    currentColumn = column/2;
                    xOffset = matchHeight / 2f;
                }
                
                for (int j = 0; j < currentColumn; j++)
                {
                    GameObject newMatch = GameObject.Instantiate(match, Vector3.zero, Quaternion.identity, panel);
                    RectTransform newMatchRect = newMatch.GetComponent<RectTransform>();
                    matchButton = newMatch.GetComponent<MatchButton>();

                    //draw horizontal
                    newMatchRect.Rotate(0, 0, -90);
                    float horizontalX = (matchHeight + matchWidth) * j + matchWidth + matchHeight / 2f;
                    float horizontalY = i * (matchHeight * (float)Math.Cos(Math.PI / 6f) + matchWidth) + matchWidth/2f;
                    newMatchRect.anchoredPosition = new Vector2(horizontalX + xOffset - globalOffsetDeltaX/2f, -horizontalY + globalOffsetDeltaY/2f);
                    newMatch.SetActive(true);
                    _horizontalMatchButtons.Add(matchButton);
                }
            }
            
            //draw backslash matches
            currentColumn = 0;
            _backslashMatchButtons = new List<MatchButton>();
            for (int i = 0; i < row; i++)
            {
                if (i % 2 == 0)
                {
                    xOffset = 0;
                    currentColumn = 1 + column/2;
                }
                else
                {
                    xOffset = matchHeight/2f;
                    currentColumn = 1 + (column - 1)/2;
                }
                
                for (int j = 0; j < currentColumn; j++)
                {
                    GameObject newMatch = GameObject.Instantiate(match, Vector3.zero, Quaternion.identity, panel);
                    RectTransform newMatchRect = newMatch.GetComponent<RectTransform>();
                    matchButton = newMatch.GetComponent<MatchButton>();
                    
                    //draw horizontal
                    newMatchRect.Rotate(0, 0, 30);
                    float horizontalX = (matchHeight + matchWidth) * j + matchWidth + matchHeight/4f;
                    float horizontalY = i * (matchWidth + matchHeight * (float)Math.Cos(Math.PI / 6f)) + matchWidth + matchHeight * (float)Math.Cos(Math.PI / 6f)/2f;
                    newMatchRect.anchoredPosition = new Vector2(horizontalX + xOffset - globalOffsetDeltaX/2f, -horizontalY + globalOffsetDeltaY/2f);
                    newMatch.SetActive(true);
                    _backslashMatchButtons.Add(matchButton);
                }
            }
            
            //draw slash matches
            currentColumn = 0;
            _slashMatchButtons = new List<MatchButton>();
            for (int i = 0; i < row; i++)
            {
                if (i % 2 == 0)
                {
                    xOffset = matchHeight/2f;
                    currentColumn = 1 + (column - 1)/2;
                }
                else
                {
                    xOffset = 0;
                    currentColumn = 1 + column/2;
                }
                
                for (int j = 0; j < currentColumn; j++)
                {
                    GameObject newMatch = GameObject.Instantiate(match, Vector3.zero, Quaternion.identity, panel);
                    RectTransform newMatchRect = newMatch.GetComponent<RectTransform>();
                    matchButton = newMatch.GetComponent<MatchButton>();
                    
                    //draw horizontal
                    newMatchRect.Rotate(0, 0, -30);
                    float horizontalX = (matchHeight + matchWidth) * j + matchWidth + matchHeight/4f;
                    float horizontalY = i * (matchWidth + matchHeight * (float)Math.Cos(Math.PI / 6f)) + matchWidth + matchHeight * (float)Math.Cos(Math.PI / 6f)/2f;
                    newMatchRect.anchoredPosition = new Vector2(horizontalX + xOffset - globalOffsetDeltaX/2f, -horizontalY + globalOffsetDeltaY/2f);
                    newMatch.SetActive(true);
                    _slashMatchButtons.Add(matchButton);
                }
            }
        }
        

        public void CleanFrame()
        {
            for (int i = 0; i < _horizontalMatchButtons.Count; i++)
            {
                GameObject.Destroy(_horizontalMatchButtons[i].gameObject);
            }
            _horizontalMatchButtons.Clear();
            
            for (int i = 0; i < _backslashMatchButtons.Count; i++)
            {
                GameObject.Destroy(_backslashMatchButtons[i].gameObject);
            }
            _backslashMatchButtons.Clear();
            
            for (int i = 0; i < _slashMatchButtons.Count; i++)
            {
                GameObject.Destroy(_slashMatchButtons[i].gameObject);
            }
            _slashMatchButtons.Clear();
            
            
            _panel.GetComponent<Image>().color = Color.white;
        }

        public void ToggleSetAnswerMode()
        {
            if (_isAnswerMode)
            {
                _isAnswerMode = false;
                _panel.GetComponent<Image>().color = Color.white;
            }
            else
            {
                _isAnswerMode = true;
                _panel.GetComponent<Image>().color = Color.gray;
            }
            
            for (int i = 0; i < _horizontalMatchButtons.Count; i++)
            {
                _horizontalMatchButtons[i].SetInteractiveMethod(_isAnswerMode);
            }

            for (int i = 0; i < _backslashMatchButtons.Count; i++)
            {
                _backslashMatchButtons[i].SetInteractiveMethod(_isAnswerMode);
            }
            
            for (int i = 0; i < _slashMatchButtons.Count; i++)
            {
                _slashMatchButtons[i].SetInteractiveMethod(_isAnswerMode);
            }
        }

        public List<MatchButtonData> GetMatchButtonList()
        {
            List<MatchButtonData> matchButtonDatas = new List<MatchButtonData>();
            
            for (int i = 0; i < _horizontalMatchButtons.Count; i++)
            {
                matchButtonDatas.Add(_horizontalMatchButtons[i].GetData());
            }
            
            for (int i = 0; i < _backslashMatchButtons.Count; i++)
            {
                matchButtonDatas.Add(_backslashMatchButtons[i].GetData());
            }
            
            for (int i = 0; i < _slashMatchButtons.Count; i++)
            {
                matchButtonDatas.Add(_slashMatchButtons[i].GetData());
            }

            return matchButtonDatas;
        }

        public bool IsAnswerMode()
        {
            return _isAnswerMode;
        }
    }
}
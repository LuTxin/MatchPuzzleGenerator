using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;


namespace DefaultNamespace
{
    public class SquareFrameGenerater : IFrameGenerater
    {
        private List<List<MatchButton>> _horizontalMatchButtons;
        private List<List<MatchButton>> _verticalMatchButtons;
        private RectTransform _panel;
        private bool _isAnswerMode;

        public bool IsAnswerMode()
        {
            return _isAnswerMode;
        }

        public void SetInvisibleMatchColor(Color color)
        {
            foreach (var matchButtons in _horizontalMatchButtons)
            {
                foreach (var matchButton in matchButtons)
                {
                    matchButton.SetInvisibleMatchColor(color);
                }
            }
            
            foreach (var matchButtons in _verticalMatchButtons)
            {
                foreach (var matchButton in matchButtons)
                {
                    matchButton.SetInvisibleMatchColor(color);
                }
            }
        }

        public void GenerateFrame(int row, int column, RectTransform panel, GameObject match, string direction, Color invisibleMatchColor)
        {
            _isAnswerMode = false;
            _panel = panel;
            
            RectTransform matchRectTransform = match.GetComponent<RectTransform>();
            Rect matchRect = matchRectTransform.rect;
            Vector2 targetSize = new Vector2();
            targetSize.x = (column + 1) * matchRect.width + column * matchRect.height;
            targetSize.y = (row + 1) * matchRect.width + row * matchRect.height;
            float scalingFactor = Main.GetRestrictionFactor(targetSize.x + MatchGeneraterConstants.PaddlingX * 2, targetSize.y + MatchGeneraterConstants.PaddlingY * 2, panel.rect.width, panel.rect.height);
            float matchWidth = matchRect.width * scalingFactor;
            float matchHeight = matchRect.height * scalingFactor;

            float offsetX = (matchWidth * (column + 1) + matchHeight * column) / 2f;
            float offsetY = (matchHeight * row + matchWidth * (row + 1)) / 2f;
            
            //allocation the space for horizontal matches
            _horizontalMatchButtons = new List<List<MatchButton>>();
            for (int i = 0; i < row + 1; i++)
            {
                _horizontalMatchButtons.Add(new List<MatchButton>());
                for (int j = 0; j < column; j++)
                {
                    _horizontalMatchButtons[i].Add(null);
                }
            }
            
            //allocation the space for vertical matches
            _verticalMatchButtons = new List<List<MatchButton>>();
            for (int i = 0; i < row; i++)
            {
                _verticalMatchButtons.Add(new List<MatchButton>());
                for (int j = 0; j < column + 1; j++)
                {
                    _verticalMatchButtons[i].Add(null);
                }
            }
            
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    GameObject newMatch = GameObject.Instantiate(match, Vector3.zero, Quaternion.identity, panel);
                    RectTransform newMatchRect = newMatch.GetComponent<RectTransform>();
                    newMatchRect.sizeDelta = new Vector2(matchWidth, matchHeight); ;
                    //draw horizontal
                    newMatchRect.Rotate(0, 0, -90);
                    float horizontalX = (matchHeight + matchWidth) * j + matchWidth + matchHeight / 2f;
                    float horizontalY = (matchHeight + matchWidth) * i + matchWidth / 2f;
                    newMatchRect.anchoredPosition = new Vector2(horizontalX - offsetX, -horizontalY + offsetY);
                    newMatch.SetActive(true);
                    _horizontalMatchButtons[i][j] = newMatch.GetComponent<MatchButton>();
                    _horizontalMatchButtons[i][j].Column = j;
                    _horizontalMatchButtons[i][j].Row = i;
                    
                    //draw vertical
                    newMatch = GameObject.Instantiate(match, Vector3.zero, Quaternion.identity, panel);
                    newMatchRect = newMatch.GetComponent<RectTransform>();
                    newMatchRect.sizeDelta = new Vector2(matchWidth, matchHeight);
                    
                    float verticalX = (matchHeight + matchWidth) * j + matchWidth / 2f;
                    float verticalY = (matchHeight + matchWidth) * i + matchWidth + matchHeight / 2f;
                    newMatchRect.anchoredPosition = new Vector2(verticalX - offsetX, -verticalY + offsetY);
                    newMatch.SetActive(true);
                    _verticalMatchButtons[i][j] = newMatch.GetComponent<MatchButton>();
                    _verticalMatchButtons[i][j].Column = j;
                    _verticalMatchButtons[i][j].Row = i;
                    
                    if (j == column - 1)
                    {
                        //draw vertical
                        newMatch = GameObject.Instantiate(match, Vector3.zero, Quaternion.identity, panel);
                        newMatchRect = newMatch.GetComponent<RectTransform>();
                        newMatchRect.sizeDelta = new Vector2(matchWidth, matchHeight);
                        
                        verticalX = (matchHeight + matchWidth) * (j + 1) + matchWidth / 2f;
                        verticalY = (matchHeight + matchWidth) * i + matchWidth + matchHeight / 2f;
                        newMatchRect.anchoredPosition = new Vector2(verticalX - offsetX, -verticalY + offsetY);
                        newMatch.SetActive(true);
                        _verticalMatchButtons[i][column] = newMatch.GetComponent<MatchButton>();
                        _verticalMatchButtons[i][column].Column = column;
                        _verticalMatchButtons[i][column].Row = i;
                    }
                    
                    if (i == row - 1)
                    {
                        //draw horizontal
                        newMatch = GameObject.Instantiate(match, Vector3.zero, Quaternion.identity, panel);
                        newMatchRect = newMatch.GetComponent<RectTransform>();
                        newMatchRect.sizeDelta = new Vector2(matchWidth, matchHeight);
                        
                        newMatchRect.Rotate(0, 0, -90);
                        horizontalX = (matchHeight + matchWidth) * j + matchWidth + matchHeight / 2f;
                        horizontalY = (matchHeight + matchWidth) * (i + 1) + matchWidth / 2f;
                        newMatchRect.anchoredPosition = new Vector2(horizontalX - offsetX, -horizontalY + offsetY);
                        newMatch.SetActive(true);
                        _horizontalMatchButtons[row][j] = newMatch.GetComponent<MatchButton>();
                        _horizontalMatchButtons[row][j].Column = j;
                        _horizontalMatchButtons[row][j].Row = row;
                    }
                }
            }

            SetInvisibleMatchColor(invisibleMatchColor);
        }

        public void CleanFrame()
        {
            for (int i = 0; i < _horizontalMatchButtons.Count; i++)
            {
                for (int j = 0; j < _horizontalMatchButtons[i].Count; j++)
                {
                    GameObject.Destroy(_horizontalMatchButtons[i][j].gameObject);
                }
                _horizontalMatchButtons[i].Clear();
            }
            _horizontalMatchButtons.Clear();
            
            for (int i = 0; i < _verticalMatchButtons.Count; i++)
            {
                for (int j = 0; j < _verticalMatchButtons[i].Count; j++)
                {
                    GameObject.Destroy(_verticalMatchButtons[i][j].gameObject);
                }
                _verticalMatchButtons[i].Clear();
            }
            _verticalMatchButtons.Clear();
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
                for (int j = 0; j < _horizontalMatchButtons[i].Count; j++)
                {
                    _horizontalMatchButtons[i][j].SetInteractiveMethod(_isAnswerMode);
                }
            }

            for (int i = 0; i < _verticalMatchButtons.Count; i++)
            {
                for (int j = 0; j < _verticalMatchButtons[i].Count; j++)
                {
                    _verticalMatchButtons[i][j].SetInteractiveMethod(_isAnswerMode);
                }
            }
        }

        public List<MatchButtonData> GetMatchButtonList()
        {
            List<MatchButtonData> matchButtonDatas = new List<MatchButtonData>();
            
            for (int i = 0; i < _horizontalMatchButtons.Count; i++)
            {
                for (int j = 0; j < _horizontalMatchButtons[i].Count; j++)
                {
                    matchButtonDatas.Add(_horizontalMatchButtons[i][j].GetData());
                }
            }
            
            for (int i = 0; i < _verticalMatchButtons.Count; i++)
            {
                for (int j = 0; j < _verticalMatchButtons[i].Count; j++)
                {
                    matchButtonDatas.Add(_verticalMatchButtons[i][j].GetData());
                }
            }

            return matchButtonDatas;
        }
    }
}
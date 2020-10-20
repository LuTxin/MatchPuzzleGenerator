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

        private string _direction;
        private int toggleFactor = 1;
        
        public void GenerateFrame(int row, int column, RectTransform panel, GameObject match, string direction, Color invisibleColor)
        {
            if (direction == MatchGeneraterConstants.Hourglass)
            {
                toggleFactor = 0;
            }
            else if (direction == MatchGeneraterConstants.Barrel)
            {
                toggleFactor = 1;
            }
            
            _panel = panel;
            _row = row;
            _column = column;

            //
            //restrict the match size
            RectTransform matchRectTransform = match.GetComponent<RectTransform>();
            Rect matchRect = matchRectTransform.rect;
            Vector2 targetSize = new Vector2();
            targetSize.x = (matchRect.height + matchRect.width) * (int) Math.Ceiling((float) column / 2f) +
                           matchRect.width;
            targetSize.y = row * (matchRect.width + matchRect.height * (float) Math.Cos(Math.PI / 6f)) +
                           matchRect.width;
            float scalingFactor = Main.GetRestrictionFactor(targetSize.x + MatchGeneraterConstants.PaddlingX * 2,
                targetSize.y + MatchGeneraterConstants.PaddlingY * 2, panel.rect.width, panel.rect.height);
            float matchWidth = matchRect.width * scalingFactor;
            float matchHeight = matchRect.height * scalingFactor;
            
            int currentColumn = 0;
            float xOffset = 0;
            float yOffset = 0;

            float globalOffsetDeltaX = (matchHeight + matchWidth) * (int)Math.Ceiling((float)column/2f) + 2.5f * matchWidth;
            float globalOffsetDeltaY = row * (matchWidth + matchHeight * (float)Math.Cos(Math.PI / 6f)) + matchWidth;
            
            MatchButton matchButton = null;
            //draw horizontal matches
            _horizontalMatchButtons = new List<MatchButton>();
            int horizontalMatchRows = 1 + row;
            for (int i = 0; i < horizontalMatchRows; i++)
            {
                if (i % 2 == toggleFactor)
                {
                    currentColumn = (int)Math.Ceiling((float)column/2f);
                    xOffset = 0;
                    
                }
                else
                {
                    currentColumn = column/2;
                    xOffset = matchHeight / 2f + matchWidth;
                }
                
                for (int j = 0; j < currentColumn; j++)
                {
                    GameObject newMatch = GameObject.Instantiate(match, Vector3.zero, Quaternion.identity, panel);
                    RectTransform newMatchRect = newMatch.GetComponent<RectTransform>();
                    matchButton = newMatch.GetComponent<MatchButton>();
                    newMatchRect.sizeDelta = new Vector2(matchWidth, matchHeight);
                    
                    //draw horizontal
                    newMatchRect.Rotate(0, 0, -90);
                    float horizontalX = (matchHeight + 2 * matchWidth) * j + matchHeight / 2f;
                    float horizontalY = i * (matchHeight * (float)Math.Cos(Math.PI / 6f) + matchWidth) + matchWidth/2f;
                    newMatchRect.anchoredPosition = new Vector2(horizontalX + xOffset - globalOffsetDeltaX/2f, -horizontalY + globalOffsetDeltaY/2f);
                    newMatch.SetActive(true);
                    _horizontalMatchButtons.Add(matchButton);
                    matchButton._column = j;
                    matchButton._row = i;
                    
                }
            }
            
            //draw backslash matches
            currentColumn = 0;
            _backslashMatchButtons = new List<MatchButton>();
            for (int i = 0; i < row; i++)
            {
                if (i % 2 == toggleFactor)
                {
                    xOffset = 0 - matchWidth/2f;
                    currentColumn = 1 + column/2;
                }
                else
                {
                    xOffset = matchHeight/2f + matchWidth/2f;
                    currentColumn = 1 + (column - 1)/2;
                }
                
                for (int j = 0; j < currentColumn; j++)
                {
                    GameObject newMatch = GameObject.Instantiate(match, Vector3.zero, Quaternion.identity, panel);
                    RectTransform newMatchRect = newMatch.GetComponent<RectTransform>();
                    matchButton = newMatch.GetComponent<MatchButton>();
                    newMatchRect.sizeDelta = new Vector2(matchWidth, matchHeight);
                    
                    //draw horizontal
                    newMatchRect.Rotate(0, 0, 30);
                    float horizontalX = (matchHeight + 2 * matchWidth) * j + matchHeight/4f;
                    float horizontalY = i * (matchWidth + matchHeight * (float)Math.Cos(Math.PI / 6f)) + matchWidth + matchHeight * (float)Math.Cos(Math.PI / 6f)/2f;
                    newMatchRect.anchoredPosition = new Vector2(horizontalX + xOffset - globalOffsetDeltaX/2f, -horizontalY + globalOffsetDeltaY/2f);
                    newMatch.SetActive(true);
                    _backslashMatchButtons.Add(matchButton);
                    matchButton._column = j;
                    matchButton._row = i;
                }
            }
            
            //draw slash matches
            currentColumn = 0;
            _slashMatchButtons = new List<MatchButton>();
            for (int i = 0; i < row; i++)
            {
                if (i % 2 == toggleFactor)
                {
                    xOffset = matchHeight/2f + matchWidth/2f;
                    currentColumn = 1 + (column - 1)/2;
                }
                else
                {
                    xOffset = 0 - matchWidth/2f;
                    currentColumn = 1 + column/2;
                }
                
                for (int j = 0; j < currentColumn; j++)
                {
                    GameObject newMatch = GameObject.Instantiate(match, Vector3.zero, Quaternion.identity, panel);
                    RectTransform newMatchRect = newMatch.GetComponent<RectTransform>();
                    matchButton = newMatch.GetComponent<MatchButton>();
                    newMatchRect.sizeDelta = new Vector2(matchWidth, matchHeight);
                    
                    //draw horizontal
                    newMatchRect.Rotate(0, 0, -30);
                    float horizontalX = (matchHeight + 2 * matchWidth) * j + matchHeight/4f;
                    float horizontalY = i * (matchWidth + matchHeight * (float)Math.Cos(Math.PI / 6f)) + matchWidth + matchHeight * (float)Math.Cos(Math.PI / 6f)/2f;
                    newMatchRect.anchoredPosition = new Vector2(horizontalX + xOffset - globalOffsetDeltaX/2f, -horizontalY + globalOffsetDeltaY/2f);
                    newMatch.SetActive(true);
                    _slashMatchButtons.Add(matchButton);
                    matchButton._column = j;
                    matchButton._row = i;
                }
            }
            
            SetInvisibleMatchColor(invisibleColor);
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
        }

        public void ToggleSetAnswerMode()
        {
            if (_isAnswerMode)
            {
                _isAnswerMode = false;
            }
            else
            {
                _isAnswerMode = true;
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

        public void SetInvisibleMatchColor(Color color)
        {
            foreach (var matchButton in _slashMatchButtons)
            {
                matchButton.SetInvisibleMatchColor(color);
            }
            
            foreach (var matchButton in _backslashMatchButtons)
            {
                matchButton.SetInvisibleMatchColor(color);
            }
            
            foreach (var matchButton in _horizontalMatchButtons)
            {
                matchButton.SetInvisibleMatchColor(color);
            }
        }

        public int GetInitialRemovedMatchNumber()
        {
            int removedNumber = 0;
            for (int i = 0; i < _horizontalMatchButtons.Count; i++)
            {
                removedNumber += _horizontalMatchButtons[i].currentMatchStatus == MatchStatus.Removed ? 1 : 0;
            }
            
            for (int i = 0; i < _backslashMatchButtons.Count; i++)
            {
                removedNumber += _backslashMatchButtons[i].currentMatchStatus == MatchStatus.Removed ? 1 : 0;
            }
            
            for (int i = 0; i < _slashMatchButtons.Count; i++)
            {
                removedNumber += _slashMatchButtons[i].currentMatchStatus == MatchStatus.Removed ? 1 : 0;
            }
            return removedNumber;
        }
        
        public int GetInitialPlacedMatchNumber()
        {
            int placedNumber = 0;
            for (int i = 0; i < _horizontalMatchButtons.Count; i++)
            {
                placedNumber += _horizontalMatchButtons[i].currentMatchStatus == MatchStatus.Placed ? 1 : 0;
            }
            
            for (int i = 0; i < _backslashMatchButtons.Count; i++)
            {
                placedNumber += _backslashMatchButtons[i].currentMatchStatus == MatchStatus.Placed ? 1 : 0;
            }
            
            for (int i = 0; i < _slashMatchButtons.Count; i++)
            {
                placedNumber += _slashMatchButtons[i].currentMatchStatus == MatchStatus.Placed ? 1 : 0;
            }
            return placedNumber;
        }
        
        public int GetFinalRemovedMatchNumber()
        {
            int removedNumber = 0;
            for (int i = 0; i < _horizontalMatchButtons.Count; i++)
            {
                removedNumber += _horizontalMatchButtons[i].currentMatchSolutionStatus == MatchStatus.Removed ? 1 : 0;
            }
            
            for (int i = 0; i < _backslashMatchButtons.Count; i++)
            {
                removedNumber += _backslashMatchButtons[i].currentMatchSolutionStatus == MatchStatus.Removed ? 1 : 0;
            }
            
            for (int i = 0; i < _slashMatchButtons.Count; i++)
            {
                removedNumber += _slashMatchButtons[i].currentMatchSolutionStatus == MatchStatus.Removed ? 1 : 0;
            }
            return removedNumber;
        }
        
        public int GetFinalPlacedMatchNumber()
        {
            int placedNumber = 0;
            for (int i = 0; i < _horizontalMatchButtons.Count; i++)
            {
                placedNumber += _horizontalMatchButtons[i].currentMatchSolutionStatus == MatchStatus.Placed ? 1 : 0;
            }
            
            for (int i = 0; i < _backslashMatchButtons.Count; i++)
            {
                placedNumber += _backslashMatchButtons[i].currentMatchSolutionStatus == MatchStatus.Placed ? 1 : 0;
            }
            
            for (int i = 0; i < _slashMatchButtons.Count; i++)
            {
                placedNumber += _slashMatchButtons[i].currentMatchSolutionStatus == MatchStatus.Placed ? 1 : 0;
            }
            return placedNumber;
        }
    }
}
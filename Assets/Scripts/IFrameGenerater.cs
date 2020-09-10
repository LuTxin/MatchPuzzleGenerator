using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IFrameGenerater
    {
        void GenerateFrame(int row, int column, RectTransform panel, GameObject match);
        void CleanFrame();
        void ToggleSetAnswerMode();
        List<MatchButtonData> GetMatchButtonList();
        bool IsAnswerMode();
    }
}
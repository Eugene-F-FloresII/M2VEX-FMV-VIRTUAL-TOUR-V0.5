using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public struct AnswerData
    {
        public string Answers;
        public bool CorrectAnswer;
    }
    [CreateAssetMenu(fileName = "TriviaGameData", menuName = "Data/Trivia Answers")]
    public class AnswerScriptableObject : ScriptableObject
    {
        public List<AnswerData> AnswerData;
    }

}

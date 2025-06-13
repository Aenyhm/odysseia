using System.Collections.Generic;
using Sources;
using TMPro;
using UnityEngine;

namespace Unity.Scripts.Views.Leaderboard {
    public class ScoreManagerView : AbstractManagerView<PlayProgression> {
        private ScoreManagerView() : base("Score") { }
        
        protected override int GetId(PlayProgression data) => data.SaveTime.GetHashCode();

        protected override ICollection<PlayProgression> GetElements(GameState gameState) {
            gameState.PlayProgressions.Sort((a, b) => b.Score.CompareTo(a.Score));
            
            return gameState.PlayProgressions;
        }

        protected override void InitChild(GameObject go, PlayProgression data) {
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.SaveTime;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.Score.ToString();
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.Distance.ToString();
        }

        protected override void UpdateChild(GameObject go, PlayProgression data) {
        }
    }
}

using System.Collections.Generic;
using Sources;
using TMPro;
using UnityEngine;

namespace Unity.Scripts.Views.Leaderboard {
    public class ScoreManagerView : AbstractManagerView<PlayProgression> {
        private ScoreManagerView() : base("Score", false) { }
        
        public override void Render(GameState gameState, float dt) {
            var dataById = new Dictionary<int, PlayProgression>(gameState.PlayProgressions.Count);
            
            gameState.PlayProgressions.Sort((a, b) => b.Score.CompareTo(a.Score));
            
            foreach (var data in gameState.PlayProgressions) {
                var id = data.SaveTime.GetHashCode();
                dataById[id] = data;
            }

            Sync(dataById);
        }
        
        protected override void InitChild(GameObject go, PlayProgression data) {
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.SaveTime;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.Score.ToString();
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.Distance.ToString();
        }
    }
}

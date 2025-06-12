using System;
using System.Collections.Generic;
using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    // Ne pas changer l'ordre : correspond à celui d'Unity
    public enum SceneType : byte { Title, Gameplay, Leaderboard }
    public enum PlayMode : byte { Play, Pause, GameOver }

    // Contient toutes les données du jeu mises à jour par le GameController à chaque Fixed Update.
    [Serializable]
    public class GameState {
        public List<PlayProgression> PlayProgressions;
        public PlayState PlayState;
        public PlayerActions PlayerActions;
        public GlobalProgression GlobalProgression;
        public SceneType CurrentSceneType;
    }
    
    [Serializable]
    public struct PlayState {
        public SwapbackArray<Cannonball> Ammos;
        public SwapbackArray<Cannonball> Cannonballs;
        public Boat Boat;
        public Region Region;
        public Wind Wind;
        public Cannon Cannon;
        public PlayProgression PlayProgression;
        public float ScoreMultiplier;
        public int CoinCount;
        public PlayMode Mode;
    }
    
    #region Données persistentes
    
    [Serializable]
    public struct GlobalProgression {
        public int CoinCount;
    }
    
    [Serializable]
    public struct PlayProgression {
        public string SaveTime;
        public int Distance;
        public int Score;
    }
    
    #endregion
}

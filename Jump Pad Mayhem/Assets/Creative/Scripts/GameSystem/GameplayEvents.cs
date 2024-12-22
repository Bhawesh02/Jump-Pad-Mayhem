using System;

namespace KWCreative
{
    public class GameplayEvents
    {
        public static event Action OnGameFail;

        public static void SendOnGameFail()
        {
            OnGameFail?.Invoke();
        }
        
        public static event Action OnGameWon;

        public static void SendOnGameWon()
        {
            OnGameWon?.Invoke();
        } 
        
        public static event Action OnAllProjectilesFired;

        public static void SendOnAllProjectilesFired()
        {
            OnAllProjectilesFired?.Invoke();
        } 
        
        public static event Action OnBallIsLooping;

        public static void SendOnBallIsLooping()
        {
            OnBallIsLooping?.Invoke();
        } 
    }    
}


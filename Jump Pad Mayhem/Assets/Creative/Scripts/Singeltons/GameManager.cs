using Sirenix.OdinInspector;
using UnityEngine;

namespace KWCreative
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField]
        [EnumToggleButtons]
        private CannonFireTypes m_cannonFireTypes;
        [SerializeField] private Transform m_projectileParent;
        
        private int m_numOfBallsLooping;
        
        public CannonFireTypes CannonFireType => m_cannonFireTypes;
        public Transform ProjectileParent => m_projectileParent;
        
        protected override void Init()
        {
            GameplayEvents.OnBallIsLooping += HandelOnBallIsLooping;
            GameplayEvents.OnAllProjectilesFired += SwitchCannonToJumpSurfaceOnAllProjectilesFired;
        }
        
        
        private void HandelOnBallIsLooping()
        {
            if (++m_numOfBallsLooping == CreativeConfig.Instance.CurrentVariationData.cannonData.projectileCount)
            {
                GameplayEvents.SendOnGameWon();
            }
        }
        
        private void SwitchCannonToJumpSurfaceOnAllProjectilesFired()
        {
           //TODO:
        }

    } 
}



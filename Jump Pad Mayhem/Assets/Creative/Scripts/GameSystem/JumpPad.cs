using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KWCreative
{
    [SelectionBase]
    public class JumpPad : JumpSurface
    {
        private const string ANIM_JUMP = "Jump";

        [SerializeField] private Animator m_jumpPadAnimator;
        
        private VariationData m_currentVariationData;
        private bool m_isGameOver;
        
        private void Awake()
        {
            m_currentVariationData = CreativeConfig.Instance.CurrentVariationData;
            GameplayEvents.OnGameFail += HandelOnGameFail;
        }

        private void OnDestroy()
        {
            GameplayEvents.OnGameFail -= HandelOnGameFail;
        }

        private void HandelOnGameFail()
        {
            m_isGameOver = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            Ball collidedBall = other.gameObject.GetComponent<Ball>();
            if (collidedBall)
            {
                HandelBallCollision(collidedBall);
            }
        }
        
        protected override void HandelBallCollision(Ball collidedBall)
        {
            if (m_isGameOver)
            {
                return;
            }
            Vector3 velocityDirection = transform.right;
            collidedBall.SetVelocity(velocityDirection * CreativeConfig.Instance.CurrentVariationData.jumpPadData.jumpPadForceOverride);
            if (m_isGhost)
            {
                return;
            }
            m_jumpPadAnimator.Play(ANIM_JUMP);
        }


        [Button]
        private void SaveRotation()
        {
            if (!m_currentVariationData)
            {
                m_currentVariationData = CreativeConfig.Instance.CurrentVariationData;
            }
            m_currentVariationData.levelElementsSlots[m_baseItemSlot.Coord.x, m_baseItemSlot.Coord.y]
                .elementRotation = transform.localEulerAngles;
            m_currentVariationData.SaveData();
            SendTransformToGhostObject();
        }

        [Button]
        private void SavePosition()
        {
            if (!m_currentVariationData)
            {
                m_currentVariationData = CreativeConfig.Instance.CurrentVariationData;
            }

            Vector3 positionToSave = transform.localPosition;
            m_currentVariationData.levelElementsSlots[m_baseItemSlot.Coord.x, m_baseItemSlot.Coord.y]
                .elementPositionOffset = new Vector3(positionToSave.x, positionToSave.y, positionToSave.z);
            m_currentVariationData.SaveData();
            SendTransformToGhostObject();
        }
        [Button]
        private void SaveTransform()
        {
            SaveRotation();
            SaveRotation();
        }
    }

}
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KWCreative
{
    public class Ball : Projectile
    {
        [Serializable]
        private struct ColorSphereReferenceData
        {
            public GameObject sphereGameObject;
            public Renderer ballRenderer;
            public ParticleSystem glowParticleSystem;
            public ParticleSystem explosionParticleSystem;
        }

        private const float EXPLOSION_DURATION = 1f;
        private const float WRONG_COLLISION_X_FORCE_MULTIPLIER = 0.5f;
        private const float WRONG_COLLISION_Z_FORCE_MULTIPLIER = 1f;
        private const float WRONG_COLLISION_FORCE = 100f;
        
        [SerializeField]
        private List<ColorSphereReferenceData> m_colorSphereReferences = new();
        
        private List<JumpPad> m_collidedJumpPads = new ();
        private ColorSphereReferenceData m_currentColorSphere;
        private bool m_isLooping;

        public Vector3 CurrentVelocity => m_rigidbody.velocity;
        
        private void OnEnable()
        {
            GameplayEvents.OnGameWon += HandelOnGameWon;
            GameplayEvents.OnGameFail += HandelOnGameFail;
        }

        private void OnDisable()
        {
            GameplayEvents.OnGameWon -= HandelOnGameWon;
            GameplayEvents.OnGameFail -= HandelOnGameFail;
        }

        public override void SetVelocity(Vector3 velocityVector)
        {
            m_rigidbody.velocity = Vector3.zero;
            m_rigidbody.AddForce(velocityVector, ForceMode.Impulse);
        }


        private void HandelOnJumpPadCollision(JumpPad jumpPad)
        {
            if (m_isGhost)
            {
                return;
            }
            CheckIfBallIsLooping(jumpPad);
            m_collidedJumpPads.Add(jumpPad);
        }

        private void CheckIfBallIsLooping(JumpPad jumpPad)
        {
            if (m_isLooping || !m_collidedJumpPads.Contains(jumpPad) || m_collidedJumpPads[0] != jumpPad)
            {
                return;
            }
            GameplayEvents.SendOnBallIsLooping();
            m_isLooping = true;
        }

        private void HandelOnGameWon()
        {
            if (m_isGhost)
            {
                return;
            }
            m_currentColorSphere.glowParticleSystem.Play();
        }
        
        private void HandelOnWrongCollision()
        {
            GameplayEvents.SendOnGameFail();
        }
        
        private void HandelOnGameFail()
        {
            Vector3 backForce = new(Random.Range(-WRONG_COLLISION_X_FORCE_MULTIPLIER,WRONG_COLLISION_X_FORCE_MULTIPLIER), 0f, -WRONG_COLLISION_Z_FORCE_MULTIPLIER);
            SetVelocity(backForce * WRONG_COLLISION_FORCE);
        }

        private void OnCollisionEnter(Collision other)
        {
            JumpPad jumpPad = other.gameObject.GetComponent<JumpPad>();
            if (jumpPad)
            {
                HandelOnJumpPadCollision(jumpPad);
                return;
            }
            HandelOnWrongCollision();
        }

        public void InitColor(int colorValue)
        {
            if (m_currentColorSphere.sphereGameObject)
            {
                m_currentColorSphere.sphereGameObject.SetActive(false);
            }
            int colorIndex = colorValue % m_colorSphereReferences.Count;
            m_currentColorSphere = m_colorSphereReferences[colorIndex];
            m_currentColorSphere.sphereGameObject.SetActive(true);
        }
    }
}
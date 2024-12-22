
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KWCreative
{
    [SelectionBase]
    public class Cannon : GhostSlotElement
    {
        private const string HORIZONTAL_AXIS = "Horizontal";
        private const KeyCode FIRE_KEY_CODE = KeyCode.Space;
        private const string ANIM_CANON_SHOOT = "Canon shoot";
        private const float CANNON_FIRE_ANIM_DELAY = 0.5f;

        [SerializeField] private Projectile m_projectilePrefab;
        [SerializeField] private Transform m_projectileFireTransform;
        [SerializeField] private LineRenderer m_lineRenderer;
        [SerializeField] private Transform m_projectileParent;
        [SerializeField] private Transform m_cannonModel;
        [SerializeField] private Animator m_cannonAnimator;
        
        private Pool<Projectile> m_projectilePool;
        private List<Projectile> m_projectileFired = new();
        private VariationData m_currentVariationData;
        private CannonData m_cannonData;
        private TrajectoryLineSimulation m_trajectoryLineSimulation;
        private float m_horizontalAxisInput;
        private Vector3 m_turnAngle;
        private Vector3 m_initialRotation;
        private Coroutine m_fireCoroutine;
        private bool m_isGameOver;

        private void Awake()
        {
            GameplayEvents.OnGameFail += HandelOnGameFail;
        }

        private void OnDestroy()
        {
            GameplayEvents.OnGameFail -= HandelOnGameFail;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            m_turnAngle = m_initialRotation = m_cannonModel.localEulerAngles;
            m_currentVariationData = CreativeConfig.Instance.CurrentVariationData;
            m_cannonData = m_currentVariationData.cannonData;
            m_trajectoryLineSimulation = new TrajectoryLineSimulation(m_projectileFireTransform, m_projectilePrefab, ref m_cannonData, m_lineRenderer);
            m_projectilePool = new Pool<Projectile>(false, m_projectilePrefab, m_cannonData.projectileCount,m_projectileParent);
            m_trajectoryLineSimulation.SimulateTrajectory();
        }

        private void Update()
        {
            HandelRotation();
            HandelProjectileFire();
        }

        private void FixedUpdate()
        {
            m_trajectoryLineSimulation.SimulateTrajectory();
        }

        private void HandelRotation()
        {
            m_horizontalAxisInput = Input.GetAxisRaw(HORIZONTAL_AXIS);
            if (Mathf.Approximately(m_horizontalAxisInput, 0f))
            {
                return;
            }
            m_turnAngle.z = Mathf.Lerp(m_turnAngle.z
                , m_cannonData.maxCannonRotationAngel * -m_horizontalAxisInput + m_initialRotation.z
                , m_cannonData.cannonRotationSpeed * Time.deltaTime);
            m_cannonModel.localEulerAngles = m_turnAngle;
        }

        private void HandelProjectileFire()
        {
            if ( m_isGameOver || !Input.GetKeyDown(FIRE_KEY_CODE))
            {
                return;
            }
            switch (GameManager.Instance.CannonFireType)
            {
                case CannonFireTypes.AUTO :
                    AutoFire();
                    break;
                case CannonFireTypes.MANUAL:
                    ManualFire();
                    break;
            }
        }

        private void AutoFire()
        {
            List<Projectile> tempProjectileList = new (m_projectileFired);
            foreach (Projectile projectile in tempProjectileList)
            {
                ReturnProjectile(projectile);
            }

            if (m_fireCoroutine != null)
            {
                StopCoroutine(m_fireCoroutine);
                m_fireCoroutine = null;
            }

            m_fireCoroutine = StartCoroutine(FireProjectile(CannonFireTypes.AUTO));
        }

        private void ManualFire()
        {
            m_fireCoroutine = StartCoroutine(FireProjectile(CannonFireTypes.MANUAL));
        }

        private IEnumerator FireProjectile(CannonFireTypes currentFireType)
        {
            if (m_projectileFired.Count == m_cannonData.projectileCount)
            {
                yield break;
            }
            WaitForSeconds projectileFireDelay = new (m_cannonData.projectileFireDelay - CANNON_FIRE_ANIM_DELAY);
            m_cannonAnimator.Play(ANIM_CANON_SHOOT);
            WaitForSeconds projectileAnimationDelay = new (CANNON_FIRE_ANIM_DELAY);
            int projectileSpawnCount = currentFireType == CannonFireTypes.AUTO ? m_cannonData.projectileCount : 1;
            for (int projectileIndex = 0; projectileIndex < projectileSpawnCount; projectileIndex++)
            {
                yield return projectileAnimationDelay;
                SpawnProjectile();
                yield return projectileFireDelay;
            }
        }

        private void SpawnProjectile()
        {
            Projectile projectileToFire = m_projectilePool.GetElement();
            projectileToFire.transform.position = m_projectileFireTransform.position;
            projectileToFire.SetVelocity(m_projectileFireTransform.forward * m_cannonData.projectileFireForce);
            projectileToFire.SetCannon(this);
            m_projectileFired.Add(projectileToFire);
            Ball ballFired = (Ball)projectileToFire;
            ballFired.InitColor(m_projectileFired.Count);
        }

        public void ReturnProjectile(Projectile projectile)
        {
            m_projectilePool.ReturnElement(projectile);
            m_projectileFired.Remove(projectile);
        }
        private void HandelOnGameFail()
        {
            m_isGameOver = true;
            if (m_fireCoroutine == null)
            {
                return;
            }
            StopCoroutine(m_fireCoroutine);
            m_fireCoroutine = null;
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
        }
    }
}
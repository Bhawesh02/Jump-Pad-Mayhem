using UnityEngine;

namespace KWCreative
{
    public class TrajectoryLineSimulation
    {
        private readonly Transform m_projectileFireTransform;
        private readonly TrajectorySimulationSceneManager m_trajectorySimulationSceneManager;
        private readonly Projectile m_ghostProjectile;
        private readonly CannonData m_cannonData;
        private readonly LineRenderer m_lineRenderer;

        public TrajectoryLineSimulation(Transform projectileFireTransform, Projectile ghostProjectilePrefab, ref CannonData cannonData, LineRenderer lineRenderer)
        {
            m_projectileFireTransform = projectileFireTransform;
            m_trajectorySimulationSceneManager = TrajectorySimulationSceneManager.Instance;
            m_cannonData = cannonData;
            m_lineRenderer = lineRenderer;
            m_ghostProjectile = m_trajectorySimulationSceneManager.SendObjectToSimulationScene(ghostProjectilePrefab);
            m_ghostProjectile.gameObject.SetActive(false);
        }

        public void SimulateTrajectory()
        {
            m_ghostProjectile.gameObject.SetActive(true);
            m_ghostProjectile.transform.position = m_projectileFireTransform.position;
            m_ghostProjectile.SetVelocity(m_projectileFireTransform.forward * m_cannonData.projectileFireForce);
            m_lineRenderer.positionCount = CreativeConfig.Instance.CurrentVariationData.maxTrajectorySimulationIteration;
            for (int frameIndex = 0; frameIndex < CreativeConfig.Instance.CurrentVariationData.maxTrajectorySimulationIteration; frameIndex++)
            {
                m_trajectorySimulationSceneManager.TrajectorySimulationPhysicsScene.Simulate(Time.fixedDeltaTime);
                m_lineRenderer.SetPosition(frameIndex, m_ghostProjectile.transform.position);
            }
            m_ghostProjectile.gameObject.SetActive(false);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace KWCreative
{
    public class TrajectorySimulationSceneManager : MonoSingleton<TrajectorySimulationSceneManager>
    {
        private const string SCENE_NAME = "Trajectory Projection Scene";

        [SerializeField] private LevelGenerator m_levelGenerator;
        
        private Scene m_trajectorySimulationScene;
        
        public PhysicsScene TrajectorySimulationPhysicsScene => m_trajectorySimulationScene.GetPhysicsScene();
        
        protected override void Init()
        { 
            //Do Nothing
        }

        private void Start()
        {
            CreatePhysicsScene();
        }

        private void CreatePhysicsScene()
        {
            m_trajectorySimulationScene = SceneManager.CreateScene(SCENE_NAME,
                new CreateSceneParameters(LocalPhysicsMode.Physics3D));
            List<JumpSurface> jumpSurfaces = m_levelGenerator.JumpSurfaces;
            JumpSurface ghostJumpSurface;
            foreach (JumpSurface jumpSurface in jumpSurfaces)
            {
                ghostJumpSurface = SendObjectToSimulationScene(jumpSurface);
                jumpSurface.SetGhostObjectTransform(ghostJumpSurface.transform);
            }
        }

        public T SendObjectToSimulationScene<T>(T ghostObject) where T : GhostObject
        {
            Transform ghostObjectTransform = ghostObject.transform;
            T ghostObjectDuplicate = Instantiate(ghostObject, ghostObjectTransform.position, ghostObjectTransform.rotation);
            ghostObjectDuplicate.SetIsGhost();
            SceneManager.MoveGameObjectToScene(ghostObjectDuplicate.gameObject, m_trajectorySimulationScene);
            return ghostObjectDuplicate;
        }
    }
}
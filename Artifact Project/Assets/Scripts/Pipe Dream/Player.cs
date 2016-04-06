using UnityEngine;

namespace PipeDream
{
    public class Player : MonoBehaviour
    {
        public PipeSystem pipeSystem;
        public float velocity;

        Transform world;

        float deltaToRotation;
        float systemRotation;
        float worldRotation;

        float distanceTravelled;

        Pipe currentPipe;

        void Start()
        {
            world = pipeSystem.transform.parent;
            currentPipe = pipeSystem.SetupFirstPipe();
            SetupCurrentPipe();
        }

        void Update()
        {
            float delta = velocity * Time.deltaTime;
            distanceTravelled += delta;
            systemRotation += delta * deltaToRotation;
            if (systemRotation >= currentPipe.CurveAngle)
            {
                delta = (systemRotation - currentPipe.CurveAngle) / deltaToRotation;
                currentPipe = pipeSystem.SetupNextPipe();
                SetupCurrentPipe();
                systemRotation = delta * deltaToRotation;
            }
            pipeSystem.transform.localRotation = Quaternion.Euler(0f, 0f, systemRotation);
        }

        void SetupCurrentPipe ()
        {
            deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
            worldRotation += currentPipe.RelativeRotation;
            if (worldRotation < 0f)
                worldRotation += 360f;
            else if (worldRotation > 360f)
                worldRotation -= 360f;
            world.localRotation = Quaternion.Euler(worldRotation, 0f, 0f);
        }
    }
}

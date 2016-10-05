using UnityEngine;

namespace PipeDream
{
    public class Player : MonoBehaviour
    {
        public PipeSystem pipeSystem;
        public float velocity, rotationalVelocity;

        Transform world, rotater;

        float deltaToRotation;
        float systemRotation;
        float worldRotation, avatarRotation;
        float addRotation = 0;

        public float distanceTravelled;

        Pipe currentPipe;

        void Start()
        {
            world = pipeSystem.transform.parent;
            rotater = transform.GetChild(0);
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
            UpdateAvatarRotation();
        }

        void UpdateAvatarRotation ()
        {
            addRotation = Mathf.Lerp(addRotation, rotationalVelocity * Time.deltaTime * Input.GetAxis("Horizontal"), Mathf.Abs(Input.GetAxis("Horizontal")) * .1f);
            avatarRotation += addRotation;
            if (avatarRotation < 0f)
                avatarRotation += 360f;
            else if (avatarRotation > 360f)
                avatarRotation -= 360f;
            rotater.localRotation = Quaternion.Euler(avatarRotation, 0, 0);
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

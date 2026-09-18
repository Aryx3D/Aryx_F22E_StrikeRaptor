using UnityEngine;

namespace Aryx_F22E_StrikeRaptor
{
    public sealed class Aryx_OverpressureVentController : MonoBehaviour
    {
        [SerializeField] private Aircraft aircraft;
        [SerializeField] private Transform[] slats;

        [SerializeField] private float minThrottle = 0.8f;
        [SerializeField] private float maxThrottle = 1f;
        [SerializeField] private float minSpeed = 0f;
        [SerializeField] private float maxSpeed = 150f;

        [SerializeField] private float maxSlatAngle = 30f;
        [SerializeField] private float deploymentSpeed = 5f;
        [SerializeField] private AnimationCurve deploymentBlend;

        private ControlInputs controlInputs;
        private float deployment;
        private bool initialised;
        private float targetDeployment;
        private float angle;

        private void Start()
        {
            if (aircraft == null || slats == null || slats.Length == 0)
                return;
            controlInputs = aircraft.GetInputs();
            if (controlInputs == null)
                return;
            for (int i = 0; i < slats.Length; i++)
            {
                if (slats[i] == null)
                    return;
            }

            initialised = true;
        }

        private void Update()
        {
            if (!initialised)
                return;

            targetDeployment = CalculateDeployment();
            deployment = Mathf.MoveTowards(deployment, targetDeployment, deploymentSpeed * Time.deltaTime);

            angle = deploymentBlend.Evaluate(deployment) * maxSlatAngle;

            for (int i = 0; i < slats.Length; i++)
                slats[i].localRotation = Quaternion.Euler(angle, slats[i].localRotation.y, slats[i].localRotation.z);
        }

        private float CalculateDeployment()
        {
            if (controlInputs.throttle < minThrottle || controlInputs.throttle > maxThrottle)
                return 0f;

            if (aircraft.speed < minSpeed || aircraft.speed > maxSpeed)
                return 0f;

            float throttleFactor = Mathf.InverseLerp(minThrottle, maxThrottle, controlInputs.throttle);
            float speedFactor = Mathf.InverseLerp(minSpeed, maxSpeed, aircraft.speed);

            return throttleFactor * speedFactor;
        }
    }
}
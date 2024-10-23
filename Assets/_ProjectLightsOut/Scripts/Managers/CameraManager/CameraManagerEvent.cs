using ProjectLightsOut.DevUtils;

namespace ProjectLightsOut.Managers
{
    public class CameraShakeEvent : GameEvent
    {
        public float Duration;
        public float Magnitude;
        public CameraShakeEvent(float duration, float magnitude)
        {
            Duration = duration;
            Magnitude = magnitude;
        }
    }
}
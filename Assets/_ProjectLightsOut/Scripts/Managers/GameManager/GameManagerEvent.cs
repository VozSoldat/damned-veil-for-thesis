using ProjectLightsOut.DevUtils;

namespace ProjectLightsOut.Managers
{
    public class OnSlowTime : GameEvent
    {
        public float TimeScale;
        public float Duration;
        public OnSlowTime(float timeScale, float duration)
        {
            TimeScale = timeScale;
            Duration = duration;
        }

        public OnSlowTime()
        {
            TimeScale = 0.5f;
            Duration = 0.5f;
        }
    }
}

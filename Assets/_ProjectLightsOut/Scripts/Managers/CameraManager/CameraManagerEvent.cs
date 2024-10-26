using UnityEngine;
using ProjectLightsOut.DevUtils;

namespace ProjectLightsOut.Managers
{
    public class OnCameraShake : GameEvent
    {
        public float Duration;
        public float Magnitude;
        public OnCameraShake(float duration, float magnitude)
        {
            Duration = duration;
            Magnitude = magnitude;
        }
    }

    public class OnSpotting : GameEvent
    {
        public Transform Target;
        public float MoveTime = 1f;
        public OnSpotting(Transform target, float moveTime = 1f)
        {
            Target = target;
            MoveTime = moveTime;
        }
    }

    public class OnSpottingEnd : GameEvent
    {
        public float MoveTime = 1f;
        public OnSpottingEnd(float moveTime = 1f)
        {
            MoveTime = moveTime;
        }
    }

    public class OnZoom : GameEvent
    {
        public float Zoom;
        public float ZoomSpeed;
        public OnZoom(float zoom, float zoomSpeed)
        {
            Zoom = zoom;
            ZoomSpeed = zoomSpeed;
        }
    }

    public class OnZoomEnd : GameEvent
    {
        public float ZoomSpeed;
        public OnZoomEnd(float zoomSpeed)
        {
            ZoomSpeed = zoomSpeed;
        }
    }
}
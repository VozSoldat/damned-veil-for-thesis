using UnityEngine;

namespace ProjectLightsOut.DevUtils
{
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteMask))]
    public class SpriteMaskOverrider : MonoBehaviour
    {
        [SerializeField, Tooltip("Use this field to control mask's sprite via animation instead SpriteMask.sprite.")]
        Sprite sprite;

        SpriteMask mask;


    #if UNITY_EDITOR
        private void OnGUI()
        {
            if(mask == null)
                Start();

            Update();
        }
    #endif

        private void Start()
        {
            TryGetComponent(out mask);
        }

        private void Update()
        {
            if(mask != null)
                mask.sprite = sprite;
        }
    }
}
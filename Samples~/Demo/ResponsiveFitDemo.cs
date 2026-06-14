using UnityEngine;

namespace KidzDev.Unity.ResponsiveFit.Samples
{
    /// <summary>
    /// Minimal demo: re-fits a <see cref="ResponsiveItemSizer"/> on start and
    /// whenever the screen size changes, over a few placeholder cells. Shows the
    /// intended integration shape even though the sizer itself is a Phase 1 stub.
    /// </summary>
    public class ResponsiveFitDemo : MonoBehaviour
    {
        [SerializeField] private ResponsiveItemSizer sizer;
        [SerializeField] private RectTransform[] cells;

        private Vector2 _lastScreenSize;

        private void Start()
        {
            _lastScreenSize = new Vector2(Screen.width, Screen.height);
            Refit();
        }

        private void Update()
        {
            var screen = new Vector2(Screen.width, Screen.height);
            if (screen == _lastScreenSize)
                return;

            _lastScreenSize = screen;
            Refit();
        }

        private void Refit()
        {
            if (sizer == null)
            {
                Debug.LogWarning($"{nameof(ResponsiveFitDemo)}: no {nameof(ResponsiveItemSizer)} assigned.");
                return;
            }

            // A real layout would read the computed size back from the sizer;
            // for the demo we just drive the recalculate/apply cycle so the
            // wiring is observable while the engine is a stub.
            sizer.Recalculate();
            sizer.Apply();
            Debug.Log($"Refit {cells?.Length ?? 0} cell(s) for screen {_lastScreenSize}.");
        }
    }
}

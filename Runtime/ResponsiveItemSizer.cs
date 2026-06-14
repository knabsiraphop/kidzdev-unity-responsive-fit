using UnityEngine;

namespace KidzDev.Unity.ResponsiveFit
{
    // Build phases (incremental — only the scaffold exists today):
    //   Phase 1: ItemsPerView mode — divide the viewport by a target item count.
    //   Phase 2: AspectRatio mode — size items to a fixed width:height ratio.
    //   Phase 3: FixedColumns mode — N columns across, square/derived height.
    //   Phase 4: safe-area insets (notch / rounded corners) honoured per mode.
    /// <summary>
    /// Resizes uGUI items to fit the current screen using one of several
    /// <see cref="FitMode"/> strategies. Recalculates automatically when the
    /// owning RectTransform's dimensions change. The computed size can be
    /// applied to a <c>GridLayoutGroup</c> / item rects, or read by another
    /// system as an <see cref="IScreenFitStrategy"/> size provider.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class ResponsiveItemSizer : MonoBehaviour
    {
        public enum FitMode { ItemsPerView, AspectRatio, FixedColumns }

        [Header("Fit")]
        [SerializeField] private FitMode mode = FitMode.ItemsPerView;

        [Tooltip("ItemsPerView: how many items should be visible across the viewport.")]
        [SerializeField] private float itemsPerView = 3f;

        [Tooltip("AspectRatio: target width:height ratio for each item.")]
        [SerializeField] private Vector2 aspectRatio = new Vector2(1f, 1f);

        [Tooltip("FixedColumns: number of columns to lay out across the viewport.")]
        [SerializeField] private int columns = 2;

        [Header("Screen")]
        [Tooltip("Inset the layout to Screen.safeArea (notches, rounded corners).")]
        [SerializeField] private bool respectSafeArea;

        /// <summary>Recomputes the target item size from the current mode and viewport.</summary>
        public void Recalculate()
        {
            // TODO: branch on `mode`, measure the viewport (honouring
            // `respectSafeArea`), and cache the resulting item size.
        }

        /// <summary>Pushes the most recent <see cref="Recalculate"/> result onto the layout.</summary>
        public void Apply()
        {
            // TODO: write the cached size to the GridLayoutGroup / item rects.
        }

        protected virtual void OnRectTransformDimensionsChange()
        {
            // TODO: re-fit when the viewport changes (rotation, resize, anchor
            // changes). Stubbed for now.
            Recalculate();
        }
    }
}

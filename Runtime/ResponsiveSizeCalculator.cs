using UnityEngine;

namespace KidzDev.Unity.ResponsiveFit
{
    /// <summary>
    /// Calculates the item size that fits exactly <c>rows × columns</c> items into
    /// the viewport — without requiring or driving any layout group. Use this when
    /// another system (e.g. a recyclable scroll view) handles item placement itself
    /// and only needs the computed <see cref="ResponsiveItemSizer.LastSize"/>.
    /// <para>
    /// Set <c>padding</c> and <c>spacing</c> to match the consuming system's own
    /// layout settings so the size math stays consistent. Subscribe to
    /// <see cref="ResponsiveItemSizer.OnSizeChanged"/> to react to viewport resizes.
    /// </para>
    /// </summary>
    [AddComponentMenu("Layout/Responsive Size Calculator")]
    [DisallowMultipleComponent]
    public sealed class ResponsiveSizeCalculator : ResponsiveItemSizer
    {
        [Header("Fit")]
        [Tooltip("Items visible down the viewport (main axis for vertical scroll).")]
        [SerializeField, Min(1)] private int rows = 3;

        [Tooltip("Items visible across the viewport (main axis for horizontal scroll).")]
        [SerializeField, Min(1)] private int columns = 1;

        [Header("Layout")]
        [Tooltip("Match this to the consuming layout's padding so the size accounts for edge offsets.")]
        [SerializeField] private RectOffset padding = new RectOffset();

        [Tooltip("Match this to the consuming layout's spacing so the size accounts for inter-item gaps.")]
        [SerializeField] private Vector2 spacing;

        [Header("Aspect (optional)")]
        [Tooltip("Keep a fixed item aspect by deriving one dimension from the other (mirrors AspectRatioFitter). Off = independent columns/rows.")]
        [SerializeField] private AspectMode aspectMode = AspectMode.None;

        [Tooltip("Item aspect as width / height, used when AspectMode is not None.")]
        [Min(0.01f)]
        [SerializeField] private float aspectRatio = 1f;

        /// <summary>Items visible down the viewport. Re-fits on set.</summary>
        public int Rows    { get => rows;    set { rows    = Mathf.Max(1, value); Refit(); } }

        /// <summary>Items visible across the viewport. Re-fits on set.</summary>
        public int Columns { get => columns; set { columns = Mathf.Max(1, value); Refit(); } }

        protected override RectOffset LayoutPadding => padding;

        protected override Vector2 ComputeItemSize(Rect available)
        {
            float w = DivideAcross(available.width,  Mathf.Max(1, columns), spacing.x);
            float h = DivideAcross(available.height, Mathf.Max(1, rows),    spacing.y);

            if (aspectMode == AspectMode.WidthControlsHeight && aspectRatio > 0f)
                h = w / aspectRatio;
            else if (aspectMode == AspectMode.HeightControlsWidth && aspectRatio > 0f)
                w = h * aspectRatio;

            return new Vector2(w, h);
        }

        protected override void ApplyItemSize(Vector2 size) { }
    }
}

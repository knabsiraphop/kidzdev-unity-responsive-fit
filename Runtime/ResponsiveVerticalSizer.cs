using UnityEngine;
using UnityEngine.UI;

namespace KidzDev.Unity.ResponsiveFit
{
    /// <summary>
    /// Drives a <see cref="VerticalLayoutGroup"/> by sizing every direct child
    /// through its <see cref="LayoutElement"/> so that exactly <c>rows</c> items are
    /// visible down the viewport — the vertical analogue of
    /// <see cref="ResponsiveGridSizer"/>. Item height comes from the rows; item width
    /// fills the column, or is locked to <c>aspectRatio</c> when <c>lockAspect</c> is
    /// on. Enable the group's <c>Child Control Width/Height</c> (and disable
    /// <c>Child Force Expand</c>) so the sizes take effect.
    /// </summary>
    [AddComponentMenu("Layout/Responsive Vertical Sizer")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public sealed class ResponsiveVerticalSizer : ResponsiveItemSizer
    {
        [Header("Vertical")]
        [Tooltip("Items visible down the viewport at once.")]
        [Min(1)]
        [SerializeField] private int rows = 3;

        [Header("Aspect (optional)")]
        [Tooltip("Off = items fill the column width. On = item width is derived from its height via Aspect Ratio.")]
        [SerializeField] private bool lockAspect;

        [Tooltip("Item aspect as width / height, used when Lock Aspect is on.")]
        [Min(0.01f)]
        [SerializeField] private float aspectRatio = 1f;

        private VerticalLayoutGroup _group;

        private VerticalLayoutGroup Group =>
            _group != null ? _group : (_group = GetComponent<VerticalLayoutGroup>());

        /// <summary>Items visible down the viewport. Re-fits on set.</summary>
        public int Rows { get => rows; set { rows = Mathf.Max(1, value); Refit(); } }

        protected override RectOffset LayoutPadding => Group != null ? Group.padding : null;

        protected override Vector2 ComputeItemSize(Rect available)
        {
            int rws = Mathf.Max(1, rows);
            float spacingY = Group != null ? Group.spacing : 0f;

            float height = DivideAcross(available.height, rws, spacingY);
            float width = (lockAspect && aspectRatio > 0f) ? height * aspectRatio : available.width;
            return new Vector2(width, height);
        }

        protected override void ApplyItemSize(Vector2 size) => ApplyToChildren(transform, size);
    }
}

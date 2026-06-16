using UnityEngine;
using UnityEngine.UI;

namespace KidzDev.Unity.ResponsiveFit
{
    /// <summary>
    /// Drives a <see cref="HorizontalLayoutGroup"/> by sizing every direct child
    /// through its <see cref="LayoutElement"/> so that exactly <c>columns</c> items
    /// are visible across the viewport — the horizontal analogue of
    /// <see cref="ResponsiveGridSizer"/>. Item width comes from the columns; item
    /// height fills the row, or is locked to <c>aspectRatio</c> when
    /// <c>lockAspect</c> is on. Enable the group's <c>Child Control Width/Height</c>
    /// (and disable <c>Child Force Expand</c>) so the sizes take effect.
    /// </summary>
    [AddComponentMenu("Layout/Responsive Horizontal Sizer")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public sealed class ResponsiveHorizontalSizer : ResponsiveItemSizer
    {
        [Header("Horizontal")]
        [Tooltip("Items visible across the viewport at once.")]
        [Min(1)]
        [SerializeField] private int columns = 3;

        [Header("Aspect (optional)")]
        [Tooltip("Off = items fill the row height. On = item height is derived from its width via Aspect Ratio.")]
        [SerializeField] private bool lockAspect;

        [Tooltip("Item aspect as width / height, used when Lock Aspect is on.")]
        [Min(0.01f)]
        [SerializeField] private float aspectRatio = 1f;

        private HorizontalLayoutGroup _group;

        private HorizontalLayoutGroup Group =>
            _group != null ? _group : (_group = GetComponent<HorizontalLayoutGroup>());

        /// <summary>Items visible across the viewport. Re-fits on set.</summary>
        public int Columns { get => columns; set { columns = Mathf.Max(1, value); Refit(); } }

        protected override RectOffset LayoutPadding => Group != null ? Group.padding : null;

        protected override Vector2 ComputeItemSize(Rect available)
        {
            int cols = Mathf.Max(1, columns);
            float spacingX = Group != null ? Group.spacing : 0f;

            float width = DivideAcross(available.width, cols, spacingX);
            float height = (lockAspect && aspectRatio > 0f) ? width / aspectRatio : available.height;
            return new Vector2(width, height);
        }

        protected override void ApplyItemSize(Vector2 size) => ApplyToChildren(transform, size);
    }
}

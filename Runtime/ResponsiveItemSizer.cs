using UnityEngine;
using UnityEngine.UI;

namespace KidzDev.Unity.ResponsiveFit
{
    /// <summary>
    /// Base component that resizes uGUI items to fit a UI <c>RectTransform</c>
    /// viewport. It measures the owning RectTransform (or an explicit
    /// <c>viewportOverride</c>), asks the subclass to compute an item size for that
    /// area, caches it, and pushes it onto the concrete layout group.
    ///
    /// Subclasses decide the sizing rule and the target layout group:
    /// <see cref="ResponsiveGridSizer"/> (fixed columns × rows on a GridLayoutGroup),
    /// <see cref="ResponsiveHorizontalSizer"/> / <see cref="ResponsiveVerticalSizer"/>
    /// (a per-child size on a Horizontal/VerticalLayoutGroup).
    ///
    /// Recalculates automatically when the RectTransform's dimensions change and
    /// live while editing thanks to <see cref="ExecuteAlways"/>. Also implements
    /// <see cref="IViewportFitStrategy"/> so the computed size can be read by another
    /// system (e.g. a recyclable scroll view) without coupling the packages.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public abstract class ResponsiveItemSizer : MonoBehaviour, IViewportFitStrategy
    {
        [Header("Viewport")]
        [Tooltip("Optional. Measure this RectTransform instead of our own — assign the Mask/Viewport when this object is oversized scroll Content.")]
        [SerializeField] private RectTransform viewportOverride;

        private RectTransform _rectTransform;
        private Vector2 _cachedSize;

        /// <summary>The most recent size produced by <see cref="Recalculate"/>.</summary>
        public Vector2 LastSize => _cachedSize;

        /// <summary>Optional RectTransform to measure instead of our own. Re-fits on set.</summary>
        public RectTransform ViewportOverride { get => viewportOverride; set { viewportOverride = value; Refit(); } }

        protected RectTransform SelfRect =>
            _rectTransform != null ? _rectTransform : (_rectTransform = (RectTransform)transform);

        // --- subclass contract ---------------------------------------------

        /// <summary>Computes the item size for <paramref name="available"/> — the viewport already net of the layout group's padding.</summary>
        protected abstract Vector2 ComputeItemSize(Rect available);

        /// <summary>Padding from the target layout group, subtracted from the viewport before sizing.</summary>
        protected abstract RectOffset LayoutPadding { get; }

        /// <summary>Pushes the computed <paramref name="size"/> onto the concrete layout group.</summary>
        protected abstract void ApplyItemSize(Vector2 size);

        // --- lifecycle -----------------------------------------------------

        private void OnEnable() => Refit();

        protected virtual void OnRectTransformDimensionsChange() => Refit();

        // --- public API ----------------------------------------------------

        /// <summary>Recalculate then Apply in one call.</summary>
        public void Refit()
        {
            Recalculate();
            Apply();
        }

        /// <summary>Recomputes the target item size from the measured viewport.</summary>
        public void Recalculate()
        {
            Rect viewport = viewportOverride != null ? viewportOverride.rect : SelfRect.rect;

            // Subtract the layout group's padding so items fit inside it; the
            // subclass then accounts for the inter-item spacing.
            RectOffset padding = LayoutPadding;
            float availableWidth = viewport.width - (padding != null ? padding.horizontal : 0);
            float availableHeight = viewport.height - (padding != null ? padding.vertical : 0);
            var available = new Rect(0f, 0f, Mathf.Max(0f, availableWidth), Mathf.Max(0f, availableHeight));

            _cachedSize = ComputeItemSize(available);
        }

        /// <summary>Pushes the most recent <see cref="Recalculate"/> result onto the layout.</summary>
        public void Apply() => ApplyItemSize(_cachedSize);

        /// <inheritdoc />
        public Vector2 CalculateItemSize(Rect viewport, int index)
        {
            // `index` is accepted for the IViewportFitStrategy contract
            // (variable-size consumers) and is ignored here; `viewport` is treated
            // as the available area.
            return ComputeItemSize(viewport);
        }

        // --- shared helpers ------------------------------------------------

        /// <summary>Size of one item when <paramref name="number"/> of them (with <paramref name="spacing"/> between) span <paramref name="total"/>.</summary>
        protected static float DivideAcross(float total, float number, float spacing)
        {
            float net = total - spacing * (number - 1f);
            return Mathf.Max(0f, net) / number;
        }

        /// <summary>Sets every direct child's <see cref="LayoutElement"/> preferred size to <paramref name="size"/> (adds one if missing). Shared by the horizontal/vertical sizers.</summary>
        protected static void ApplyToChildren(Transform parent, Vector2 size)
        {
            int childCount = parent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = parent.GetChild(i) as RectTransform;
                if (child == null)
                    continue;

                if (!child.TryGetComponent(out LayoutElement element))
                    element = child.gameObject.AddComponent<LayoutElement>();

                // LayoutElement's setters dirty the layout only when the value changes.
                element.preferredWidth = size.x;
                element.preferredHeight = size.y;
            }
        }

        protected static bool Approximately(Vector2 a, Vector2 b) =>
            Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Layout/serialized state isn't safe to act on during OnValidate; defer a frame.
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this != null)
                    Refit();
            };
        }
#endif
    }
}

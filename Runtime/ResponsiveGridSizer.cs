using UnityEngine;
using UnityEngine.UI;

namespace KidzDev.Unity.ResponsiveFit
{
    /// <summary>
    /// Drives a <see cref="GridLayoutGroup"/> by sizing its <c>cellSize</c> so that
    /// exactly <c>columns</c> columns and <c>rows</c> rows are visible in the
    /// viewport at once — a port of the classic DynamicGridSize model. Cell width
    /// comes from the columns, cell height from the rows; an optional
    /// <see cref="AspectMode"/> instead derives one dimension from the other to keep
    /// a fixed cell aspect.
    /// </summary>
    [AddComponentMenu("Layout/Responsive Grid Sizer")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GridLayoutGroup))]
    public sealed class ResponsiveGridSizer : ResponsiveItemSizer
    {
        [Header("Grid")]
        [Tooltip("Columns visible across the viewport at once.")]
        [Min(1)]
        [SerializeField] private int columns = 3;

        [Tooltip("Rows visible down the viewport at once.")]
        [Min(1)]
        [SerializeField] private int rows = 2;

        [Header("Aspect (optional)")]
        [Tooltip("Keep a fixed cell aspect by deriving one dimension from the other (mirrors AspectRatioFitter). Off = independent columns/rows.")]
        [SerializeField] private AspectMode aspectMode = AspectMode.None;

        [Tooltip("Cell aspect as width / height, used when AspectMode is not None.")]
        [Min(0.01f)]
        [SerializeField] private float aspectRatio = 1f;

        private GridLayoutGroup _grid;

        private GridLayoutGroup Grid =>
            _grid != null ? _grid : (_grid = GetComponent<GridLayoutGroup>());

        /// <summary>Columns visible across the viewport. Re-fits on set.</summary>
        public int Columns { get => columns; set { columns = Mathf.Max(1, value); Refit(); } }

        /// <summary>Rows visible down the viewport. Re-fits on set.</summary>
        public int Rows { get => rows; set { rows = Mathf.Max(1, value); Refit(); } }

        protected override RectOffset LayoutPadding => Grid != null ? Grid.padding : null;

        protected override Vector2 ComputeItemSize(Rect available)
        {
            int cols = Mathf.Max(1, columns);
            int rws = Mathf.Max(1, rows);
            Vector2 spacing = Grid != null ? Grid.spacing : Vector2.zero;

            // Width from the columns, height from the rows — DynamicGridSize model.
            float cellWidth = DivideAcross(available.width, cols, spacing.x);
            float cellHeight = DivideAcross(available.height, rws, spacing.y);

            if (aspectMode == AspectMode.WidthControlsHeight && aspectRatio > 0f)
                cellHeight = cellWidth / aspectRatio;
            else if (aspectMode == AspectMode.HeightControlsWidth && aspectRatio > 0f)
                cellWidth = cellHeight * aspectRatio;

            return new Vector2(cellWidth, cellHeight);
        }

        protected override void ApplyItemSize(Vector2 size)
        {
            GridLayoutGroup grid = Grid;
            if (grid == null || Approximately(grid.cellSize, size))
                return;

            grid.cellSize = size;
        }
    }
}

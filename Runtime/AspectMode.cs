namespace KidzDev.Unity.ResponsiveFit
{
    /// <summary>
    /// How (if at all) an item keeps a fixed aspect ratio, mirroring <c>AspectRatioFitter</c>.
    /// Shared by <see cref="ResponsiveGridSizer"/> and <see cref="ResponsiveSizeCalculator"/>.
    /// </summary>
    public enum AspectMode
    {
        /// <summary>Off — width and height are computed independently from the viewport.</summary>
        None,
        /// <summary>Width is computed from the viewport; height is derived from it via <c>aspectRatio</c>.</summary>
        WidthControlsHeight,
        /// <summary>Height is computed from the viewport; width is derived from it via <c>aspectRatio</c>.</summary>
        HeightControlsWidth,
    }
}

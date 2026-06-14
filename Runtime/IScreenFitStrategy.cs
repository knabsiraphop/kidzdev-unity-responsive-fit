using UnityEngine;

namespace KidzDev.Unity.ResponsiveFit
{
    /// <summary>
    /// Computes the pixel size an item should occupy so a layout fits the
    /// available screen/viewport. Implementations back the different
    /// <c>FitMode</c>s of <see cref="ResponsiveItemSizer"/> (items-per-view,
    /// aspect ratio, fixed columns) and may also be consumed directly as a size
    /// provider by other packages (e.g. a recyclable scroll view).
    /// </summary>
    public interface IScreenFitStrategy
    {
        /// <summary>
        /// Returns the desired size, in pixels, for the item at
        /// <paramref name="index"/> given the current <paramref name="viewport"/>.
        /// </summary>
        Vector2 CalculateItemSize(Rect viewport, int index);
    }
}

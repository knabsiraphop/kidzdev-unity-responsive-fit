using UnityEngine;

namespace KidzDev.Unity.ResponsiveFit
{
    /// <summary>
    /// Computes the pixel size an item should occupy so a layout fits inside a UI
    /// <c>RectTransform</c> viewport — <b>not</b> the device screen. Implementations
    /// back the concrete responsive sizers and can also be consumed directly as a
    /// size provider by other packages — e.g. a recyclable scroll view sizing its
    /// rows to its viewport RectTransform.
    /// </summary>
    public interface IViewportFitStrategy
    {
        /// <summary>
        /// Returns the desired size, in pixels, for the item at
        /// <paramref name="index"/> given the current <paramref name="viewport"/>
        /// rect (the area available inside the owning RectTransform).
        /// </summary>
        Vector2 CalculateItemSize(Rect viewport, int index);
    }
}

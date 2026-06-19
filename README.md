# KidzDev Unity Responsive Fit

Responsive item sizing for Unity uGUI. Sizes the items of a layout group so a
fixed number fit the available **RectTransform viewport**, and re-fits
automatically when that viewport changes (resize, rotation, safe-area panel).

## Install

Add via Package Manager → *Add package from git URL*, or edit
`Packages/manifest.json`:

```
https://github.com/knabsiraphop/kidzdev-unity-responsive-fit.git#v2.0.0
```

---

## Which component do I need?

| Scenario | Component | Requires |
| --- | --- | --- |
| Grid of items that fills the screen (e.g. icon grid, card gallery) | `ResponsiveGridSizer` | `GridLayoutGroup` |
| Horizontal strip of items (e.g. horizontal carousel, tab bar) | `ResponsiveHorizontalSizer` | `HorizontalLayoutGroup` |
| Vertical list of items (e.g. vertical feed, menu list) | `ResponsiveVerticalSizer` | `VerticalLayoutGroup` |
| Recyclable scroll view that manages its own items | `ResponsiveSizeCalculator` | nothing (no layout group) |

---

## Components

### `ResponsiveGridSizer`

Drives a `GridLayoutGroup` by setting `cellSize` so exactly `Columns` × `Rows`
cells are visible in the viewport at once.

**Inspector setup:**

1. Add a `GridLayoutGroup` to your container.
2. Add `Responsive Grid Sizer` (menu: *Layout → Responsive Grid Sizer*) to the
   same GameObject.
3. Set **Columns** and **Rows** to the number of cells you want visible.
4. Optionally set **Aspect Mode** (see below) and **Aspect Ratio** to keep square
   or fixed-ratio cells.
5. (Inside a ScrollRect) Set **Viewport Override** to the ScrollRect's Viewport.

**Runtime API:**

```csharp
// Get the sizer
var sizer = GetComponent<ResponsiveGridSizer>();

// Change column/row count at runtime — triggers an immediate Refit
sizer.Columns = 4;
sizer.Rows    = 3;

// Force a refit manually
sizer.Refit();

// Read the last computed cell size
Vector2 cellSize = sizer.LastSize;

// React to size changes
sizer.OnSizeChanged += size => Debug.Log($"Cell size: {size}");
```

**Aspect Mode:**

The `AspectMode` enum (top-level in `KidzDev.Unity.ResponsiveFit`) controls
whether one cell dimension is derived from the other:

| Value | Behaviour |
| --- | --- |
| `AspectMode.None` | Width comes from the column count; height comes from the row count — fully independent. |
| `AspectMode.WidthControlsHeight` | Width is computed from the columns; height = width / aspectRatio. |
| `AspectMode.HeightControlsWidth` | Height is computed from the rows; width = height × aspectRatio. |

> **Migration note (v2.0.0):** `AspectMode` was previously a nested type inside
> `ResponsiveGridSizer`. Update any references from `ResponsiveGridSizer.AspectMode`
> to the top-level `AspectMode`.

---

### `ResponsiveHorizontalSizer`

Drives a `HorizontalLayoutGroup` by setting each direct child's `LayoutElement`
preferred size so exactly `Columns` items are visible across the viewport. Item
height fills the row by default, or is locked to a fixed aspect ratio.

**Inspector setup:**

1. Add a `HorizontalLayoutGroup` to your container.
   - Enable **Child Control Width** and **Child Control Height**.
   - Disable **Child Force Expand Width** and **Child Force Expand Height**.
2. Add `Responsive Horizontal Sizer` (menu: *Layout → Responsive Horizontal Sizer*).
3. Set **Columns** to the number of items visible across.
4. Optionally enable **Lock Aspect** and set **Aspect Ratio** (width/height) to
   keep each item at a fixed ratio instead of filling the row height.
5. (Inside a ScrollRect) Set **Viewport Override** to the ScrollRect's Viewport.

**Runtime API:**

```csharp
var sizer = GetComponent<ResponsiveHorizontalSizer>();

// Change column count at runtime
sizer.Columns = 3;

// Force a refit
sizer.Refit();

// Read the last computed item size
Vector2 itemSize = sizer.LastSize;

// React to size changes
sizer.OnSizeChanged += size => Debug.Log($"Item size: {size}");
```

**Example: horizontal carousel with square items**

Inspector:
- `Columns` = 3
- `Lock Aspect` = true
- `Aspect Ratio` = 1.0 (square)

Result: each item is `viewport.width / 3` wide, with height = width (square).

---

### `ResponsiveVerticalSizer`

Drives a `VerticalLayoutGroup` by setting each direct child's `LayoutElement`
preferred size so exactly `Rows` items are visible down the viewport. Item width
fills the column by default, or is locked to a fixed aspect ratio.

**Inspector setup:**

1. Add a `VerticalLayoutGroup` to your container.
   - Enable **Child Control Width** and **Child Control Height**.
   - Disable **Child Force Expand Width** and **Child Force Expand Height**.
2. Add `Responsive Vertical Sizer` (menu: *Layout → Responsive Vertical Sizer*).
3. Set **Rows** to the number of items visible down.
4. Optionally enable **Lock Aspect** and set **Aspect Ratio** (width/height) to
   keep each item at a fixed ratio instead of filling the column width.
5. (Inside a ScrollRect) Set **Viewport Override** to the ScrollRect's Viewport.

**Runtime API:**

```csharp
var sizer = GetComponent<ResponsiveVerticalSizer>();

// Change row count at runtime
sizer.Rows = 5;

// Force a refit
sizer.Refit();

// Read the last computed item size
Vector2 itemSize = sizer.LastSize;

// React to size changes
sizer.OnSizeChanged += size => Debug.Log($"Item size: {size}");
```

**Example: vertical news feed with wide items**

Inspector:
- `Rows` = 4
- `Lock Aspect` = true
- `Aspect Ratio` = 2.0 (2× wider than tall)

Result: each item is `viewport.height / 4` tall, with width = height × 2.

---

### `ResponsiveSizeCalculator`

Computes a responsive item size without driving any layout group. Use this when
another system (e.g.
[kidzdev-unity-recyclable-scroll](https://github.com/knabsiraphop/kidzdev-unity-recyclable-scroll))
handles item placement itself and only needs the correct item size.

**Inspector setup:**

1. Add `Responsive Size Calculator` (menu: *Layout → Responsive Size Calculator*)
   to the container whose rect represents the viewport (or use `Viewport Override`).
2. Set **Rows** and **Columns** to match the consuming layout's visible count.
3. Set **Padding** and **Spacing** to match the consuming layout's settings so the
   size math is consistent.
4. Optionally set **Aspect Mode** and **Aspect Ratio**.
5. Subscribe to **`OnSizeChanged`** or read **`LastSize`** to consume the size.

**Runtime API:**

```csharp
var calc = GetComponent<ResponsiveSizeCalculator>();

// Configure to match the consuming layout
calc.Rows    = 3;
calc.Columns = 1;   // 1 item wide (vertical scroll)

// React to size changes — perfect for notifying a recyclable scroll view
calc.OnSizeChanged += newSize =>
{
    myRecyclableScroll.SetItemSize(newSize);
};

// Or poll the last known size
Vector2 itemSize = calc.LastSize;
```

**Example: recyclable scroll view integration**

```csharp
void Start()
{
    var calc = GetComponent<ResponsiveSizeCalculator>();
    // Initial size
    myScroll.SetItemSize(calc.LastSize);
    // Keep up with resizes
    calc.OnSizeChanged += myScroll.SetItemSize;
}
```

---

### `IViewportFitStrategy`

Every sizer implements this interface, which lets another system consume per-item
sizes without coupling to the concrete sizer type:

```csharp
public interface IViewportFitStrategy
{
    Vector2 CalculateItemSize(Rect viewport, int index);
}
```

The `index` parameter is accepted for variable-size consumers but is ignored by
all built-in sizers (they return the same size for every item). Use this seam when
integrating with a recyclable scroll view or any other system that drives sizing
through a strategy object.

```csharp
// Any sizer can be used as an IViewportFitStrategy
IViewportFitStrategy strategy = GetComponent<ResponsiveGridSizer>();

// Called by the scroll view per item
Vector2 size = strategy.CalculateItemSize(viewportRect, itemIndex);
```

---

## Use inside a ScrollRect

This is the most common setup. Place the sizer on the ScrollRect **Content**
(together with its layout group and a `ContentSizeFitter`) and set **Viewport
Override** to the ScrollRect's **Viewport** child:

```
ScrollRect
├── Viewport  ← assign this as Viewport Override
│   └── Content  ← add the sizer and layout group here, plus ContentSizeFitter
│       ├── Item 0
│       ├── Item 1
│       └── ...
```

Why this works: the sizer measures the stable *Viewport* rect rather than its own
*Content* rect, so there is no feedback loop between `ContentSizeFitter` growing
Content and the sizer reacting to that growth. Items size to the screen, and
Content grows as tall (or wide) as needed for all items.

The bundled Demo sample shows this setup for all three layout-group sizers.

---

## `OnSizeChanged` event

All sizers fire `OnSizeChanged` whenever the computed size changes:

```csharp
sizer.OnSizeChanged += newSize =>
{
    // Called once per frame at most, even if many layout callbacks fire
    Debug.Log($"New item size: {newSize}");
};
```

The event fires from both the automatic coalesced re-fit path and from explicit
`Refit()` calls. Use it instead of polling `LastSize` in `Update`.

---

## `AspectMode` enum

```csharp
namespace KidzDev.Unity.ResponsiveFit
{
    public enum AspectMode
    {
        None,               // width and height computed independently
        WidthControlsHeight, // height = width / aspectRatio
        HeightControlsWidth, // width  = height * aspectRatio
    }
}
```

Used by `ResponsiveGridSizer` and `ResponsiveSizeCalculator`. The horizontal and
vertical sizers use the simpler `lockAspect` bool instead (they only ever lock the
cross axis from the main axis).

> **v2.0.0 breaking change:** `AspectMode` was previously declared as
> `ResponsiveGridSizer.AspectMode`. It is now a standalone top-level type. Rename
> any qualified references.

---

## Refit coalescing

A single layout pass can fire `OnRectTransformDimensionsChange` several times, and
adding or removing children fires `OnTransformChildrenChanged`. Rather than running
a refit per callback, the sizers queue a single re-fit through
`Canvas.willRenderCanvases` so the whole burst collapses into one per frame. The
subscription is held only while a re-fit is pending, so an idle sizer costs nothing
per frame.

---

## Sample

Import the **Demo** sample from the Package Manager — a scene with all three
layout-group sizers inside ScrollRects + ContentSizeFitter, demonstrating the
Viewport Override setup.

---

## License

MIT — see [LICENSE.md](LICENSE.md).

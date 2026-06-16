# KidzDev Unity Responsive Fit

Responsive item sizing for Unity uGUI. Sizes the items of a layout group so a
fixed number fit the available **RectTransform viewport**, and re-fits
automatically when that viewport changes (resize, rotation, safe-area panel).

## Install

Add via Package Manager → *Add package from git URL*, or edit
`Packages/manifest.json`:

```
https://github.com/knabsiraphop/kidzdev-unity-responsive-fit.git#v1.0.0
```

## Components

Add the sizer that matches your layout group — each counts items along its axis
and re-fits on its own:

| Component | Layout group | You set | Item size |
| --- | --- | --- | --- |
| `ResponsiveGridSizer` | `GridLayoutGroup` | `Columns` × `Rows` | `cellSize` so that many cells fit the viewport |
| `ResponsiveHorizontalSizer` | `HorizontalLayoutGroup` | `Columns` | width = viewport ÷ columns; height fills the row |
| `ResponsiveVerticalSizer` | `VerticalLayoutGroup` | `Rows` | height = viewport ÷ rows; width fills the column |

- **Aspect (optional)** — the grid's `AspectMode` derives one cell dimension from
  the other; the horizontal/vertical sizers can lock the item to a fixed
  `width:height` instead of filling the cross axis.
- **Viewport Override** — when the sizer sits on oversized scroll *Content*, point
  it at the *Viewport* RectTransform so it measures the visible area.

## Use inside a ScrollRect

Put the sizer on the ScrollRect **Content** (alongside its layout group and a
`ContentSizeFitter`) and set **Viewport Override** to the ScrollRect's Viewport.
The sizer measures the stable viewport while `ContentSizeFitter` grows Content to
fit all items — so the list sizes to the screen and still scrolls, with no
feedback loop. The bundled demo scene shows this for all three sizers.

## Use as a size provider

Every sizer implements `IViewportFitStrategy`:

```
Vector2 CalculateItemSize(Rect viewport, int index)
```

so another system — e.g.
[kidzdev-unity-recyclable-scroll](https://github.com/knabsiraphop/kidzdev-unity-recyclable-scroll)
— can read per-item sizes for a recycled list without coupling the two packages.

## Usage

1. Add a `GridLayoutGroup` / `Horizontal`- / `VerticalLayoutGroup` to your
   container (for the H/V groups, enable *Child Control Width/Height* and disable
   *Child Force Expand*).
2. Add the matching `Responsive…Sizer` and set its `Columns` / `Rows`.
3. It re-fits automatically on dimension changes; call `Refit()` to force one.

## Sample

Import the **Demo** sample from the Package Manager — a scene with all three
sizers inside ScrollRects + ContentSizeFitter.

## License

MIT — see [LICENSE.md](LICENSE.md).

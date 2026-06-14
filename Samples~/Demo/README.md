# Responsive Fit — Demo

A small grid of placeholder cells that resizes to fit the screen.

> ⚠️ The sizing engine is still in progress (**Phase 1**). The sizer's
> `Recalculate` / `Apply` methods are stubs, so this demo shows the intended
> *wiring* — the refit cycle is logged rather than reflowing real cells.

## Fit modes

`ResponsiveItemSizer` exposes a `FitMode` that decides how an item's size is
derived from the viewport:

- **ItemsPerView** — divide the viewport by a target visible-item count, so
  exactly *N* items span the screen (e.g. `itemsPerView = 3.5` to hint the next
  row/column peeks in).
- **AspectRatio** — size each item to a fixed `width:height` ratio, deriving the
  free dimension from the constrained one.
- **FixedColumns** — lay out a set number of `columns` across the viewport and
  size cells to fill the width (height derived or square).

Toggle **Respect Safe Area** to inset the layout away from notches and rounded
corners.

## Setup

1. Add a **ResponsiveItemSizer** to the RectTransform that owns your grid
   (typically the one with a `GridLayoutGroup`).
2. Pick a **Fit Mode** and fill in its field (`itemsPerView`, `aspectRatio`, or
   `columns`).
3. Add **ResponsiveFitDemo** to a GameObject, assign the **Sizer** reference,
   and (optionally) a few placeholder **Cells**.
4. Enter Play mode and resize the Game view — the demo re-fits on every screen
   size change.

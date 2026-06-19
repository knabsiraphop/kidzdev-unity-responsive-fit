# Changelog

All notable changes to this package are documented here.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2026-06-20

### Added

- `ResponsiveSizeCalculator` — a new `ResponsiveItemSizer` subclass that computes
  item sizes for a fixed `Rows` × `Columns` fit without driving any layout group.
  Designed for recyclable scroll views and other systems that manage item placement
  themselves. Configure `padding` and `spacing` to match the consuming layout so the
  size math stays consistent; subscribe to `OnSizeChanged` to react to viewport
  resizes.
- `OnSizeChanged` event on `ResponsiveItemSizer` — raised whenever the computed size
  changes, either from a coalesced viewport resize or an explicit `Refit()` call.
  Allows external systems to react to size changes without polling `LastSize`.

### Changed

- **Breaking:** `AspectMode` is now a top-level enum in the
  `KidzDev.Unity.ResponsiveFit` namespace rather than a nested type inside
  `ResponsiveGridSizer`. Any code referencing `ResponsiveGridSizer.AspectMode`,
  `ResponsiveGridSizer.AspectMode.None`, `.WidthControlsHeight`, or
  `.HeightControlsWidth` must be updated to use the top-level `AspectMode` enum
  directly (e.g. `AspectMode.WidthControlsHeight`). The enum values and their
  semantics are unchanged.

## [1.0.0] - 2026-06-16

First stable release. A performance and correctness pass over the automatic
re-fit path; no public API changes from `0.2.0`.

### Changed

- Automatic re-fits — `OnRectTransformDimensionsChange`, and now
  `OnTransformChildrenChanged` — are coalesced to a single re-fit per frame via a
  lazy `Canvas.willRenderCanvases` subscription, so the burst of callbacks a single
  layout pass emits no longer drives one re-fit each. The subscription is held only
  while a re-fit is pending, so an idle sizer costs nothing per frame.
- A size + child-count guard skips the per-child sizing loop in
  `ResponsiveHorizontalSizer` / `ResponsiveVerticalSizer` when neither the computed
  size nor the child count changed (the grid sizer already short-circuited on
  `cellSize`).

### Fixed

- Children added to or removed from a fixed-size container (one without a
  `ContentSizeFitter`, whose rect never changes) are now sized automatically via
  `OnTransformChildrenChanged`, instead of staying unsized until the next manual
  `Refit()`.

## [0.2.0] - 2026-06-16

Replaces the `0.1.0` scaffold with a working, RectTransform-based sizing system.
Breaking change throughout.

### Added

- `ResponsiveGridSizer` — drives a `GridLayoutGroup`, sizing `cellSize` so a fixed
  number of `Columns` × `Rows` fit the viewport, with an optional `AspectMode`
  (none / width-controls-height / height-controls-width).
- `ResponsiveHorizontalSizer` / `ResponsiveVerticalSizer` — drive a
  `Horizontal`/`VerticalLayoutGroup`, sizing each child so `Columns` (resp. `Rows`)
  fit the viewport; the cross axis fills the strip, or optionally locks to an
  aspect ratio.
- `viewportOverride` — measure a separate RectTransform (e.g. a ScrollRect
  Viewport) instead of the component's own rect, so the sizers work inside a
  `ScrollRect` + `ContentSizeFitter` without feedback loops.
- A demo scene (`Samples~/Demo/ResponsiveFitDemo.unity`) with all three sizers
  inside ScrollRects + ContentSizeFitter.

### Changed

- **Breaking:** Renamed `IScreenFitStrategy` → `IViewportFitStrategy`; sizing fits a
  UI `RectTransform` viewport rather than the device screen.
- **Breaking:** `ResponsiveItemSizer` is now an abstract base (measurement,
  lifecycle, and the `IViewportFitStrategy` seam) — use one of the three concrete
  sizers above.

### Removed

- **Breaking:** The `FitMode` system (`ItemsPerView`, `AspectRatio`,
  `FixedColumns`) and the `respectSafeArea` option. Counts are explicit
  (`Columns` / `Rows`); handle safe area with a dedicated safe-area panel above the
  sizer.
- The self-building `ResponsiveFitDemo.cs` sample script, replaced by the demo
  scene.

## [0.1.0] - 2026-06-14

### Added

- Initial scaffold: `ResponsiveItemSizer` and `IScreenFitStrategy` (compiling
  stubs), asmdef, and a Demo sample.

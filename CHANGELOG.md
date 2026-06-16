# Changelog

All notable changes to this package are documented here.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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

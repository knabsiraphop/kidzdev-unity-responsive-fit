# KidzDev Responsive Fit

Responsive item sizing for Unity uGUI. Fits scroll and grid items to any screen
using one of several strategies — items-per-view, aspect ratio, or a fixed
column count — with optional safe-area insets.

> ⚠️ **Work in progress.** The package currently ships a compiling scaffold
> (Phase 1 in progress). The sizing methods are stubs — see the roadmap below.

## Install

Add via Package Manager → *Add package from git URL*, or edit
`Packages/manifest.json`:

```
https://github.com/knabsiraphop/kidzdev-responsive-fit.git#v0.1.0
```

## Features

- **Items-per-view** — size items so exactly *N* span the viewport.
- **Aspect ratio** — lock each item to a fixed width:height ratio.
- **Fixed columns** — lay out a set number of columns across the screen.
- **Safe area** — optionally inset the layout for notches and rounded corners.
- Re-fits automatically when the RectTransform's dimensions change.

## Use it standalone or as a size provider

`ResponsiveItemSizer` drives a `GridLayoutGroup` (or item rects) directly, so it
works **standalone** on any uGUI grid. It also implements the
`IScreenFitStrategy` seam, so it can feed
[kidzdev-recyclable-scroll](https://github.com/knabsiraphop/kidzdev-recyclable-scroll)
as a per-item **size provider** — letting a recycled list size its rows to the
screen without coupling the two packages.

## Roadmap

- **Phase 1** — `ItemsPerView` mode: divide the viewport by a target item count.
- **Phase 2** — `AspectRatio` mode: fixed width:height per item.
- **Phase 3** — `FixedColumns` mode: N columns across, derived height.
- **Phase 4** — safe-area insets honoured across every mode.

## Usage

1. Add a `ResponsiveItemSizer` to the RectTransform that owns your grid.
2. Choose a `FitMode` and set its field (`itemsPerView`, `aspectRatio`, or
   `columns`); toggle `respectSafeArea` as needed.
3. Call `Recalculate()` then `Apply()` (the sizer also re-fits automatically on
   dimension changes).

## Sample

Import the **Demo** sample from the Package Manager to see a grid that resizes to
fit the screen.

## License

MIT — see [LICENSE.md](LICENSE.md).

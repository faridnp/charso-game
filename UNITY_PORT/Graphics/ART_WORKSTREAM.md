# Graphics and Presentation Workstream

Owner: project owner + ChatGPT.

## Owned paths
- `UnityProject/Assets/ParsehNo/Art/`
- `UnityProject/Assets/ParsehNo/Presentation/`
- Visual test scenes and art showcase scenes.

## Responsibilities
- 3D buildings, environment, terrain, mountains, roads, vegetation, props and VFX.
- Materials, shaders, texture budgets, lighting, shadows and post-processing.
- RTS camera, zoom, pan, rotation and desktop/Android input adapters.
- Visual placement preview, building selection presentation, animations and LOD/instancing.
- Persian RTL HUD, build palette, building inspection UI, menus and responsive layout.
- Visual profiling on desktop Web and physical Android hardware.

## Rules
- Do not hardcode costs, taxes, income, treasury or simulation formulas in presentation code.
- Request state and commands through `ParsehNo.Contracts` only.
- No mobile-browser UI or Web Mobile build is part of the target.
- Preserve the empty new-city start. Art showcase scenes are separate and do not pre-place a hero palace in a new save.
- Every new asset needs provenance/license notes, source file or generation script, triangle/texture budget and a manifest update.

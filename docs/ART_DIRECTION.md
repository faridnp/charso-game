# Charso — Visual Direction & Environment Production Guide

## Visual identity

Charso is a warm, crafted Iranian heritage-restoration adventure. The world uses stylized realism: readable silhouettes, believable materials, restrained ornament, soft sun, and carefully composed color. Avoid generic fantasy-medieval language, European timber houses, snow villages, or random palace mashups.

### Core palette
- Warm plaster and lime: `#E8D4AE`
- Adobe and sandstone: `#B8946B`
- Deep glazed blue: `#22577A`
- Turquoise tile accent: `#43839A`
- Garden cypress: `#385D3B`
- Water: `#236375`
- Aged wood: `#523824`
- Brass and warm light: `#DCA64B`

Use blue/turquoise as architectural accents, not as a coating over every surface. Keep UI surfaces light, quiet, and high contrast.

## Mehrgan hub composition

The fictional city is a game-world hub, not a claim about a real historical city. The first blockout contains:
1. Central bazaar square and fountain.
2. Covered bazaar lane with shop bays and awnings.
3. Chahar-bagh garden with crossing water channels, flower beds, and pergola.
4. Residential quarters with courtyards, flat roofs, windcatcher-inspired silhouettes, and selected domed roof forms.
5. Civic courtyard with an iwan-inspired focal wall, arcade piers, dome and minaret silhouettes.
6. Restoration workshop and material yard.
7. Old-city wall and gate.
8. One separate landmark plot with four visibly distinct restoration states.

These are broad architectural vocabulary cues only. Do not label the placeholder buildings as a reconstruction of a specific site.

## Landmark visual progression

Each restoration step must alter the same landmark in a visible, coherent order:
- Stage 0: neglected and damaged surfaces, temporary bracing, incomplete garden.
- Stage 1: structural stabilization and safe access.
- Stage 2: repaired plaster, reviewed tile palette, restored entrance and roof silhouette.
- Stage 3: renewed courtyard, water, planting, and lighting.

The art team must keep a before/after comparison and an asset provenance record for every final stage.

## Asset standards

- Prefer original modular meshes and textures authored for Charso.
- Every external asset must have a recorded commercial-use license, attribution requirements, source URL, version, and allowed modifications.
- Never extract models, textures, audio, or UI from another game.
- A generated concept image is not a source of historical truth.
- A landmark model cannot pass blockout review until its research dossier and rights record are approved.
- Preserve modularity for LODs, batching, and staged restoration variants.

## Camera and composition

- Portrait-first Android presentation.
- Orthographic/isometric overview for the hub; keep the landmark as a readable focal point.
- Allow touch drag to explore and pinch to zoom; UI hit areas must not steal world gestures outside active controls.
- Keep the active objective visually legible behind UI panels.
- Avoid fast camera movement, motion blur, and excessive bloom.

## Initial mobile rendering budgets (targets, to be validated)

These are working budgets, not measured results:
- Hub visible set: 100k–180k triangles on the base tier.
- Main landmark: 25k–45k triangles before LODs.
- Texture atlas: 1024 px for most shared environment materials; 2048 px only for close-up hero assets.
- Materials: aim for 2–3 shared materials per modular building kit.
- Repeated vegetation/props: GPU instancing or combined meshes where supported.
- Real-time shadow casters: hero objects and selected large structures only.
- No real-time point/spot lights per lantern; use emissive materials and baked lighting.
- Pool effects and UI animation; no per-frame allocations.

Do not claim these budgets are met until Unity Profiler and device captures confirm them.

## QA image checklist

Capture at minimum:
- Full hub at portrait phone aspect.
- Full hub at tablet aspect.
- Close-up of each district.
- Landmark stages 0, 1, 2, and 3.
- Text overlays with Persian, Latin words, digits, punctuation, and mixed-direction strings.
- Low graphics setting and thermal/performance capture.

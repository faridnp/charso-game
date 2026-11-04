# Build Matrix

| Output | Target | Input | Artifact | QA focus |
|---|---|---|---|---|
| Android QA | Native Android | Touch | APK | memory, GPU, thermal, touch, lifecycle |
| Android Store | Native Android | Touch | AAB | signing, target API, device compatibility |
| Web Desktop | WebGL/WebAssembly | Mouse + keyboard | Build folder | browser compatibility, compression headers, memory, load time |

There is no mobile-browser build.

Both outputs share the same Unity scenes, contracts, domain logic and GLB assets. Platform-specific input and quality profiles must be selected at runtime or build time without forking gameplay rules.


Release signing: Android AAB generation requires a private custom keystore configured in Unity Player Settings. Never commit the keystore or passwords.

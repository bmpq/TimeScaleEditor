Simple utility window for Unity Editor. Quick access to common time controls, target frame rate, useful in debugging.
Features smooth time scale transition.

Accessed through: **Tools > Time Scale Editor**.

## Installation
### Method 1: Install via Unity Package Manager (Recommended):
1. Open UPM in Unity: **Window > Package Management > Package Manager**
2. Click **"+"** button at the top left
3. Select **"Add package from git URL..."** and paste following URL:
```
https://github.com/bmpq/TimeScaleEditor.git
```

### Method 2: Manual installation
1. Copy the `TimeScaleEditor.cs` file into your project's `Assets/Editor`
2. Keep the script in a folder named `Editor` for it to be treated as an editor script by Unity, it will avoid building the script into runtime

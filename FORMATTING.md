# How to format your code

NOTE: If you do not understand a particular naming convention, examples are given in the [Source code](#source-code) section.
This guide derives mostly from [this one](https://github.com/stillwwater/UnityStyleGuide)

#### Coding conventions serve the following purposes:

1. They create a consistent look to the code, so that readers can focus on content, not layout.
2. They enable readers to understand the code more quickly by making assumptions based on previous experience.
3. They facilitate copying, changing, and maintaining the code.
4. They demonstrate C# best practices.

## Asset Naming

There should be no spaces in filenames or directory names.

## Folders

`PascalCase`

Prefer a deep folder structure over having long asset names.

Directory names should be as concise as possible, prefer one or two words. If a directory name is too long, it probably makes sense to split it into sub directories.

Try to have only one file type per folder. Use `Textures/Trees`, `Models/Trees` and not `Trees/Textures`, `Trees/Models`. That way its easy to set up root directories for the different software involved, for example, Substance Painter would always be set to save to the Textures directory.

If your project contains multiple environments or art sets, use the asset type for the parent directory: `Trees/Jungle`, `Trees/City` not `Jungle/Trees`, `City/Trees`. Since it makes it easier to compare similar assets from different art sets to ensure continuity across art sets.

## Debug folders

`[PascalCase]`

This signifies that the folder only contains assets that are not ready for production. For example, having an `[Assets]` and `Assets` folder.

## Non-Code Assets
Use `tree_small` not `small_tree`. While the latter sound better in English, it is much more effective to group all tree objects together instead of all small objects.

`camelCase` where necessary. Use `weapon_miniGun` instead of `weapon_gun_mini`. Avoid this if possible, for example, `vehicles_fighterJet` should be `vehicles_jet_fighter` if you plan to have multiple types of jets.

Prefer using descriptive suffixes instead of iterative: `vehicle_truck_damaged` not `vehicle_truck_01`. If using numbers as a suffix, always use 2 digits. And do not use it as a versioning system! We already have `git`.

### Persistent/Important GameObjects

`_snake_case`

Use a leading underscore to make object instances that are not specific to the current scene stand out.

### Debug objects

`[SNAKE_CASE]`

Enclose objects that are only being used for debugging/testing and are not part of the release with brackets.

# Directory/File Structure

```
Root
+---Assets
+---Build
\---Tools           # Programs to aid development: compilers, asset managers etc.
```

## Assets

```
Assets
+---Art
|   +---Materials
|   +---Models      # FBX and BLEND files
|   +---Textures    # PNG files
+---Audio
|   +---Music
|   \---Sound       # Samples and sound effects
+---Code
|   +---Scripts     # C# scripts
|   \---Shaders     # Shader files and shader graphs
+---Docs            # Wiki, concept art, marketing material
+---Level           # Anything related to game design in Unity
|   +---Prefabs
|   +---Scenes
|   \---UI
\---Resources       # Configuration files, localization text and other user files.
```

## Scripts

Use namespaces that match your directory structure.

A Framework directory is great for having code that can be reused across projects.

The Scripts folder varies depending on the project, however, `Environment`, `Framework`, `Tools` and `UI` should be consistent across projects.

```
Scripts
+---Environment
+---Framework
+---NPC
+---Player
+---Tools
\---UI
```

## Models

Separate files from the modelling program and ready to use, exported models.

```
Models
+---Blend
\---FBX
```

# Workflow

## Models

File extension: `FBX`

Even though Unity supports Blender files by default, it is better to keep what is being worked on and what is a complete, exported model separate. This is also a must when using other software, such as Substance for texturing.

Use `Y up`, `-Z forward` and `uniform scale` when exporting.

## Textures

File extension: `PNG`, `TIFF` or `HDR`

Choose either a `Specularity/Glossiness` or `Roughness/Metallic` workflow. This depends on the software being used and what your artists are more comfortable with. Specularity maps have the advantage of being having the possibility to be RGB maps instead of grayscale (useful for tinted metals), apart from that there is little difference between the result from either workflow.

### Texture Suffixes

Suffix | Texture
:------|:-----------------
`_AL`  | Albedo
`_SP`  | Specular
`_R`   | Roughness
`_MT`  | Metallic
`_GL`  | Glossiness
`_N`   | Normal
`_H`   | Height
`_DP`  | Displacement
`_EM`  | Emission
`_AO`  | Ambient Occlusion
`_M`   | Mask

### RGB Masks

It is good practice to use a single texture to combine black and white masks in a single texture split by each RGB channel. Using this, most textures should have:

```
texture_AL.png  # Albedo
texture_N.png   # Normal Map
texture_M.png   # Mask
```

Channel | Spec/Gloss        | Rough/Metal
:-------|:------------------|:-----------
R       | Specularity       | Roughness
G       | Glossiness        | Metallic
B       | Ambient Occlusion | Ambient Occlusion

#### The blue channel can vary depending on the type of material:

 - For character materials use the `B` channel for *subsurface opacity/strength*
 - For anisotropic materials use the `B` channel for the *anisotropic direction map*

## Configuration Files

File extension: `INI`

Fast and easy to parse, clean and easy to tweak.

`XML`, `JSON`, and `YAML` are also good alternatives, pick one and be consistent.

Use binary file formats for files that should not be changed by the player. For multiplayer games store configuration data on a secure server.

## Localization

File extension: `CSV`

Widely used by localization software, makes it trivial to edit strings using spreadsheets.

## Audio

File extension: `WAV` while mixing, `OGG` in game.

Preload small sound clips to memory, load on the fly for longer music and less frequent ambient noise files.

# Source Code

This is (mostly) following the C# formatting convention. 

##### 1. Use pascal casing ("PascalCasing") when naming a `class`, `record`, or `struct`.

Example:
```csharp
// Example class
public class DataService
{
}
// Example record
public record PhysicalAddress(
    string Street,
    string City,
    string StateOrProvince,
    string ZipCode);
// Example struct
public struct ValueCoordinate
{
}
```

##### 2. When naming an `interface`, use pascal casing in addition to prefixing the name with an I. This clearly indicates to developers that it's an `interface`.

Example: 
```csharp
public interface IWorkerQueue
{
}
```
##### 3. When naming `public` members of types, such as fields, properties, events, methods, and local functions, use camel casing.

NOTE: In this formatting style, we use `camelCasing` for record and struct parameters too. This deviates from official C# convention

Example:
```csharp
public class ExampleEvents
{
    // A public field, these should be used sparingly
    public bool isValid;

    // An init-only property
    public IWorkerQueue workerQueue { get; init; }

    // An event
    public event Action eventProcessing;

    // Method
    public void startEventProcessing()
    {
        // Local function
        static int countQueueItems() => workerQueue.Count;
        // ...
    }
}
```

##### 4. Use camel casing ("camelCasing") when naming `private` or `internal` fields, and prefix them with `_`.

Example: 
```csharp
public class DataService
{
    private IWorkerQueue _workerQueue;
}
```
##### 5. When working with `static` fields that are `private` or `internal`, use the `s_` prefix and for thread static use `t_`.

Example:
```csharp
public class DataService
{
    private static IWorkerQueue s_workerQueue;

    [ThreadStatic]
    private static TimeSpan t_timeSpan;
}
```

##### 6. When writing method parameters, use camel casing.

Example:
```csharp
public T SomeMethod<T>(int someNumber, bool isValid)
{
}
```

##### 7. Layout conventions

Good layout uses formatting to emphasize the structure of your code and to make the code easier to read. Use the default Code Editor settings (smart indenting, four-character indents, tabs saved as spaces). For more information, see Options, Text Editor, C#, Formatting.

    - Write only one statement per line.    

    - Write only one declaration per line.

    - If continuation lines are not indented automatically, indent them one tab stop (four spaces).

    - Add at least one blank line between method definitions and property definitions.

    - Use parentheses to make clauses in an expression apparent, as shown in the following code.
    
```csharp
    if ((val1 > val2) && (val1 > val3))
{
    // Take appropriate action.
}
```

# Be Consistent

> The point of having style guidelines is to have a common vocabulary of coding so people can concentrate on what you're saying rather than on how you're saying it. We present global style rules here so people know the vocabulary, but local style is also important. If code you add to a file looks drastically different from the existing code around it, it throws readers out of their rhythm when they go to read it. Avoid this.

-- [Google C++ Style Guide](https://google.github.io/styleguide/cppguide.html)

---
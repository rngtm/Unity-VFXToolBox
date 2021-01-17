# Unity-VFXToolBox
<b>Unity-VFXToolBox</b> has a lot of tools for ParticleSystem.<br>
・Create Empty ParticleSystem<br>
・Shader Preset Tools (Custom Data & Custom Vertex Streams)<br>
・etc<br><br>

# Requirement
- Unity 2020.2.0 or higher
- Universal RP 10.2.2 or higher

<br>

## Git Path (Unity Package Manager)
> https://github.com/rngtm/Unity-VFXToolBox.git?path=Assets/VfxToolBox

<br>

## Usage (Shader Preset)
Shader Preset save ParticleSystem CustomData and VertexStreams in association with Shader.<br>
<br>

### STEP 1 : Create Preset
Window -> VFX ToolBox -> Material Preset Generator<br>
<img src = "Demo/images/2_open_preset_generator.png"><br>

Use Meterial Preset Generator to create <i>ShaderPresetData</i>.<br>
<img src = "Demo/images/2_preset_generator.png"><br>

Edit CustomData and Vertex Streams in ShaderPresetData.<br>
<img src = "Demo/images/2_edit_preset.png"><br><br>


### STEP 2 : Register Preset
Register <i>ShaderPresetData</i> to <i>ShaderPresetDataBase</i>.<br>
<img src = "Demo/images/3_register_preset.png"><br><br>

### STEP 3 : Apply Preset

Window -> VFX ToolBox -> Material Preset Attacher<br>
<img src = "Demo/images/2_open_preset_attacher.png"><br>

Drag & Drop ParticleSystem to <i>Material Preset Attacher</i> window and click <i>apply</i> button.<br>
<img src = "Demo/images/3_apply_preset.png"><br><br>

### Result
ParticleSystem's Custom Vertex Streams and Custom Data will be overwrited.<br>
<img src = "Demo/images/3_apply_result.png"><br>
<br>

# VFX Samples

## Installation (UPM)
You can install VFX samples via Unity Package Manager.

<img src = "Demo/images/vfx_samples/01_sample.png" height = 360>

<br>
<br>

---


## Sample 001 (VFX Samples)
(TBW)

<br>

---

## Sample 002 (Aura VFX Samples)
(TBW)

<br>

---

## Sample 003 (Procedural Mesh Generator)
Sample 003 contains procedural mesh generator implemented in Unity C#. 
- Spiral Mesh Generator
- Disc Mesh Generator
- Cylinder Mesh Generator

## Disc Generator
You can generate disc mesh via *DiscMeshGenerator* component.
<img src = "Demo/images/vfx_samples/disc_generator_component.png">

## Disc Generator Samples
There are few disc mesh samples in *Sample003_ProceduralMesh/Sample003_Disc.unity*.
<img src = "Demo/images/vfx_samples/disc_generator_sample.jpg" height = 240>


<br>

---

## Spiral Mesh Generator
(TBW)

<br>

---


## Cylinder Mesh Generator
(TBW)

# GPU Instancing Methods

This project contains **3 different GPU instancing approaches** for efficiently rendering large numbers of objects in Unity.

## 📁 Contents

### 1. **GPU_Instancer** (Compute Shader Based)
The most advanced and performant method. Calculates transformation matrices on the GPU using Compute Shaders.

**Features:**
- GPU computation with Compute Shader
- Procedural rendering
- Random position, rotation, and scale support
- Optimized for large instance counts

**Requirements:**
- Compute Shader file
- Custom shader (GPUInstancingTest.shader)
- Prefab (with MeshFilter + MeshRenderer)

---

### 2. **CPUInstancer** (Batch Instancing)
Uses `DrawMeshInstanced` with matrix calculations performed on the CPU.

**Features:**
- CPU-based matrix calculation
- Rendering in batches of 1023
- Context menu for position and matrix generation
- Suitable for medium-scale projects

**Usage:**
- Use "Generate Positions" and "Prepare Matrices" buttons from the Inspector

---

### 3. **SimpleInstancing** (Basic Instancing)
The simplest and most straightforward implementation. Ideal for small-scale projects and learning purposes.

**Features:**
- Single `DrawMeshInstanced` call
- Simple position randomization
- Minimal code, maximum clarity

---

## 🎯 Which Method Should I Choose?

| Method | Instance Count | Performance | Complexity |
|--------|----------------|------------|-------------|
| **GPU_Instancer** | 10,000+ | ⭐⭐⭐⭐⭐ | High |
| **CPUInstancer** | 1,000-10,000 | ⭐⭐⭐⭐ | Medium |
| **SimpleInstancing** | <1,000 | ⭐⭐⭐ | Low |

## 🚀 Quick Start

1. Add the relevant script to an empty GameObject
2. Assign a prefab (must contain MeshFilter and MeshRenderer)
3. Set the instance count and area size
4. Press Play!

## 📝 Notes

- All methods use **GPU Instancing**, the difference is in the calculation and rendering approach
- Compute Shader method provides the highest performance
- Shader files must be included in the project


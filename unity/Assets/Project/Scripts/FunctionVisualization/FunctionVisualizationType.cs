using System;

namespace DRL
{
    [Serializable]
    public enum FunctionVisualizationType : byte
    {
        DrawMesh = 0,
        DrawMeshInstanced = 1,
        DrawMeshInstancedIndirect = 2
    }
}

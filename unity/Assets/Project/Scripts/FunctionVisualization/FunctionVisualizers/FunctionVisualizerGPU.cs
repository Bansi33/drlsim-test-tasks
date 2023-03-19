using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Specific implementation of the <see cref="FunctionVisualizerBase"/> that executes both 
    /// points calculation and rendering on the GPU.
    /// </summary>
    public class FunctionVisualizerGPU : FunctionVisualizerBase
    {
        [Header("References: ")]
        [SerializeField] private Material _material = null;
        [SerializeField] private ComputeShader _positionsGenerationComputeShader = null;

        private ComputeBuffer _drawingArgumentsBuffer = null;
        private ComputeBuffer _meshInstancesDataBuffer = null;
        private int _currentlyInitializedBuffersSize = 0;

        /// <inheritdoc/>
        public override void CleanUp()
        {
            CleanUpComputeBuffer(ref _drawingArgumentsBuffer);
            CleanUpComputeBuffer(ref _meshInstancesDataBuffer);
        }

        /// <inheritdoc/>
        public override FunctionVisualizationType GetVisualizationType()
        {
            return FunctionVisualizationType.DrawMeshInstancedIndirect;
        }

        /// <inheritdoc/>
        protected override void VisualizeFunction(FunctionVisualizationData functionVisualizationData, Mesh mesh)
        {
            InitializeMaxBuffers(functionVisualizationData, mesh);

            // Calculate the positions of the spheres on the GPU.
            DispatchPositionsGenerationComputeShader(functionVisualizationData);

            // Setting the data buffer to the material that will be displaying mesh instances.
            _material.SetBuffer("_Data", _meshInstancesDataBuffer);

            // Making sure that all meshes are visible.
            Bounds bounds = new Bounds(Vector3.zero, Vector3.one * functionVisualizationData.Radius.Value * 200);

            // Dispatching draw command to the GPU.
            Graphics.DrawMeshInstancedIndirect(mesh, 0, _material, bounds, _drawingArgumentsBuffer);
        }

        private void InitializeMaxBuffers(FunctionVisualizationData functionVisualizationData, Mesh mesh)
        {
            if(_drawingArgumentsBuffer != null && _meshInstancesDataBuffer != null && _numberOfInstancesToDraw == _currentlyInitializedBuffersSize)
            {
                // Already initialized and the instances count hasn't changed.
                return;
            }

            CleanUp();

            uint[] drawingArguments = new uint[]
            {
                (uint)mesh.GetIndexCount(0),
                (uint)_numberOfInstancesToDraw,
                (uint)mesh.GetIndexStart(0),
                (uint)mesh.GetBaseVertex(0),
                (uint)0
            };

            _drawingArgumentsBuffer = new ComputeBuffer(
                    count: 1,
                    stride: drawingArguments.Length * sizeof(uint),
                    type: ComputeBufferType.IndirectArguments);
            _drawingArgumentsBuffer.SetData(drawingArguments);

            _meshInstancesDataBuffer = new ComputeBuffer(count: _numberOfInstancesToDraw, stride: sizeof(float) * 4);
            _currentlyInitializedBuffersSize = _numberOfInstancesToDraw;
        }

        private void DispatchPositionsGenerationComputeShader(FunctionVisualizationData functionVisualizationData)
        {
            int mainKernelID = _positionsGenerationComputeShader.FindKernel("CSMain");
            _positionsGenerationComputeShader.GetKernelThreadGroupSizes(mainKernelID, out uint threadGroupSizeX, out _, out _);
            int threadGroupsX = Mathf.CeilToInt((float)_numberOfInstancesToDraw / threadGroupSizeX);

            _positionsGenerationComputeShader.SetBuffer(mainKernelID, "_Data", _meshInstancesDataBuffer);
            _positionsGenerationComputeShader.SetVector("_PositionsGenerationData", new Vector4(
                x: _startOffsetAngle,
                y: functionVisualizationData.ConstA.Value,
                z: functionVisualizationData.ConstB.Value,
                w: functionVisualizationData.Radius.Value));
            _positionsGenerationComputeShader.SetFloat("_Size", functionVisualizationData.Size.Value);
            _positionsGenerationComputeShader.SetInt("_TotalInstancesCount", _numberOfInstancesToDraw);
            _positionsGenerationComputeShader.Dispatch(mainKernelID, threadGroupsX, 1, 1);
        }

        private void CleanUpComputeBuffer(ref ComputeBuffer computeBuffer)
        {
            if (computeBuffer != null)
            {
                computeBuffer.Dispose();
                computeBuffer.Release();
                computeBuffer = null;
            }
        }
    }
}

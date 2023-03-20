using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Class that contains reference to all currently supported function visualizers and triggers
    /// the function visualization process on the desired one.
    /// </summary>
    public class FunctionVisualization : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] private ColorPattern _colorPattern = null;
        [SerializeField] private Mesh _mesh = null;
        [SerializeField] private Material _material = null;
        [SerializeField] private ComputeShader _positionsGenerationComputeShader = null;

        /// <summary>
        /// Starting angle offset that needs to be applied to the function plotting. Used to simulate
        /// the movement of the plotted points over time.
        /// </summary>
        private float _startOffsetAngle = 0f;

        private ComputeBuffer _drawingArgumentsBuffer = null;
        private ComputeBuffer _meshInstancesDataBuffer = null;
        private int _numberOfInstancesToDraw = 0;
        private int _currentlyInitializedBuffersSize = 0;

        private void OnDisable()
        {
            CleanUp();
        }

        /// <summary>
        /// The method visualizes the polar coordinates function by spawning the <see cref="_mesh"/> asset along the discrete sample
        /// points based on the provided <paramref name="functionVisualizationData"/>.
        /// </summary>
        /// <param name="functionVisualizationData">Instance of the <see cref="FunctionVisualizationData"/> that holds the 
        /// parameters required for successfully plotting the function.</param>
        public void VisualizeData(FunctionVisualizationData functionVisualizationData)
        {
            if (functionVisualizationData == null)
            {
                Debug.LogError($"Can't visualize the function since the {nameof(FunctionVisualizationData)} is null.", gameObject);
                return;
            }

            _numberOfInstancesToDraw = Mathf.RoundToInt(functionVisualizationData.Instances.Value);
            if (_numberOfInstancesToDraw == 0)
            {
                return;
            }            

            VisualizeFunction(functionVisualizationData);
            _startOffsetAngle -= functionVisualizationData.Speed.Value * Time.deltaTime;
        }

        private void CleanUp()
        {
            CleanUpComputeBuffer(ref _drawingArgumentsBuffer);
            CleanUpComputeBuffer(ref _meshInstancesDataBuffer);
        }

        /// <inheritdoc/>
        private void VisualizeFunction(FunctionVisualizationData functionVisualizationData)
        {
            InitializeBuffers();

            // Calculate the positions of the spheres on the GPU.
            DispatchPositionsGenerationComputeShader(functionVisualizationData);

            // Setting the data buffer to the material that will be displaying mesh instances.
            _material.SetBuffer("_Data", _meshInstancesDataBuffer);

            // Making sure that all meshes are visible.
            Bounds bounds = new Bounds(Vector3.zero, Vector3.one * functionVisualizationData.Radius.Value * 200);

            // Dispatching draw command to the GPU.
            Graphics.DrawMeshInstancedIndirect(_mesh, 0, _material, bounds, _drawingArgumentsBuffer);
        }

        private void InitializeBuffers()
        {
            if (_drawingArgumentsBuffer != null && _meshInstancesDataBuffer != null && _numberOfInstancesToDraw == _currentlyInitializedBuffersSize)
            {
                // Already initialized and the instances count hasn't changed.
                return;
            }

            CleanUp();

            uint[] drawingArguments = new uint[]
            {
                (uint)_mesh.GetIndexCount(0),
                (uint)_numberOfInstancesToDraw,
                (uint)_mesh.GetIndexStart(0),
                (uint)_mesh.GetBaseVertex(0),
                (uint)0
            };

            _drawingArgumentsBuffer = new ComputeBuffer(
                    count: 1,
                    stride: drawingArguments.Length * sizeof(uint),
                    type: ComputeBufferType.IndirectArguments);
            _drawingArgumentsBuffer.SetData(drawingArguments);

            _meshInstancesDataBuffer = new ComputeBuffer(count: _numberOfInstancesToDraw, stride: sizeof(float) * 4 * 2);
            _currentlyInitializedBuffersSize = _numberOfInstancesToDraw;
        }

        private void DispatchPositionsGenerationComputeShader(FunctionVisualizationData functionVisualizationData)
        {
            int mainKernelID = _positionsGenerationComputeShader.FindKernel("CSMain");
            _positionsGenerationComputeShader.GetKernelThreadGroupSizes(mainKernelID, 
                out uint threadGroupSizeX, 
                out uint threadGroupSizeY,
                out uint threadGroupSizeZ);

            // [0, 63] representing the same range.
            int threadGroupsX = Mathf.CeilToInt((float)_numberOfInstancesToDraw / threadGroupSizeX);

            // [0, 63] representing multipliers of 64.
            int multiplier = Mathf.CeilToInt(_numberOfInstancesToDraw / 64.0f);
            int threadGroupsY = (int)(multiplier % threadGroupSizeY);

            // [0, 63] representing multipliers of 4096.
            multiplier = Mathf.FloorToInt(_numberOfInstancesToDraw / 4096.0f);
            int threadGroupsZ = (int)(multiplier % threadGroupSizeZ);


            _positionsGenerationComputeShader.SetBuffer(mainKernelID, "_Data", _meshInstancesDataBuffer);
            _positionsGenerationComputeShader.SetVector("_PositionsGenerationData", new Vector4(
                x: _startOffsetAngle,
                y: functionVisualizationData.ConstA.Value,
                z: functionVisualizationData.ConstB.Value,
                w: functionVisualizationData.Radius.Value));

            Vector4[] colorIDs = _colorPattern.ParseColorPattern(functionVisualizationData.Pattern);
            _positionsGenerationComputeShader.SetVector("_RepresentationData", new Vector4(
                x: functionVisualizationData.Size.Value,
                y: _numberOfInstancesToDraw,
                z: colorIDs.Length,
                w: 0));

            _positionsGenerationComputeShader.SetVectorArray("_ColorPatternData", colorIDs);

            // Dispatch compute shader that needs to calculate the positions.
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

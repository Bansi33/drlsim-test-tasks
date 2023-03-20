using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Class that visualizes the function based on the <see cref="FunctionVisualizationData"/> by 
    /// indirectly instancing <see cref="_mesh"/> on the GPU. The positions and colors are calculated
    /// via the compute shader and assigned to the buffer that is shared between the material and the 
    /// compute shader.
    /// </summary>
    public class FunctionVisualization : MonoBehaviour
    {
        [Header("References: ")]
        [Tooltip("Class that parses the user-provided color pattern.")]
        [SerializeField] private ColorPattern _colorPattern = null;
        [Tooltip("Mesh that will be instantiated along the function.")]
        [SerializeField] private Mesh _mesh = null;
        [Tooltip("Material that supports indirect instancing, that will be applied to the mesh instances.")]
        [SerializeField] private Material _material = null;
        [Tooltip("Compute shader used for calculating positions along the function.")]
        [SerializeField] private ComputeShader _positionsGenerationComputeShader = null;

        /// <summary>
        /// Starting angle offset that needs to be applied to the function plotting. Used to simulate
        /// the movement of the plotted points over time.
        /// </summary>
        private float _startOffsetAngle = 0f;

        /// <summary>
        /// Compute buffer holding the arguments indicating the number of instances that will be rendered,
        /// size and stride of the mesh, etc.
        /// </summary>
        private ComputeBuffer _drawingArgumentsBuffer = null;

        /// <summary>
        /// Compute buffer holding the positions and colors for every instance that will be spawned.
        /// </summary>
        private ComputeBuffer _meshInstancesDataBuffer = null;

        /// <summary>
        /// How many mesh instances will be indirectly instanced.
        /// </summary>
        private int _numberOfInstancesToDraw = 0;

        /// <summary>
        /// Current size of the initialized buffers, used to compare with the currently required size
        /// and potentially reinitialize the buffers if the values doesn't match.
        /// </summary>
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

            // Visualizing the function by instantiating meshes along the path.
            VisualizeFunction(functionVisualizationData);

            // Offset the starting angle to simulate the appearance of motion with instances.
            _startOffsetAngle -= functionVisualizationData.Speed.Value * Time.deltaTime;
        }

        /// <summary>
        /// Function cleans up the instantiated compute buffers.
        /// </summary>
        private void CleanUp()
        {
            CleanUpComputeBuffer(ref _drawingArgumentsBuffer);
            CleanUpComputeBuffer(ref _meshInstancesDataBuffer);
        }

        /// <inheritdoc/>
        private void VisualizeFunction(FunctionVisualizationData functionVisualizationData)
        {
            // Potentially initialize the compute buffers and update the data.
            InitializeBuffers();

            // Calculate the positions of the instances on the GPU.
            DispatchPositionsGenerationComputeShader(functionVisualizationData);

            // Setting the data buffer to the material that will be displaying mesh instances.
            _material.SetBuffer("_Data", _meshInstancesDataBuffer);

            // Making sure that all meshes are visible.
            Bounds bounds = new Bounds(Vector3.zero, Vector3.one * functionVisualizationData.Radius.Value);

            // Dispatching draw command to the GPU.
            Graphics.DrawMeshInstancedIndirect(_mesh, 0, _material, bounds, _drawingArgumentsBuffer);
        }

        /// <summary>
        /// Function initializes the compute buffers whenever the number of instances changes.
        /// </summary>
        private void InitializeBuffers()
        {
            if (_drawingArgumentsBuffer != null && _meshInstancesDataBuffer != null && _numberOfInstancesToDraw == _currentlyInitializedBuffersSize)
            {
                // Already initialized and the instances count hasn't changed.
                return;
            }

            CleanUp();

            // Initialize the drawing arguments that tell the GPU how many instances will be rendered,
            // what are the properties of the mesh that will be instantiated, etc.
            uint[] drawingArguments = new uint[]
            {
                (uint)_mesh.GetIndexCount(0),
                (uint)_numberOfInstancesToDraw,
                (uint)_mesh.GetIndexStart(0),
                (uint)_mesh.GetBaseVertex(0),
                (uint)0
            };

            // Assigning the drawing arguments to the compute buffer so that they would be readable to the GPU.
            _drawingArgumentsBuffer = new ComputeBuffer(
                    count: 1,
                    stride: drawingArguments.Length * sizeof(uint),
                    type: ComputeBufferType.IndirectArguments);
            _drawingArgumentsBuffer.SetData(drawingArguments);

            // Initialize the buffer that will store the positions and colors of every instance.
            // The size of the stride is determined by the total size of one instance, which contains two 
            // float4 vectors. One for position and one for the color. In total 8 float elements.
            _meshInstancesDataBuffer = new ComputeBuffer(count: _numberOfInstancesToDraw, stride: sizeof(float) * 4 * 2);
            _currentlyInitializedBuffersSize = _numberOfInstancesToDraw;
        }

        /// <summary>
        /// Function dispatches the compute shader that calculates the positions of the instances along the function
        /// and applies the correct color pattern on the GPU.
        /// </summary>
        /// <param name="functionVisualizationData">Visualization data containing properties affecting the visualization: 
        /// number of instances, instance size, color pattern, etc.</param>
        private void DispatchPositionsGenerationComputeShader(FunctionVisualizationData functionVisualizationData)
        {
            int mainKernelID = _positionsGenerationComputeShader.FindKernel("CSMain");
            _positionsGenerationComputeShader.GetKernelThreadGroupSizes(mainKernelID, 
                out uint threadGroupSizeX, out _, out _);

            // Calculate the required number of thread groups based on the size of one thread group in the compute shader.
            int threadGroupsX = Mathf.CeilToInt((float)_numberOfInstancesToDraw / threadGroupSizeX);

            // Assigning the buffer holding all the positions and colors for every instance, that the compute shader needs to fill.
            _positionsGenerationComputeShader.SetBuffer(mainKernelID, "_Data", _meshInstancesDataBuffer);

            // Assigning the information required for the positions calculation, all tightly packed into one 
            // vector so that there is as few data passing from the CPU to the GPU as possible.
            _positionsGenerationComputeShader.SetVector("_PositionsGenerationData", new Vector4(
                x: _startOffsetAngle,
                y: functionVisualizationData.ConstA.Value,
                z: functionVisualizationData.ConstB.Value,
                w: functionVisualizationData.Radius.Value));

            // Assigning the rest of the data to the GPU, also tightly packed.
            Vector4[] colorIDs = _colorPattern.ParseColorPattern(functionVisualizationData.Pattern);
            _positionsGenerationComputeShader.SetVector("_RepresentationData", new Vector4(
                x: functionVisualizationData.Size.Value,
                y: _numberOfInstancesToDraw,
                z: colorIDs.Length,
                w: 0));

            // Assigning the array containing the color pattern of the instances.
            _positionsGenerationComputeShader.SetVectorArray("_ColorPatternData", colorIDs);

            // Dispatch compute shader that needs to calculate the positions.
            _positionsGenerationComputeShader.Dispatch(mainKernelID, threadGroupsX, 1, 1);
        }

        /// <summary>
        /// Function releases and disposes the content of the provided <paramref name="computeBuffer"/>
        /// if it's not null.
        /// </summary>
        /// <param name="computeBuffer">Reference to the <see cref="ComputeBuffer"/> that needs to be disposed.</param>
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

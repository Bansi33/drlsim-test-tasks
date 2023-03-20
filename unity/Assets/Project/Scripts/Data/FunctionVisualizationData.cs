using System;

namespace DRL
{
    /// <summary>
    /// Data required for correctly visualizing the function: r = Sin(angle * A) + Cos(angle * B).
    /// Expanded with the <see cref="Pattern"/> specifying the color pattern that will be applied 
    /// to the instances along the function.
    /// </summary>
    [Serializable]
    public class FunctionVisualizationData
    {
        /// <summary>
        /// Number of instances that will be spawned along the function.
        /// </summary>
        public ConstrainedValue Instances;
        /// <summary>
        /// Movement speed of the instances along the function path.
        /// </summary>
        public ConstrainedValue Speed;
        /// <summary>
        /// Size of the instances.
        /// </summary>
        public ConstrainedValue Size;
        /// <summary>
        /// Radius of the simulation, applied to the calculated function positions.
        /// </summary>
        public ConstrainedValue Radius;
        /// <summary>
        /// Constant used in calculating the polar coordinates.
        /// </summary>
        public ConstrainedValue ConstA;
        /// <summary>
        /// Constant used in calculating the polar coordinates.
        /// </summary>
        public ConstrainedValue ConstB;
        /// <summary>
        /// Color pattern that will be applied to the spawned instances in form of
        /// "R|G|B|*", where "*" represents an invisible (empty) instance.
        /// </summary>
        public string Pattern;

        public FunctionVisualizationData()
        {
            Instances = new ConstrainedValue();
            Speed = new ConstrainedValue();
            Size = new ConstrainedValue();
            Radius = new ConstrainedValue();
            ConstA = new ConstrainedValue();
            ConstB = new ConstrainedValue();
            Pattern = string.Empty;
        }
    }
}
using System;

namespace DRL
{
    [Serializable]
    public class FunctionVisualizationData
    {
        public ConstrainedValue Instances;
        public ConstrainedValue Speed;
        public ConstrainedValue Size;
        public ConstrainedValue Radius;
        public ConstrainedValue ConstA;
        public ConstrainedValue ConstB;
        public string Pattern;
        public FunctionVisualizationType Mode;

        public FunctionVisualizationData()
        {
            Instances = new ConstrainedValue();
            Speed = new ConstrainedValue();
            Size = new ConstrainedValue();
            Radius = new ConstrainedValue();
            ConstA = new ConstrainedValue();
            ConstB = new ConstrainedValue();
            Pattern = string.Empty;
            Mode = FunctionVisualizationType.DrawMesh;
        }

#if UNITY_EDITOR
        public void ValidateData()
        {
            Instances?.ValidateData();
            Speed?.ValidateData();
            Size?.ValidateData();
            Radius?.ValidateData();
            ConstA?.ValidateData();
            ConstB?.ValidateData();
        }
#endif
    }
}
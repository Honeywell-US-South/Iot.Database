using Microsoft.ML.Data;

namespace Iot.Database.Vector
{
    public class TransformedVectorData : VectorData
    {
        [VectorType]
        public float[] Features { get; set; }
    }
}

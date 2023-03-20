using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Class that calculates the color pattern based on the provided input string.
    /// </summary>
    public class ColorPattern : MonoBehaviour
    {
        /// <summary>
        /// Max allowed pattern length, constrained by the compute buffer size.
        /// </summary>
        private const int MAX_PATTERN_LENGTH = 256;
        /// <summary>
        /// Default color vector that needs to be applied when no color pattern is specified (Red color).
        /// </summary>
        private readonly Vector4 DEFAULT_COLOR_VECTOR = new Vector4(1, 0, 0, 1);
        /// <summary>
        /// Default vectors for every supported color (red, green, blue and transparent).
        /// </summary>
        private readonly Vector4[] DEFAULT_COLOR_VECTORS = new Vector4[] {
            new Vector4(1, 0, 0, 1),
            new Vector4(0, 1, 0, 1),
            new Vector4(0, 0, 1, 1),
            new Vector4(0, 0, 0, 0)
        };

        /// <summary>
        /// Already parsed color pattern. Small optimization so that pattern doesn't have to 
        /// parsed every frame, only when changed.
        /// </summary>
        private string _parsedColorPattern = string.Empty;
        /// <summary>
        /// Array of color vectors that will be propagated to the GPU.
        /// </summary>
        private Vector4[] _colorIDs = new Vector4[MAX_PATTERN_LENGTH];

        /// <summary>
        /// Function parses the provided <paramref name="colorPattern"/> and returns an 
        /// array of vectors that represent color pattern in a GPU readable format.
        /// </summary>
        /// <param name="colorPattern">String representing the color pattern in format: 
        /// "R|G|B|*", where "*" represents an empty/transparent color.</param>
        /// <returns>Array of vectors that represent color pattern in a GPU readable format.</returns>
        public Vector4[] ParseColorPattern(string colorPattern)
        {
            if (string.IsNullOrEmpty(colorPattern))
            {
                // No pattern provided, return default color vectors array.
                ClearColorIDs();
                return _colorIDs;
            }

            if (_parsedColorPattern.Equals(colorPattern))
            {
                // Already parsed this pattern, no need to do it again.
                return _colorIDs;
            }

            // Construct the array of color vectors that will be provided to the GPU based
            // on the provided pattern. Each letter is examined and the correct vector is applied.
            // In case of a character that doesn't match the required ones, a random color vector
            // will be applied from the collection of supported color vectors.
            int colorPatternLength = colorPattern.Length;
            string colorPatternAllLowercase = colorPattern.ToLower();
            for (int i = 0; i < MAX_PATTERN_LENGTH; i++)
            {
                int letterIndex = i % colorPatternLength;
                char letter = colorPatternAllLowercase[letterIndex];
                Vector4 colorID;
                switch (letter)
                {
                    case 'r':
                        colorID = DEFAULT_COLOR_VECTORS[0];
                        break;
                    case 'g':
                        colorID = DEFAULT_COLOR_VECTORS[1];
                        break;
                    case 'b':
                        colorID = DEFAULT_COLOR_VECTORS[2];
                        break;
                    case '*':
                        colorID = DEFAULT_COLOR_VECTORS[3];
                        break;
                    default:
                        colorID = DEFAULT_COLOR_VECTORS[Random.Range(0, DEFAULT_COLOR_VECTORS.Length)];
                        break;
                }
                _colorIDs[i] = colorID;
            }

            _parsedColorPattern = colorPattern;
            return _colorIDs;
        }

        /// <summary>
        /// Function resets the color vector array to the default color vector value (Red).
        /// </summary>
        private void ClearColorIDs()
        {
            for(int i = 0; i < MAX_PATTERN_LENGTH; i++)
            {
                _colorIDs[i] = DEFAULT_COLOR_VECTOR;
            }
        }
    }
}

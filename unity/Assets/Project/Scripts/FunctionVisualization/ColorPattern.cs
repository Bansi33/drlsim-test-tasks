using UnityEngine;

namespace DRL
{
    public class ColorPattern : MonoBehaviour
    {
        private const int MAX_PATTERN_LENGTH = 256;
        private readonly Vector4 DEFAULT_COLOR_VECTOR = new Vector4(1, 0, 0, 1);
        private readonly Vector4[] DEFAULT_COLOR_VECTORS = new Vector4[] {
            new Vector4(1, 0, 0, 1),
            new Vector4(0, 1, 0, 1),
            new Vector4(0, 0, 1, 1),
            new Vector4(0, 0, 0, 0)
        };

        private string _parsedColorPattern = string.Empty;
        private Vector4[] _colorIDs = new Vector4[MAX_PATTERN_LENGTH];

        public Vector4[] ParseColorPattern(string colorPattern)
        {
            if (string.IsNullOrEmpty(colorPattern))
            {
                ClearColorIDs();
                return _colorIDs;
            }

            if (_parsedColorPattern.Equals(colorPattern))
            {
                // Already parsed this pattern, no need to do it again.
                return _colorIDs;
            }

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

        private void ClearColorIDs()
        {
            for(int i = 0; i < MAX_PATTERN_LENGTH; i++)
            {
                _colorIDs[i] = DEFAULT_COLOR_VECTOR;
            }
        }
    }
}

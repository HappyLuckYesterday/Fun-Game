using Rhisis.Core.Structures;
using System;
using System.IO;

namespace Rhisis.Game.IO.World
{
    /// <summary>
    /// FlyFF World File structure.
    /// </summary>
    public class WldFile : FileStream, IDisposable
    {
        private const int DefaultMPU = 4;
        private static readonly char[] SplitCharacters = new[] { ' ', '\t' };

        public WldFileInformations WorldInformations { get; private set; }

        /// <summary>
        /// Creates a new <see cref="WldFile"/> instance.
        /// </summary>
        /// <param name="filePath">Wld file data</param>
        public WldFile(string filePath)
            : base(filePath, FileMode.Open, FileAccess.Read)
        {
            Read();
        }

        /// <summary>
        /// Reads the content of the Wld file.
        /// </summary>
        private void Read()
        {
            var reader = new StreamReader(this);
            Vector3 size = null;
            var isIndoor = false;
            var canFly = false;
            var mpu = DefaultMPU;
            var revivalMapId = 0;
            var revivalKey = string.Empty;

            while (!reader.EndOfStream)
            {
                var lineContent = reader.ReadLine();
                if (lineContent is null)
                {
                    continue;
                }

                var line = lineContent.Trim().ToLower();

                if (string.IsNullOrEmpty(line) || line.StartsWith("//"))
                {
                    continue;
                }

                var lineArray = line.Split(SplitCharacters, StringSplitOptions.RemoveEmptyEntries);

                switch (lineArray[0].ToLower())
                {
                    case "size":
                        size = ReadSize(lineArray);
                        break;
                    case "indoor":
                        isIndoor = lineArray[1] == "1" ? true : false;
                        break;
                    case "fly":
                        canFly = lineArray[1] == "1" ? true : false;
                        break;
                    case "mpu":
                        mpu = int.Parse(lineArray[1]);
                        break;
                    case "revival":
                        revivalMapId = int.Parse(lineArray[1]);
                        revivalKey = lineArray[2].Trim('"');
                        break;
                }
            }

            if (size is null)
            {
                return;
            }

            WorldInformations = new WldFileInformations((int)size.X, (int)size.Z, mpu, isIndoor, canFly, revivalMapId, revivalKey);
        }

        /// <summary>
        /// Read the "size" field of the wld file.
        /// </summary>
        /// <param name="lineArray">Current line array</param>
        private Vector3 ReadSize(string[] lineArray)
        {
            var width = lineArray[1].Replace(",", string.Empty);
            var length = lineArray[2];

            return new Vector3(int.Parse(width), 0, int.Parse(length));
        }
    }
}

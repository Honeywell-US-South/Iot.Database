namespace Iot.Database.Video
{
    internal class VideoCollection : BaseDatabase
    {

        public VideoCollection(string path, string name, string password) : base (path, name, password)
        {

        }

        public static void ReconstructVideo(string inputFilePath, string outputFilePath)
        {
            using (var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
            using (var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[4096];
                byte[] previousFrame = null;

                while (inputStream.Position < inputStream.Length)
                {
                    var header = new byte[5];
                    inputStream.Read(header, 0, 5);

                    bool isFullFrame = header[0] == 1;
                    int frameLength = BitConverter.ToInt32(header, 1);

                    var frameData = new byte[frameLength];
                    inputStream.Read(frameData, 0, frameLength);

                    if (isFullFrame)
                    {
                        outputStream.Write(frameData, 0, frameData.Length);
                        previousFrame = frameData;
                    }
                    else
                    {
                        var fullFrame = new byte[previousFrame.Length];
                        Array.Copy(previousFrame, fullFrame, previousFrame.Length);

                        for (int i = 0; i < frameData.Length; i += 5)
                        {
                            int index = BitConverter.ToInt32(frameData, i);
                            byte value = frameData[i + 4];
                            fullFrame[index] = value;
                        }

                        outputStream.Write(fullFrame, 0, fullFrame.Length);
                        previousFrame = fullFrame;
                    }

                    outputStream.Flush();
                }
            }
        }

        protected override void InitializeDatabase()
        {
            throw new NotImplementedException();
        }

        protected override void PerformBackgroundWork(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

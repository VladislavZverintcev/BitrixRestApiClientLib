using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class BaseFile : AttachObj
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "SIZE")]
        public int Size { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public BaseFile() : base()
        {

        }

        public BaseFile(File file, int size)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            Name = file.Name;
            DownloadUrl = file.DownloadUrl;
            Size = size;
        }
        #endregion Public

        #endregion Constructors
    }
}
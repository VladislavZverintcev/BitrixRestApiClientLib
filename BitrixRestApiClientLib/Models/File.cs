using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class File : AttachObj
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "ID")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "CODE")]
        public string? Code { get; set; }

        [JsonProperty(PropertyName = "STORAGE_ID")]
        public int StorageId { get; set; }

        [JsonProperty(PropertyName = "TYPE")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "PARENT_ID")]
        public int ParentId { get; set; }

        [JsonProperty(PropertyName = "DELETED_TYPE")]
        public int DeletedType { get; set; }

        [JsonProperty(PropertyName = "CREATE_TIME")]
        public string? CreateTime { get; set; }

        [JsonProperty(PropertyName = "UPDATE_TIME")]
        public string? UpdateTime { get; set; }

        [JsonProperty(PropertyName = "DELETE_TIME")]
        public string? DeleteTime { get; set; }

        [JsonProperty(PropertyName = "CREATED_BY")]
        public int CreatedBy { get; set; }

        [JsonProperty(PropertyName = "UPDATED_BY")]
        public int UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "DELETED_BY")]
        public int? DeletedBy { get; set; }

        [JsonProperty(PropertyName = "DOWNLOAD_URL")]
        public new string DownloadUrl { get; set; }

        [JsonProperty(PropertyName = "DETAIL_URL")]
        public string DetailUrl { get; set; }
        #endregion Public

        #endregion Propertis

        #region Constructors

        #region Public
        public File()
        {
            Name = string.Empty;
            Type = string.Empty;
            DownloadUrl = string.Empty;
            DetailUrl = string.Empty;
        }
        #endregion Public

        #endregion Constructors
    }
}
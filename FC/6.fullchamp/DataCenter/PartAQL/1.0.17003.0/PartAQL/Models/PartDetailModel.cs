namespace PartAQL.Models
{
    /// <summary>
    /// 物料的細節
    /// </summary>
    public class PartDetailModel
    {
        public PartDetailModel() { }

        public PartDetailModel(PartDetailModel e)
        {
            PartID = e.PartID;

            PartNo = e.PartNo;

            Version = e.Version;
        }

        public string PartID { get; set; } = string.Empty;

        public string PartNo { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;
    }
}

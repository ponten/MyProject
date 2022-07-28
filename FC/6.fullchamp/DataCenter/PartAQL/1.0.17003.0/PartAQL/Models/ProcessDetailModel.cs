namespace PartAQL.Models
{
    /// <summary>
    /// 製程細節
    /// </summary>
    public class ProcessDetailModel
    {
        public ProcessDetailModel() { }

        public ProcessDetailModel(ProcessDetailModel e)
        {
            RouteID = e.RouteID;

            RouteName = e.RouteName;

            ProcessID = e.ProcessID;

            ProcessCode = e.ProcessCode;

            ProcessName = e.ProcessName;

            NodeID = e.NodeID;
        }

        public string RouteID { get; set; } = string.Empty;

        public string RouteName { get; set; } = string.Empty;

        public string ProcessID { get; set; } = string.Empty;

        public string ProcessCode { get; set; } = string.Empty;

        public string ProcessName { get; set; } = string.Empty;

        public string NodeID { get; set; } = string.Empty;
    }
}

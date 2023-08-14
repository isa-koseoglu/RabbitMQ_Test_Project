
namespace RabbitMQ.Common.Model
{
    public class GetVersionModel
    {
        public int BranchCode { get; set; }
        public int StationNo { get; set; }
        public string ApiVers { get; set; }
        public string HtmlVers { get; set; }
        public string MyQueueName { get; set; }
        public string? GetMmeesage { get; set; }
    }
}

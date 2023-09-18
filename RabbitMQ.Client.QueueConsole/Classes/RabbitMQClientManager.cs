
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Common.Model;
using System.Text;

namespace RabbitMQ.Client.QueueConsole.Classes
{
    public class RabbitMQClientManager
    {
        
        private static RabbitMQClientManager obj;
        public static RabbitMQClientManager Obj()
        {
            if (obj == null)
            {
                obj = new RabbitMQClientManager();
            }
            return obj;
        }
        public async Task AddLog(string logText)
        {
            //Task.Delay(1000).Wait();
            Console.WriteLine(logText);
        }

        #region RabbitConnect Properties

        private Dictionary<string, IConnection> RabbitMQConneciton_List;
        private Dictionary<string, IModel> RabbitMQChannel_List;

        private readonly string _connectStringRabbit = "amqp://echotest:1@192.168.1.45:5672";

        private readonly string _getVersionQueue = "GET_VERSION_QUEUE";

        private readonly string _getVersiyonRoutKey = "GET_VERSION_RABBIT";

        private readonly string _routesVersionExchange = "Routes_Version_Exchange";

        public RabbitMQClientManager()
        {
            RabbitMQConneciton_List = new();
            RabbitMQChannel_List = new();
        }

        #endregion
        private IConnection GetConnection()
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(_connectStringRabbit)
            };

            var connect = connectionFactory.CreateConnection();

            if (connect.IsOpen)
            {
                AddLog("Sistemde Bağlantı Açıldı.....");
            }
            else
            {
                AddLog("Sistemde Bağlantı Açılamadı");
                            }
            return connect;
        }
        private IModel GetChannelModel(IConnection conneclt)
        {

            IModel channel = conneclt.CreateModel();

            return channel;
        }

        private void WriteToQueue(IModel channel, string exchangeName, string queueName, GetVersionModel jsonModel) /* Mesajı publish ediyoruz  */
        {
            var messageArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonModel));

            channel.BasicPublish(exchangeName, queueName, null, messageArr);

            AddLog($"{jsonModel.MyQueueName} kuyruğuna Mesaj Eklendi ");
        }
        public string ClientCountCreatedConnection(int clientCount)
        {
            for (int i = 1; i <= clientCount; i++)
            {
                /* "SET_VERSIYON_" + i + "_1_RABBIT"  */
                string CheckConnect = "Branch_" + i + "_Connect";
                string CheckChannel = "Branch_" + i + "_Channel";

                if (!RabbitMQConneciton_List.TryGetValue(CheckConnect, out IConnection connectvalue))
                {
                    RabbitMQConneciton_List.Add(CheckConnect, GetConnection());
                    RabbitMQChannel_List.Add(CheckChannel, GetChannelModel(RabbitMQConneciton_List[CheckConnect]));
                }
            }
            return "Connection is Open " + clientCount;
        }
        public void SendMessage(int messageCount)
        {
            int count = Convert.ToInt32(messageCount);
            Task task = new(() =>
            {
                int index = 0;

                foreach (var channel in RabbitMQChannel_List)
                {
                    index++;
                    for (int i = 1; i <= count; i++)
                    {
                        GetVersionModel model = new GetVersionModel
                        {
                            MyQueueName = "SET_VERSIYON_" + index + "_1_RABBIT",
                            BranchCode = index,
                            StationNo = 1,
                            ApiVers = "4.1.6.5",
                            HtmlVers = "5.2.1",
                            GetMmeesage = $"{i}. Mesaj.."
                        };
                        WriteToQueue(channel.Value, _routesVersionExchange, _getVersiyonRoutKey, model);
                    }
                }
            });
            task.Start();
            
        }
        public void ReplyMessage()
        {
            Task task = new(() =>
            {

                int index = 0;
                foreach (var channel in RabbitMQChannel_List)
                {
                    index++;
                    string[] channelCheck = channel.Key.Split('_');
                    string myQueueCheck = "SET_VERSIYON_" + channelCheck[1] + "_1_RABBIT";

                    if (RabbitMQChannel_List.TryGetValue(channel.Key, out IModel channelRabbit))
                    {
                        channelRabbit.QueueBind(myQueueCheck, _routesVersionExchange, myQueueCheck); /*  tüm bağlantılar sağlandı. */


                        var consumer = new EventingBasicConsumer(channel.Value);
                        consumer.Received += (ch, ea) =>
                        { 
                            string setQueueName = channel.Key;
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            var modelConvert = JsonConvert.DeserializeObject<GetVersionModel>(message);
                            AddLog("----------------------------------");
                            AddLog(channel.Key + " Queden Cevap =  " + message);
                        };


                        channelRabbit.BasicConsume(myQueueCheck, true, consumer);
                    }

                }



            });
            task.Start();
        }




    }
}



using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Common.Model;
using System.Data;
using System.Text;


namespace RabbitMQ.Server.Queue.Manager
{
    public class Server_GetSet_Message
    {

        //private readonly string _connectStringRabbit = "amqp://guest:guest@localhost:5672";
        private Server_SQL_CRUD Server_SQL_CRUDClass = Server_SQL_CRUD.Obj();


        private IConnection _connectionRabbit;
        private IModel _channel;

        #region Properties

        private readonly string _connectStringRabbit = "amqp://guest:guest@localhost:5672";

        private readonly string _getVersionQueue = "GET_VERSION_QUEUE";

        private readonly string _getVersiyonRoutKey = "GET_VERSION_RABBIT";

        private readonly string _routesVersionExchange = "Routes_Version_Exchange";


        #endregion

        private static Server_GetSet_Message obj;
        public static Server_GetSet_Message Obj()
        {
            if (obj == null)
            {
                obj = new Server_GetSet_Message();
            }
            return obj;
        }

        private IConnection GetConnection()
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(_connectStringRabbit)
            };
            
            var connect= connectionFactory.CreateConnection();

            if (connect.IsOpen)
            {
                Console.WriteLine("Sistemde Bağlantı Açıldı.....");
            }
            else
            {
                Console.WriteLine("Sistemde Bğalantı Açılamadı");
            }
            return connect;
        }
        private IModel GetChannelModel()
        {
            if (_channel == null)
            {
                _channel = _connectionRabbit.CreateModel();
            }
            return _channel;
        }
        public void RunQueue()
        {
            /* bağlantı durumunun kontrolü */
            if (_connectionRabbit == null || !_connectionRabbit.IsOpen)
            {
                _connectionRabbit = GetConnection();
            }
            _channel = GetChannelModel();

            CreateExchange(_channel, _routesVersionExchange, "direct");

            CreateQueue(_channel, _getVersionQueue);

            _channel.QueueBind(_getVersionQueue, _routesVersionExchange, _getVersiyonRoutKey); /*  tüm bağlantılar sağlandı. */

            Console.WriteLine($"Mesaj Bekleniyor.");

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += Consumer_Received;
                
            _channel.BasicConsume(_getVersionQueue, true, consumer);

            Console.WriteLine($"{_getVersionQueue} dinleniyor");
        }

        private void Consumer_Received(object? sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var modelConvert = JsonConvert.DeserializeObject<GetVersionModel>(message);
            Console.WriteLine($"Received Data = {message.ToString()}");

            /* sql işlemerinde versiyonu güncel olmadığı ortaya çıkmış bulundu. */
            GetVersionModel sqlmodel = Server_SQL_CRUDClass.GetById(modelConvert.BranchCode);

            modelConvert.ApiVers = sqlmodel.ApiVers;
            modelConvert.HtmlVers = sqlmodel.HtmlVers;
            modelConvert.GetMmeesage = @"{ ""BranchID"": ""100"", ""StationNo"": ""1"", ""IpAddress"": ""192.168.1.98"", ""Version"": ""????"", ""InfiniaApiVersion"": ""????"", ""InfiniaExeVersion"": ""????"", ""WindowsOsVersion"": ""????"", ""ModuleVersion"": ""????"", ""HtmlVersion"": ""????"", ""MacAddress"": ""????"", ""ApiVersionUpdate"": true, ""ApiMainBaseUrl"": ""http://localhost:44376"", ""ApiMainUserName"": ""echoclientrestapi95iw46"", ""ApiMainUserPass"": ""mbj4zDP1I7"", ""ApiVersionBaseUrl"": ""http://localhost:44376"", ""ApiVersionUserName"": ""echoclientrestapi95iw46"", ""ApiVersionUserPass"": ""mbj4zDP1I7"", ""ApiBackupBaseUrl"": ""http://localhost:44376"", ""ApiBackupUserName"": ""echoclientrestapi95iw46"", ""ApiBackupUserPass"": ""mbj4zDP1I7"", ""ApiPluBaseUrl"": ""http://localhost:44376"", ""ApiPluUserName"": ""echoclientrestapi95iw46"", ""ApiPluUserPass"": ""mbj4zDP1I7"", ""ApiMediaBaseUrl"": ""http://localhost:44376"", ""ApiMediaUserName"": ""echoclientrestapi95iw46"", ""ApiMediaUserPass"": ""mbj4zDP1I7"", ""ApiConnUpdate"": true, ""BackupFtpBaseUrl"": ""ftp://SOK-ECHOP3.YILDIZ.DOMAIN/"", ""BackupFtpUserName"": ""echoftpclient"", ""BackupFtpUserPass"": ""i3N1t0aYUR"", ""PluFtpBaseUrl"": ""ftp://SOK-ECHOP1.YILDIZ.DOMAIN/"", ""PluFtpUserName"": ""echoftpclient"", ""PluFtpUserPass"": ""i3N1t0aYUR"", ""MediaFtpBaseUrl"": ""ftp://SOK-ECHOP1.YILDIZ.DOMAIN/"", ""MediaFtpUserName"": ""echoftpclient"", ""MediaFtpUserPass"": ""i3N1t0aYUR"", ""BackUpMainFile"": ""E:\\ECHO_FTP\\"", ""FtpConnUpdate"": true, ""LogRemoveDay"": 7, ""ClientAgainMinute"": 5, ""VersionTaskAgainMinute"": 4, ""PluAgainMinute"": 2, ""MediaAgainMinute"": 20, ""ParamKeyAgainMinute"": 0, ""DataBackupMinute"": 10, ""DataBackupStartHour"": ""10"", ""DataBackupEndHour"": ""10"", ""DataBackupCheckHour"": ""10"", ""DataBackupLastThreeDays"": ""7.11.2022 00:00:00"", ""SettingsUpdate"": false }";



            /* ilgili sql işlemleri yapıldı. */

            string getQueueName = modelConvert.MyQueueName;

            CreateQueue(_channel, getQueueName);

            _channel.QueueBind(getQueueName, _routesVersionExchange, getQueueName); /*  tüm bağlantılar sağlandı. */

            WriteToQueue(_channel, _routesVersionExchange, getQueueName, modelConvert);  /* mesaj artık burdan gönderiliyor...  */
        }

        private bool CheckExchangeExtists(IModel channel , string exchangeName )
        {
            try
            {
                channel.ExchangeDeclarePassive(exchangeName);
                return true;
            }
            catch
            {
                return false;
            }

        }
        private void CreateExchange(IModel channel, string exchangeName, string exchangeType)
        {
            try
            {
                channel.ExchangeDeclare(exchangeName, exchangeType);/* eğer durableyi true yaparsak kalıcı olur kapatılıp açıldığında silinmez exchane kalıcı kalır. */ 
            }
            catch 
            {

            }
        }
        private bool CheckQueueExtists(IModel channel, string queueName)
        {
            try
            {
                channel.ExchangeDeclarePassive(queueName);
                return true;
            }
            catch
            {
                return false;
            }

        }
        private void CreateQueue(IModel channel, string queueName)
        {
            try
            {
                channel.QueueDeclare(queueName,false,false,false );
            }
            catch
            {

            }
        }
        private void WriteToQueue(IModel channel,string exchangeName ,string queueName ,GetVersionModel jsonModel)
        {
            var messageArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonModel));

            channel.BasicPublish(exchangeName, queueName, null, messageArr);

            Console.WriteLine($"{queueName} kuyruğuna Mesaj Eklendi ");
        }



    }
   
}

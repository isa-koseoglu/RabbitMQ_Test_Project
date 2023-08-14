

using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Common.Model;
using System.Text;


namespace RabbitMQ.Server.Queue.Manager
{
    public class Server_GetSet_Message
    {

        //private readonly string _connectStringRabbit = "amqp://guest:guest@localhost:5672";

        private IConnection _connectionRabbit;
        private IModel _channel;

        #region Properties

        private readonly string _connectStringRabbit = "amqp://guest:guest@localhost:5672";

        private readonly string _getVersionQueue = "GET_VERSION_QUEUE";

        private readonly string _getVersiyonRoutKey = "GET_VERSION_RABBIT";

        private readonly string _routesVersionExchange = "Routes_Version_Exchange";


        #endregion


        //public Server_GetSet_Message()
        //{
            
           
        //}

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

            bool GuncelMi = false;
            /*  güncel değilse güncel api versiyonları eklenip gönderilebilir .. */
            modelConvert.GetMmeesage = "Versiyonun Güncel Değil Neden..!!";

            /* ilgili sql işlemleri yapıldı. */

            if (!GuncelMi)
            {

                GuncelMi = true;

                string getQueueName=modelConvert.MyQueueName;

                CreateQueue(_channel, getQueueName);

                _channel.QueueBind(getQueueName, _routesVersionExchange, getQueueName); /*  tüm bağlantılar sağlandı. */

                WriteToQueue(_channel, _routesVersionExchange, getQueueName, modelConvert);  /* mesaj artık burdan gönderiliyor...  */
            }
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

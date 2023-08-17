using Newtonsoft.Json;
using RabbitMQ.Common.Model;
using System.Text;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Client.Queue
{
    public partial class Dashboard : Form
    {
        #region LOG Funciton

        public async Task AddLog1(string logText)
        {
            //Task.Delay(1000).Wait();
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke((MethodInvoker)delegate
                {
                    AddLog1(logText);
                });
            }
            else
            {
                listBox1.Items.Add("[ " + DateTime.Now.ToString("G") + " ] " + logText);

                // En son eklenen log girdisini görüntülemek için ListBox'ý kaydýr
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
            }

        }
        public async Task AddLog2(string logText)
        {
            //Task.Delay(1000).Wait();
            if (listBox2.InvokeRequired)
            {
                listBox2.Invoke((MethodInvoker)delegate
                {
                    AddLog2(logText);
                });
            }
            else
            {
                listBox2.Items.Add("[ " + DateTime.Now.ToString("G") + " ] " + logText);

                // En son eklenen log girdisini görüntülemek için ListBox'ý kaydýr
                listBox2.SelectedIndex = listBox1.Items.Count - 1;
                listBox2.SelectedIndex = -1;
            }

        }
        public async Task AddLog3(string logText)
        {
            //Task.Delay(1000).Wait();
            if (listBox3.InvokeRequired)
            {
                listBox3.Invoke((MethodInvoker)delegate
                {
                    AddLog3(logText);
                });
            }
            else
            {
                listBox3.Items.Add(logText);

                // En son eklenen log girdisini görüntülemek için ListBox'ý kaydýr
                listBox3.SelectedIndex = listBox1.Items.Count - 1;
                listBox3.SelectedIndex = -1;
            }

        }
        public async Task LogDeleted()
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke((MethodInvoker)delegate
                {
                    LogDeleted();
                });
            }
            else
            {
                listBox1.Items.Clear();
            }

            if (listBox2.InvokeRequired)
            {
                listBox2.Invoke((MethodInvoker)delegate
                {
                    LogDeleted();
                });
            }
            else
            {
                listBox2.Items.Clear();
            }
            if (listBox3.InvokeRequired)
            {
                listBox3.Invoke((MethodInvoker)delegate
                {
                    LogDeleted();
                });
            }
            else
            {
                listBox3.Items.Clear();
            }
        }
        #endregion

        public Dashboard()
        {
            InitializeComponent();
        }

        #region Connect And Properties ---------------------------------

        private IConnection _connectionRabbit;
        private IModel _channel;

        #region Properties

        private readonly string _connectStringRabbit = "amqp://guest:guest@localhost:5672";

        private readonly string _getVersionQueue = "GET_VERSION_QUEUE";

        private readonly string _getVersiyonRoutKey = "GET_VERSION_RABBIT";

        private readonly string _routesVersionExchange = "Routes_Version_Exchange";


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
                AddLog1("Sistemde Baðlantý Açýldý.....");
                AddLog2("Sistemde Baðlantý Açýldý.....");
                AddLog3("Sistemde Baðlantý Açýldý.....");
            }
            else
            {
                AddLog1("Sistemde Baðlantý Açýlamadý");
                AddLog2("Sistemde Baðlantý Açýlamadý");
                AddLog3("Sistemde Baðlantý Açýlamadý");
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
        private void CreateExchange(IModel channel, string exchangeName, string exchangeType)
        {
            try
            {
                channel.ExchangeDeclare(exchangeName, exchangeType);/* eðer durableyi true yaparsak kalýcý olur kapatýlýp açýldýðýnda silinmez exchane kalýcý kalýr. */
            }
            catch
            {

            }
        }

        private void CreateQueue(IModel channel, string queueName)
        {
            try
            {
                channel.QueueDeclare(queueName, false, false, false);
            }
            catch
            {

            }
        }
        private void WriteToQueue(IModel channel, string exchangeName, string queueName, GetVersionModel jsonModel) /* Mesajý publish ediyoruz  */
        {
            var messageArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonModel));

            channel.BasicPublish(exchangeName, queueName, null, messageArr);

            AddLog1($"{jsonModel.MyQueueName} kuyruðuna Mesaj Eklendi ");
        }

        private async Task<List<string>> GetAllQueueAsync() /* Tüm Queue alýyoruz ve iþlemlerini bunlar dahilinde yapýyoruz. */
        {
            List<string> queueList = new List<string>();
            string baseUrl = "http://localhost:15672/api/";
            string username = "guest";
            string password = "guest";

            using (var httpClient = new HttpClient())
            {
                var base64AuthInfo = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64AuthInfo);

                var queuesUrl = $"{baseUrl}queues";
                var response = await httpClient.GetAsync(queuesUrl);
                var content = await response.Content.ReadAsStringAsync();
                var queues = JsonConvert.DeserializeObject<QueueInfo[]>(content);
                int count = queues.Count();
                foreach (var queue in queues)
                {
                    string queueName = queue.name;
                    if (queueName.StartsWith("SET_VERSIYON"))
                    {
                        queueList.Add(queueName);
                    }


                }

            }
            return queueList.ToList();
        }
        class QueueInfo /* Class oluþturmama amacaý gelen bir deðeri bu sýnýfa atýp kontrol etmek. */
        {
            public string name { get; set; }
        }

        #endregion ---------------------------------------------


        private void Dashboard_Load(object sender, EventArgs e) /* login olurken ilgili baðlantý iþelmerimi yapýyorum  */
        {

            if (_connectionRabbit == null || !_connectionRabbit.IsOpen)
            {
                _connectionRabbit = GetConnection();
            }
            _channel = GetChannelModel();
            try
            {
                CreateExchange(_channel, _routesVersionExchange, "direct");
                CreateQueue(_channel, _getVersionQueue);
            }
            catch (Exception)
            {

            }

        }
        private void pnl1SendRequest_btn_Click(object sender, EventArgs e) /* servere mesajý publish ediyorum */
        {
            
            if (string.IsNullOrEmpty(pnl1CountClient_txt.Text))
            {
                MessageBox.Show("Lütfen Boþ Býrakmayýnýz Adedi");
                return;
            }
            bool checkNumer = false;
            try
            {
                int numbertxt = int.Parse(pnl1CountClient_txt.Text);
                checkNumer = true;
            }
            catch (Exception)
            {
                checkNumer = false;
                MessageBox.Show("Lütfen Sayý Dýþýnda Veri Girmeyiniz.");
            }

            if (checkNumer)
            {
                int count = Convert.ToInt32(pnl1CountClient_txt.Text);
                Task task = new(() =>
                {
                    for (int i = 1; i <= count; i++)
                    {
                        GetVersionModel model = new GetVersionModel
                        {
                            MyQueueName = "SET_VERSIYON_" + i + "_1_RABBIT",
                            BranchCode = i,
                            StationNo = 1,
                            ApiVers = "4.1.6.5",
                            HtmlVers = "5.2.1"
                        };
                        WriteToQueue(_channel, _routesVersionExchange, _getVersiyonRoutKey, model);
                    }
                });
                task.Start();

            }
        }

        private async void AllDeleteQueue_btn_Click(object sender, EventArgs e) /* server tarafýnda clienta özgü oluþturduðum eventleri silme planlanmasý bunun servere eklenmesi gerekiyor tek bir yereden yönetim için test için buraya ekelndi. */
        {
            Task task = new(async () =>
            {
                var queueAllList = await GetAllQueueAsync();
                if (queueAllList.Count == 0)
                {
                    MessageBox.Show($"Kuyruk Yok ");

                }
                else
                {
                    foreach (var queue in queueAllList)
                    {
                        _channel.QueueDelete(queue);
                    }
                    MessageBox.Show($"{queueAllList.Count} adet kuyruk silindi.. ");
                }

                LogDeleted();
                AddLog1("Sistem Dinliyor");
                AddLog2("Sistem Dinliyor");
                AddLog3("Sistem Dinliyor");

            });
            task.Start();

        }

        private async void AllClientReadQueue_btn_Click(object sender, EventArgs e) /* Server tarafýndan oluþturduðum tüm queue'leri alýp okuma iþlemi yapar.  */
        {
            var getlistQueue = await GetAllQueueAsync();
            if (getlistQueue.Count == 0)
            {
                AddLog3("Bana Tanýmlanmýþ Kuyruk Yok ki");
                return;
            }
            Task task = new(() =>
            {
                foreach (var getQueue in getlistQueue)
                {
                    string getQueueKey = getQueue;

                    _channel.QueueBind(getQueueKey, _routesVersionExchange, getQueueKey); /*  tüm baðlantýlar saðlandý. */

                    var consumer = new EventingBasicConsumer(_channel);
                    consumer.Received += (ch, ea) =>
                    {
                        string setQueueName = getQueueKey;
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var modelConvert = JsonConvert.DeserializeObject<GetVersionModel>(message);
                        AddLog3(getQueueKey + " Queden Cevap =  " + message);
                    };


                    _channel.BasicConsume(getQueueKey, true, consumer);
                }
            });
            task.Start();

            Console.WriteLine($"Mesaj Bekleniyor.");
        }

        private void pnl2ClientReply_btn_Click(object sender, EventArgs e) /* ilgili queden bilgi geldiðini doðru test edebilmek için yapýldý.  */
        {
            if (string.IsNullOrEmpty(pnl2WhichClient_txt.Text))
            {
                MessageBox.Show("Queue Name Girilmesi Gerekiyor");
                return;
            }

            string getQueueKey = pnl2WhichClient_txt.Text.Trim();
            try
            {
                _channel.QueueBind(getQueueKey, _routesVersionExchange, getQueueKey); /*  tüm baðlantýlar saðlandý. */

                listBox2.Items.Clear();
                AddLog2("Dinleme Yapýlýyor.");
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (ch, ea) =>
                {
                    string setQueueName = getQueueKey;
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var modelConvert = JsonConvert.DeserializeObject<GetVersionModel>(message);
                    AddLog2(getQueueKey + " Queden Cevap =  " + message);
                };


                _channel.BasicConsume(getQueueKey, true, consumer);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Server Tarafýndan adýna açýlmýþ bir kuyruk bulanamadý");
            }

        }
    }
}
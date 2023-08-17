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

                // En son eklenen log girdisini g�r�nt�lemek i�in ListBox'� kayd�r
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

                // En son eklenen log girdisini g�r�nt�lemek i�in ListBox'� kayd�r
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

                // En son eklenen log girdisini g�r�nt�lemek i�in ListBox'� kayd�r
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
                AddLog1("Sistemde Ba�lant� A��ld�.....");
                AddLog2("Sistemde Ba�lant� A��ld�.....");
                AddLog3("Sistemde Ba�lant� A��ld�.....");
            }
            else
            {
                AddLog1("Sistemde Ba�lant� A��lamad�");
                AddLog2("Sistemde Ba�lant� A��lamad�");
                AddLog3("Sistemde Ba�lant� A��lamad�");
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
                channel.ExchangeDeclare(exchangeName, exchangeType);/* e�er durableyi true yaparsak kal�c� olur kapat�l�p a��ld���nda silinmez exchane kal�c� kal�r. */
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
        private void WriteToQueue(IModel channel, string exchangeName, string queueName, GetVersionModel jsonModel) /* Mesaj� publish ediyoruz  */
        {
            var messageArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonModel));

            channel.BasicPublish(exchangeName, queueName, null, messageArr);

            AddLog1($"{jsonModel.MyQueueName} kuyru�una Mesaj Eklendi ");
        }

        private async Task<List<string>> GetAllQueueAsync() /* T�m Queue al�yoruz ve i�lemlerini bunlar dahilinde yap�yoruz. */
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
        class QueueInfo /* Class olu�turmama amaca� gelen bir de�eri bu s�n�fa at�p kontrol etmek. */
        {
            public string name { get; set; }
        }

        #endregion ---------------------------------------------


        private void Dashboard_Load(object sender, EventArgs e) /* login olurken ilgili ba�lant� i�elmerimi yap�yorum  */
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
        private void pnl1SendRequest_btn_Click(object sender, EventArgs e) /* servere mesaj� publish ediyorum */
        {
            
            if (string.IsNullOrEmpty(pnl1CountClient_txt.Text))
            {
                MessageBox.Show("L�tfen Bo� B�rakmay�n�z Adedi");
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
                MessageBox.Show("L�tfen Say� D���nda Veri Girmeyiniz.");
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

        private async void AllDeleteQueue_btn_Click(object sender, EventArgs e) /* server taraf�nda clienta �zg� olu�turdu�um eventleri silme planlanmas� bunun servere eklenmesi gerekiyor tek bir yereden y�netim i�in test i�in buraya ekelndi. */
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

        private async void AllClientReadQueue_btn_Click(object sender, EventArgs e) /* Server taraf�ndan olu�turdu�um t�m queue'leri al�p okuma i�lemi yapar.  */
        {
            var getlistQueue = await GetAllQueueAsync();
            if (getlistQueue.Count == 0)
            {
                AddLog3("Bana Tan�mlanm�� Kuyruk Yok ki");
                return;
            }
            Task task = new(() =>
            {
                foreach (var getQueue in getlistQueue)
                {
                    string getQueueKey = getQueue;

                    _channel.QueueBind(getQueueKey, _routesVersionExchange, getQueueKey); /*  t�m ba�lant�lar sa�land�. */

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

        private void pnl2ClientReply_btn_Click(object sender, EventArgs e) /* ilgili queden bilgi geldi�ini do�ru test edebilmek i�in yap�ld�.  */
        {
            if (string.IsNullOrEmpty(pnl2WhichClient_txt.Text))
            {
                MessageBox.Show("Queue Name Girilmesi Gerekiyor");
                return;
            }

            string getQueueKey = pnl2WhichClient_txt.Text.Trim();
            try
            {
                _channel.QueueBind(getQueueKey, _routesVersionExchange, getQueueKey); /*  t�m ba�lant�lar sa�land�. */

                listBox2.Items.Clear();
                AddLog2("Dinleme Yap�l�yor.");
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
                MessageBox.Show("Server Taraf�ndan ad�na a��lm�� bir kuyruk bulanamad�");
            }

        }
    }
}
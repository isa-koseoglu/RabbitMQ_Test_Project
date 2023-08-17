using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Common.Model;
using System;
using System.Text;


namespace RabbitMQ.Client.Queue
{
    public partial class Dashboard2 : Form
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

                // En son eklenen log girdisini görüntülemek için ListBox'ı kaydır
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

                // En son eklenen log girdisini görüntülemek için ListBox'ı kaydır
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

                // En son eklenen log girdisini görüntülemek için ListBox'ı kaydır
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

        public Dashboard2()
        {
            InitializeComponent();
        }

        #region Connect And Properties ---------------------------------

        private Dictionary<string, IConnection> RabbitMQConneciton_List;
        private Dictionary<string, IModel> RabbitMQChannel_List;

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
                AddLog1("Sistemde Bağlantı Açıldı.....");
                AddLog2("Sistemde Bağlantı Açıldı.....");
                AddLog3("Sistemde Bağlantı Açıldı.....");
            }
            else
            {
                AddLog1("Sistemde Bağlantı Açılamadı");
                AddLog2("Sistemde Bağlantı Açılamadı");
                AddLog3("Sistemde Bağlantı Açılamadı");
            }
            return connect;
        }
        private IModel GetChannelModel(IConnection conneclt)
        {

            IModel channel = conneclt.CreateModel();

            return channel;
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
        private void WriteToQueue(IModel channel, string exchangeName, string queueName, GetVersionModel jsonModel) /* Mesajı publish ediyoruz  */
        {
            var messageArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonModel));

            channel.BasicPublish(exchangeName, queueName, null, messageArr);

            AddLog1($"{jsonModel.MyQueueName} kuyruğuna Mesaj Eklendi ");
        }

        private async Task<List<string>> GetAllQueueAsync() /* Tüm Queue alıyoruz ve işlemlerini bunlar dahilinde yapıyoruz. */
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
        class QueueInfo /* Class oluşturmama amacaı gelen bir değeri bu sınıfa atıp kontrol etmek. */
        {
            public string name { get; set; }
        }

        #endregion ---------------------------------------------


        private void Dashboard2_Load(object sender, EventArgs e) /* login olurken ilgili bağlantı işelmerimi yapıyorum  */
        {

            RabbitMQConneciton_List = new();
            RabbitMQChannel_List = new();

            //if (_connectionRabbit == null || !_connectionRabbit.IsOpen)
            //{
            //    _connectionRabbit = GetConnection();
            //}
            //_channel = GetChannelModel();
            //try
            //{
            //    CreateExchange(_channel, _routesVersionExchange, "direct");
            //    CreateQueue(_channel, _getVersionQueue);
            //}
            //catch (Exception)
            //{

            //}

        }
        private void pnl1SendRequest_btn_Click(object sender, EventArgs e) /* servere mesajı publish ediyorum */
        {

            if (string.IsNullOrEmpty(pnl1CountClient_txt.Text))
            {
                MessageBox.Show("Lütfen Boş Bırakmayınız Adedi");
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
                MessageBox.Show("Lütfen Sayı Dışında Veri Girmeyiniz.");
            }

            if (checkNumer)
            {
                int count = Convert.ToInt32(pnl1CountClient_txt.Text);
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
                                GetMmeesage=$"{i}. Mesaj.."
                            };
                            WriteToQueue(channel.Value, _routesVersionExchange, _getVersiyonRoutKey, model);
                        }
                    }
                });
                task.Start();

            }
        }

        private async void AllDeleteQueue_btn_Click(object sender, EventArgs e) /* server tarafında clienta özgü oluşturduğum eventleri silme planlanması bunun servere eklenmesi gerekiyor tek bir yereden yönetim için test için buraya ekelndi. */
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
                    foreach (var channel in RabbitMQChannel_List)
                    {
                        string[] channelCheck = channel.Key.Split('_');
                        string myQueueCheck = "SET_VERSIYON_" + channelCheck[1] + "_1_RABBIT";
                        channel.Value.QueueDelete(myQueueCheck);
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

        private async void AllClientReadQueue_btn_Click(object sender, EventArgs e) /* Server tarafından oluşturduğum tüm queue'leri alıp okuma işlemi yapar.  */
        {
            var getlistQueue = await GetAllQueueAsync();
            if (getlistQueue.Count == 0)
            {
                AddLog3("Bana Tanımlanmış Kuyruk Yok ki");
                return;
            }
            Task task = new(() =>
            {

            //foreach (var getQueue in getlistQueue)
            //{
            //    string getQueueKey = getQueue;

            //    _channel.QueueBind(getQueueKey, _routesVersionExchange, getQueueKey); /*  tüm bağlantılar sağlandı. */

            //    var consumer = new EventingBasicConsumer(_channel);
            //    consumer.Received += (ch, ea) =>
            //    {
            //        string setQueueName = getQueueKey;
            //        var body = ea.Body.ToArray();
            //        var message = Encoding.UTF8.GetString(body);
            //        var modelConvert = JsonConvert.DeserializeObject<GetVersionModel>(message);
            //        AddLog3(getQueueKey + " Queden Cevap =  " + message);
            //    };


            //    _channel.BasicConsume(getQueueKey, true, consumer);
            //}
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
                            AddLog3(channel.Key + " Queden Cevap =  " + message);
                        };


                        channelRabbit.BasicConsume(myQueueCheck, true, consumer);
                    }

                }
                
                

            });
            task.Start();

            Console.WriteLine($"Mesaj Bekleniyor.");
        }

        private void pnl2ClientReply_btn_Click(object sender, EventArgs e) /* ilgili queden bilgi geldiğini doğru test edebilmek için yapıldı.  */
        {
            if (string.IsNullOrEmpty(pnl2WhichClient_txt.Text))
            {
                MessageBox.Show("Queue Name Girilmesi Gerekiyor");
                return;
            }

            string getQueueKey = pnl2WhichClient_txt.Text.Trim();

            try
            {
                // string channelCheck= "Branch_" + i + "_Channel";/* "SET_VERSIYON_" + index + "_1_RABBIT",  "Branch_1_Channel */
                string[] channelCheck = getQueueKey.Split('_');
                string myQueueCheck = "SET_VERSIYON_" + channelCheck[1] + "_1_RABBIT";

                if (RabbitMQChannel_List.TryGetValue(getQueueKey,out IModel channelRabbit))
                {
                    channelRabbit.QueueBind(myQueueCheck, _routesVersionExchange, myQueueCheck); /*  tüm bağlantılar sağlandı. */

                    listBox2.Items.Clear();
                    AddLog2("Dinleme Yapılıyor.");
                    var consumer = new EventingBasicConsumer(channelRabbit);
                    consumer.Received += (ch, ea) =>
                    {
                        string setQueueName = myQueueCheck;
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var modelConvert = JsonConvert.DeserializeObject<GetVersionModel>(message);
                        AddLog2(myQueueCheck + " Queden Cevap =  " + message);
                    };


                    channelRabbit.BasicConsume(myQueueCheck, true, consumer);
                }
                
            }
            catch (Exception exception)
            {
                MessageBox.Show("Server Tarafından adına açılmış bir kuyruk bulanamadı");
            }

        }

        private void ConnectCreated_btn_Click(object sender, EventArgs e)
        {
            int connectCount = 0;
            try
            {
                connectCount = int.Parse(ConnectCount_txt.Text.Trim());
                if (connectCount <= 0)
                {
                    MessageBox.Show("Adet girişini sadece 0 dan büyük pozitif sayılardan giriniz...");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Adet girişini sadece pozitif sayılardan giriniz...");
                return;
            }
            for (int i = 1; i <= connectCount; i++)
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
            ConnectState_lbl.Text = "Connection is Open " + connectCount;
        }

    }
}

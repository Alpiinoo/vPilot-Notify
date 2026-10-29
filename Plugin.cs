using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using RossCarlson.Vatsim.Vpilot.Plugins;

namespace VpilotNotify
{
    public class Plugin : IPlugin
    {
        private static readonly HttpClient http = new HttpClient();
        private const string ConfigFileName = "vpilotnotify-config.txt";
        private string topic = "vpilot-notify-default";

        public string Name => "VpilotNotify";

        public void Initialize(IBroker broker)
        {
            // Optimize network performance for faster delivery
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.UseNagleAlgorithm = false;

            LoadOrCreateConfig();

            Send("vPilot", "Plugin installed and connected", "default");

            // Event Subscriptions
            broker.PrivateMessageReceived += (sender, e) =>
                Send("Private: " + e.From, e.Message, "max");

            broker.RadioMessageReceived += (sender, e) =>
                Send("Radio: " + e.From, e.Message, "high");

            broker.SelcalAlertReceived += (sender, e) =>
                Send("SELCAL: " + e.From, "SELCAL call received", "max");
        }

        private void LoadOrCreateConfig()
        {
            try
            {
                string assemblyPath = Assembly.GetExecutingAssembly().Location;
                string assemblyDir = Path.GetDirectoryName(assemblyPath);
                string configPath = Path.Combine(assemblyDir, ConfigFileName);

                if (File.Exists(configPath))
                {
                    string content = File.ReadAllText(configPath).Trim();
                    if (!string.IsNullOrEmpty(content))
                    {
                        topic = content;
                    }
                }
                else
                {
                    File.WriteAllText(configPath, "my-custom-vatsim-topic-12345");
                    topic = "my-custom-vatsim-topic-12345";
                }
            }
            catch (Exception)
            {
                // Fallback to default topic on error to prevent vPilot crash
            }
        }

        private async void Send(string title, string body, string priority)
        {
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, "https://ntfy.sh/" + topic);
                req.Content = new StringContent(body ?? "", Encoding.UTF8, "text/plain");

                req.Headers.Add("Title", title);
                req.Headers.Add("Priority", priority == "max" ? "5" : "4"); // Max priority (5) forces instant push

                // Send request asynchronously without waiting for full payload processing
                _ = http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
            }
            catch (Exception)
            {
                // Silent catch to keep vPilot stable during network drops
            }
        }
    }
}
using System;
using System.IO;
using System.IO.Ports;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.Observables;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Volumecontroller {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort MySerialPort = new SerialPort();  //Define serial connection
        private IDevice activeDevice;
        private NotifyIcon ni;
        private ToolStripItem connectionToggleMenuItem;
        private Icon greenIcon, blueIcon, redIcon;
        private IDisposable audioDeviceChangedSubscription;
        private IDisposable volumeChangedSubscription;

        public ObservableCollection<IDevice> audioDevices { get; private set; }
        public CoreAudioController controller { get; private set; }

        private void refreshAudioDevices() {
            var devices = controller.GetPlaybackDevices(DeviceState.Active);
            audioDevices.Clear();
            foreach (var d in devices.OrderBy(x => x.Name)) {
                audioDevices.Add(d);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            audioDevices = new ObservableCollection<IDevice>();
            controller = new CoreAudioController();

            Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Resources/sp.png")).Stream;
            Bitmap bm = new Bitmap(iconStream);
            blueIcon = System.Drawing.Icon.FromHandle(bm.GetHicon());

            iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Resources/hp.png")).Stream;
            bm = new Bitmap(iconStream);
            greenIcon = System.Drawing.Icon.FromHandle(bm.GetHicon());

            iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Resources/speaker_red.png")).Stream;
            bm = new Bitmap(iconStream);
            redIcon = System.Drawing.Icon.FromHandle(bm.GetHicon());
            
            ni = new NotifyIcon();
            
            ni.Icon = redIcon;
            ni.Visible = true;
            ni.Text = "Volume Controller";
            ni.DoubleClick +=
                delegate (object sender, EventArgs args) {
                    Show();
                    WindowState = WindowState.Normal;
                };

            var trayIconContextMenu = new ContextMenuStrip();
            var closeMenuItem = new ToolStripMenuItem();
            connectionToggleMenuItem = new ToolStripMenuItem();

            trayIconContextMenu.SuspendLayout();
            
            trayIconContextMenu.Items.AddRange(new ToolStripItem[] {
                connectionToggleMenuItem,
                closeMenuItem
            });
            trayIconContextMenu.Name = "TrayIconContextMenu";
            trayIconContextMenu.Size = new System.Drawing.Size(153, 70);

            closeMenuItem.Name = "CloseMenuItem";
            closeMenuItem.Size = new System.Drawing.Size(152, 22);
            closeMenuItem.Text = "Exit";
            closeMenuItem.Click += new EventHandler(CloseMenuItem_Click);

            connectionToggleMenuItem.Name = "ConnectMenuItem";
            connectionToggleMenuItem.Size = new System.Drawing.Size(152, 22);
            connectionToggleMenuItem.Text = "Connect";
            connectionToggleMenuItem.Click += new EventHandler(Connect_MenuItem_Click);

            trayIconContextMenu.ResumeLayout(false);

            ni.ContextMenuStrip = trayIconContextMenu;

            Hide();
            
            COM_text_box.Text = Properties.Settings.Default.default_port_num;
            checkBox.IsChecked = Properties.Settings.Default.starts_with_windows;

            activeDevice = controller.DefaultPlaybackDevice;

            MySerialPort.BaudRate = 9600;
            //MySerialPort.PortName = Properties.Settings.Default.default_port; //Set the default port
            MySerialPort.Parity = Parity.None;
            MySerialPort.StopBits = StopBits.One;
            MySerialPort.DataBits = 8;
            MySerialPort.Handshake = Handshake.None;
            MySerialPort.RtsEnable = true;

            connect(MySerialPort);
            check_state();
        }
        public void connect(SerialPort Comport)  //Connect to com
        {
            try {
                string com = COM_text_box.Text;
                string comport = string.Format("COM{0}", com);

                MySerialPort.PortName = comport;
                Comport.Open();

                ni.Icon = isSpeaker(activeDevice) ? blueIcon : greenIcon;

                Comport.DiscardInBuffer();
                Comport.DiscardOutBuffer();

                MySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler); //Receive data from controller
                MySerialPort.Write("opening");
                sendCurrentState();

                audioDeviceChangedSubscription = controller.AudioDeviceChanged.Subscribe(AudioDeviceManager_AudioDeviceChanged);
                volumeChangedSubscription = controller.DefaultPlaybackDevice.VolumeChanged.Subscribe(AudioDeviceManager_VolumeChanged);

                Properties.Settings.Default.default_port_num = com;
                Properties.Settings.Default.default_port = comport;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex) {
                //string print2 = String.Format("Couldn't open: {0}", Comport.PortName);
                // ni bubbel grej
                //message_box.Text = print2;
                //string printout = String.Format("               Error! \n\n{0}", print2);
                //System.Windows.MessageBox.Show(printout, "Volume Controller");

                Comport.Close();
                ni.Text = "Volume Controller - Disconnected";

                ni.Icon = redIcon;
            }

        }

        private void disconnect() {
            if (MySerialPort.IsOpen) {
                try {
                    MySerialPort.Write("closing");
                    MySerialPort.Close();

                    audioDeviceChangedSubscription.Dispose();
                    volumeChangedSubscription.Dispose();
                }
                catch {
                }
                ni.Icon = redIcon;
            }
        }

        public void check_state()  // Check if the port is open
        {
            if (MySerialPort.IsOpen) {
                status_text.Text = "Connected";
                COM_text_box.IsEnabled = false;
                ConnectBtn.Content = "Disconnect";
                connectionToggleMenuItem.Text = "Disconnect";
            }
            else {
                status_text.Text = "Disconnected";
                COM_text_box.IsEnabled = true;
                ConnectBtn.Content = "Connect";
                connectionToggleMenuItem.Text = "Connect";
            }
        }

        private void sendCurrentState() {
            string deviceType;
            if (!isHeadphones(activeDevice)) {
                MySerialPort.Write("source1");
                deviceType = "Speakers";
            }
            else {
                MySerialPort.Write("source0");
                deviceType = "Headphones";
            }
            MySerialPort.Write("vol" + (int)(activeDevice.Volume));
            ni.Text = deviceType + ": " + (int)(activeDevice.Volume) + "%";
        }

        private void AudioDeviceManager_AudioDeviceChanged(DeviceChangedArgs e) {
            if (e.Device.IsDefaultDevice) {
                volumeChangedSubscription.Dispose();
                volumeChangedSubscription = e.Device.VolumeChanged.Subscribe(AudioDeviceManager_VolumeChanged);

                ni.Icon = isSpeaker(e.Device) ? blueIcon : greenIcon;

                activeDevice = e.Device;
                if (MySerialPort.IsOpen) {
                    sendCurrentState();
                }
            }
        }
        private void AudioDeviceManager_VolumeChanged(DeviceVolumeChangedArgs e) {
            if (MySerialPort.IsOpen) {
                string deviceType;
                if (isSpeaker(activeDevice)) {
                    deviceType = "Speakers";
                }
                else {
                    deviceType = "Headphones";
                }
                MySerialPort.Write("vol" + (int)(e.Volume));
                ni.Text = deviceType + ": " + e.Volume + "%";
            }
        }
        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)  // React to received data
        {
            SerialPort myport = (SerialPort)sender;
            if (!myport.IsOpen)
                return;
            int data = myport.ReadByte();

            switch (data) {
                case 1:
                    toggleSource();
                    break;
                case 0:
                    sendCurrentState();
                    break;
            }


        }
        private void toggleSource() {
            refreshAudioDevices();
            bool headphonesToBeActivated = false;
            if (!isHeadphones(activeDevice)) {
                headphonesToBeActivated = true;
            }
            foreach (var ad in audioDevices) {
                if (headphonesToBeActivated) {
                    if (isHeadphones(ad)) {
                        ad.SetAsDefault();
                        break;
                    }
                }
                else {
                    if (isSpeaker(ad)) {
                        ad.SetAsDefault();
                        break;
                    }
                }
            }
            sendCurrentState();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            disconnect();
            ni.Dispose();
        }

        private void State_Change(object sender, EventArgs e) {
            if (((MainWindow)sender).WindowState == WindowState.Minimized) {
                Hide();
            }
        }
        
        private void ConnectBtn_Click(object sender, RoutedEventArgs e) {  //Connect
            if (!MySerialPort.IsOpen) {
                connect(MySerialPort);
            }
            else {
                disconnect();
            }
            check_state();
        }


        private void Connect_MenuItem_Click(object sender, EventArgs e) {
            if (!MySerialPort.IsOpen) {
                connect(MySerialPort);
            }
            else {
                disconnect();
            }
            check_state();
        }
        private void CloseMenuItem_Click(object sender, EventArgs e) {
            /*if (
                System.Windows.MessageBox.Show(
                    "Do you really want to close me?",
                    "Are you sure?", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.Yes
                ) == MessageBoxResult.Yes) {*/
            Close();
            /*}*/
        }

        private void PlayCommand_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) {
            toggleSource();
        }


        private void Checked(object sender, RoutedEventArgs e) {
            if (StartUpManager.IsUserAdministrator()) {
                // Will Add application to All Users StartUp
                StartUpManager.AddApplicationToAllUserStartup();
            }
            else {
                // Will Add application to Current Users StartUp
                StartUpManager.AddApplicationToCurrentUserStartup();
            }
            Properties.Settings.Default.starts_with_windows = true;
            Properties.Settings.Default.Save();
        }
        private void Unchecked(object sender, RoutedEventArgs e) {
            if (StartUpManager.IsUserAdministrator()) {
                // Will Remove application to All Users StartUp
                StartUpManager.RemoveApplicationFromAllUserStartup();
            }
            else {
                // Will Remove application to Current Users StartUp
                StartUpManager.RemoveApplicationFromCurrentUserStartup();
            }
            Properties.Settings.Default.starts_with_windows = false;
            Properties.Settings.Default.Save();
        }

        private bool isSpeaker(IDevice device) {
            return device.Name.Contains("HTR") || device.Name.Contains("LG");
        }
        private bool isHeadphones(IDevice device) {
            return device.Name.Contains("Högtalare");
        }
    }
}

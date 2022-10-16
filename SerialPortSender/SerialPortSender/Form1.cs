using System.IO.Ports;

namespace SerialPortSender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SerialPort mySerialPort = new SerialPort();
        private void Form1_Load(object sender, EventArgs e)
        {
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
            timer1.Interval = 500;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                mySerialPort.PortName = comboBox1.Text;
                mySerialPort.BaudRate = Convert.ToInt32(textBox1.Text);
                mySerialPort.DataBits = Convert.ToInt32(textBox2.Text);
                mySerialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), textBox3.Text);
                mySerialPort.Parity = (Parity)Enum.Parse(typeof(Parity), textBox4.Text); ;
                mySerialPort.Open();
                MessageBox.Show($"{mySerialPort.PortName}'e bağlandı.");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message,"Error");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (mySerialPort.IsOpen)
                {
                    mySerialPort.Close();
                    MessageBox.Show($"{mySerialPort.PortName} ile olan bağlantı kesildi.");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message,"Error");
            }
        }
        string datatosend;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (mySerialPort.IsOpen)
                {
                    datatosend = textBox5.Text;
                    mySerialPort.WriteLine(datatosend);
                    MessageBox.Show("Data Send", "Notification");
                }
            }
            catch(Exception error) 
            {
                MessageBox.Show(error.Message,"Error");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt16(textBox6.Text);
            timer1.Start();
            i = 0;
            tur_sayisi = 0;

        }
        int[] surat = { 0, 5, 10, 15, 20, 50, 60, 70, 10, 15, 20, 50, 60, 70, 10, 15, 20, 50, 60, 70 };
        int[] ivme = { 9, 10, 12, 15, 32, 9, 10, 12, 15, 32, 9, 10, 12, 15, 32, 9, 10, 12, 15, 32 };
        int i = 0;
        int tur_sayisi = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tur_sayisi == 0 && i == 0)
            {
                datatosend = $"NewDataSet:1,MotorKonumu1:ABC,MotorKonumu2:ABC2,MotorKonumu3:ABC3,Sürat:{surat[i].ToString()},İvme:{ivme[i].ToString()},Basınç:10,Yükseklik:700,Sıcaklık:70";
            }
            if (tur_sayisi != 0 || i != 0)
            {
                datatosend = $"NewDataSet:0,MotorKonumu1:ABC,MotorKonumu2:ABC2,MotorKonumu3:ABC3,Sürat:{surat[i].ToString()},İvme:{ivme[i].ToString()},Basınç:10,Yükseklik:700,Sıcaklık:70";
            }
            mySerialPort.WriteLine(datatosend);
            i = i + 1;
            if(i == surat.Length-1)
            {
                i = 0;
                tur_sayisi++;
            }
            if (tur_sayisi==8)
            {
                timer1.Stop();
                MessageBox.Show("Tüm veriler gönderildi.");
                
            }
        }
    }
}
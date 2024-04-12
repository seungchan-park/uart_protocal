using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;

namespace stm32v1
{
    public partial class Form1 : Form
    {
        byte[] txdata = new byte[256]; // 배열선언
        byte[] packet = new byte[256]; 
        int packet_cnt = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Visible = false;
            string[] ports = SerialPort.GetPortNames();
            cbport.Items.AddRange(ports);
            tbrx.ForeColor = Color.Red;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // OPEN
        private void btn1_Click(object sender, EventArgs e)
        {
            label1.Visible=true;
            try
            {
                serialPort1.PortName = cbport.Text;
                serialPort1.BaudRate = Convert.ToInt32(cbbaud.Text);

                serialPort1.Open();
                btn1.Enabled = false;
                btn2.Enabled = true;
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btn1.Enabled = false;
                btn2.Enabled = false;
            }
        }

        // CLOSE
        private void btn2_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            if (serialPort1.IsOpen)
            {
                serialPort1.DiscardInBuffer();
                serialPort1.Close();
                btn1.Enabled = true;
                btn2.Enabled = false;
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int size = 0, k = 0, m = 0;
            int buff = 0;
            byte sum = 0;

            size = serialPort1.BytesToRead;
            for (k = 0; k < size; ++k)
            {
                buff = serialPort1.ReadByte();

                packet[packet_cnt++] = (byte)buff;
                switch (packet_cnt)
                {
                    case 1:
                        if (packet[packet_cnt - 1] != 0xAA) packet_cnt = 0;
                        break;
                    case 2:
                        if (packet[packet_cnt - 1] != 0x55) packet_cnt = 0;
                        break;
                    case 8:
                        for (m = 0; m < 7; ++m)
                        {
                            sum += packet[m];
                        }
                        if (sum == packet[7])
                        {                            
                            tbrx.Text += "Packet\r\n";
                        }
                        packet_cnt = 0;
                        break;
                }
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            tbrx.Text = "";
        }

        private void btntx_Click(object sender, EventArgs e)
        {
            string txstr;

            if (serialPort1.IsOpen)
            {
                txstr = tbtx.Text + '\n';
                serialPort1.Write(txstr);
            }
        }

        private void btn_servo0_Click(object sender, EventArgs e)
        {
            byte sum = 0;
            int k = 0;

            if (serialPort1.IsOpen)
            {
                txdata[0] = 0xAA;
                txdata[1] = 0x55;
                txdata[6] = 0;
                for (k = 0; k < 7; ++k)
                {
                    sum += txdata[k];
                }
                txdata[7] = sum;
                serialPort1.Write(txdata, 0, 8);
            }
        }

        private void btn_servo90_Click(object sender, EventArgs e)
        {
            byte sum = 0;
            int k = 0;

            if (serialPort1.IsOpen)
            {
                txdata[0] = 0xAA;
                txdata[1] = 0x55;
                txdata[6] = 90;
                for (k = 0; k < 7; ++k)
                {
                    sum += txdata[k];
                }
                txdata[7] = sum;
                serialPort1.Write(txdata, 0, 8);
            }
        }

        private void btn_servo180_Click(object sender, EventArgs e)
        {
            byte sum = 0;
            int k = 0;

            if (serialPort1.IsOpen)
            {
                txdata[0] = 0xAA;
                txdata[1] = 0x55;
                txdata[6] = 180;
                for (k = 0; k < 7; ++k)
                {
                    sum += txdata[k];
                }
                txdata[7] = sum;
                serialPort1.Write(txdata, 0, 8);
            }
        }
    }
}

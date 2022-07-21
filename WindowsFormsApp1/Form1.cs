using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Message = System.Messaging.Message;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var sendMessageQueue = new MessageQueue())
            {
                sendMessageQueue.Path = @".\private$\messageQueue";
                if (!MessageQueue.Exists(sendMessageQueue.Path))
                {
                    MessageQueue.Create(sendMessageQueue.Path);
                }
                MessageBox.Show("Done");
                var message = new Message { Body = textBox1.Text };
                sendMessageQueue.Send(message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var receivedMessageQueue = new MessageQueue())
            {
                receivedMessageQueue.Path = ".\\private$\\EPSS_BOF_ReceiverQueue";
                var message = new Message { Body = textBox1.Text };
                try
                {
                    message = receivedMessageQueue.Receive(new TimeSpan(0, 0, 5));
                    message.Formatter = new XmlMessageFormatter(new string[] { "System.String,mscorlib" });
                    var msg = message.Body.ToString();
                    textBox2.Text = msg;
                }
                catch
                {
                    MessageBox.Show("The queue is empty");
                }
             
            }
        }
    }
}

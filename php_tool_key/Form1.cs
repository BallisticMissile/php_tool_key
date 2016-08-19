using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace php_tool_key
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = System.IO.File.ReadAllBytes(textBox1.Text);

            byte[] publickey = Properties.Resources.publickey;

            int index = 0;
            int length = 120;

            for (int i = 0; i < data.Length && index == 0; i++)
            {
                if (data[i] == publickey[0] && data.Length - i > publickey.Length)
                {
                    for (int j = 1; j < publickey.Length; j++)
                    {
                        if (data[i + j] != publickey[j])
                        {
                            if (j < 120 || data[i + publickey.Length - 1] != 0x0B)
                                break;
                            else
                            {
                                index = i;
                                break;
                            }
                        }
                    }
                }
            }



            if (index > 0)
            {
                for (int i = 0; i < publickey.Length; i++)
                {
                    data[index + i] = publickey[i];
                }
                System.IO.File.Delete(textBox1.Text + ".back");
                System.IO.File.Move(textBox1.Text, textBox1.Text + ".back");
                System.IO.File.WriteAllBytes(textBox1.Text, data);
                MessageBox.Show("替换完成！文件已经备份到" + textBox1.Text + ".back");
            }
            else
            {
                MessageBox.Show("抱歉，该版本无法替换！");
            }
        }
    }
}

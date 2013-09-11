using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace TableCleaner
{
    public partial class Form1 : Form
    {
        List<string> strs = new List<string>();
        List<string> logs = new List<string>();
        int fb = 0, fe = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strs = new List<string>();
                Encoding enc=Encoding.GetEncoding(int.Parse(textBox6.Text));
                StreamReader sr = new StreamReader(openFileDialog1.FileName,enc);
                while (!sr.EndOfStream)
                    strs.Add(sr.ReadLine());
                sr.Close();
                textBox4.Text = "" + strs.Count;
                logs.Add("Загружен файл " + openFileDialog1.FileName + " из " + strs.Count + " строк.");
                textBox5.Lines = logs.ToArray();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Regex name = new Regex(textBox3.Text);

            fb = int.Parse(textBox1.Text);
            fe = int.Parse(textBox2.Text);
            List<string> sn = new List<string>();
            string str = "";
            for (int i = 0; i < strs.Count; i++)
            {
                str = strs[i];
                strs[i]=name.Replace(strs[i], new MatchEvaluator(Replacer));
                if(strs[i]!=str)sn.Add(""+i);
            }
            str = "Произведена замена в строках: ";
            for (int i = 0; i < sn.Count; i++) str += sn[i] + ", ";
            logs.Add(str);
            textBox5.Lines = logs.ToArray();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Encoding enc = Encoding.GetEncoding(int.Parse(textBox7.Text));
                StreamWriter sw=new StreamWriter(saveFileDialog1.FileName,false,enc);
                for (int i = 0; i < strs.Count; i++)
                    sw.WriteLine(strs[i]);
                sw.Close();
                logs.Add("Сохранен файл " + saveFileDialog1.FileName + " из " + strs.Count + " строк.");
                textBox5.Lines = logs.ToArray();
            }
        }

        string Replacer(Match m)
        {
            string s=m.Value;
            if(radioButton1.Checked){
                return s.Substring(fb,s.Length-fb-fe);
            }
            else{
                if (fb == 0)
                {
                    return s.Substring(s.Length - 1 - fe, fe);
                }
                else if (fe == 0)
                {
                    return s.Substring(0, fb);
                }
                else
                {
                    return s.Substring(0, fb) + s.Substring(s.Length - 1 - fe, fe);
                }
            }
        }
    }
}


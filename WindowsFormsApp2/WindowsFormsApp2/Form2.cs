using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        public delegate void TextEventHandler(string[,,] data, int index, int line);
        public event TextEventHandler WriteTextEvent;

        MP3Player2 mp3Player;
        Form1 form1 = new Form1();
        //Track Bar 
        bool isScrolled = false;
        int trackBarBlankSize = 14;    // TrackBar 양옆 빈공간
        int trackBarLength = 0;        // TrackBar의 실제 길이
        int trackBarMouseX = 0;        // TrackBar에서 마우스 클릭 위치
        int leng, min, sec, milliSec;
        public static String lyrics;
        public static int startNum, endNum;
        public static int total;
        int index = 0;
        int lineIndex;
        bool isIndex = true;
        int lineCount = 0;
        string file;
        public static string[,,] data = new string[500, 20, 4];
        int idx;
        string data1, data2, data3;
        string[,,] detailTime;
        int cell;
        int rowIndex;
        int columnIndex;
        string combo;
        Boolean pause = false;
        public Form2()
        {
            InitializeComponent();
            mp3Player = new MP3Player2();
            this.KeyPreview = true;
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.PowderBlue;
            mp3Player.Open(file);
            trackBarLength = DetailTracBar.Size.Width - (trackBarBlankSize * 2); // TrackBar의 실제 길이

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string[] comboBox = { "1000", "100", "10" };
            comboBox1.Items.AddRange(comboBox);
            comboBox1.SelectedIndex = 1;
        }

        // Form1 dataGridView1 의 값을 가져오는 메소드
        public void dataReceive(string[,,] result, int idx, int line)
        {
            lineIndex = idx;
            index = idx;
            for (int i = 0; i < idx; i++)
            {
                dataGridView2.Rows.Add();
                dataGridView2.Rows.Add();
                TimeSpan t1 = TimeSpan.FromMilliseconds(double.Parse(result[line, i, 0]) * 1000);
                //dataGridView2["Column2", i].Value = String.Format("{0:00}:{1:00}:{2:000}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                dataGridView2["Column2", i + i].Value = String.Format("{0:00}:{1:00}:{2:000}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                dataGridView2["Column2", i + i + 1].Value = String.Format("{0:00}:{1:00}:{2:000}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                //TimeSpan t2 = TimeSpan.FromMilliseconds(double.Parse(result[line, i, 1]) * 1000);
                //dataGridView2["Column3", i].Value = String.Format("{0:00}:{1:00}:{2:000}", t2.Minutes, t2.Seconds, t2.Milliseconds);
                //dataGridView2["Column4", i].Value = result[line, i, 2];
                dataGridView2["Column4", i + i].Value = data[line, i, 2];
                dataGridView2["Column4", i + i + 1].Value = data[line, i, 1];
            }

        }

        // Json 파일에서 값을 가져오는 메소드
        public void parserReceive(string[,,] data, int line)
        {
            //try
            //{
            if (data != null)
            {
                lineIndex = Int32.Parse(data[line, 0, 3]);
                Console.WriteLine("Json파일에서 불러오는 lineIndex : " + lineIndex);
                for (int j = 0; j < Int32.Parse(data[line, 0, 3]) * 2; j++)
                {
                    dataGridView2.Rows.Add();
                    dataGridView2.Rows.Add();
                    TimeSpan t1 = TimeSpan.FromMilliseconds(double.Parse(data[line, j, 0]) * 1000);
                    dataGridView2["Column2", j + j].Value = String.Format("{0:00}:{1:00}:{2:000}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                    dataGridView2["Column2", j + j + 1].Value = String.Format("{0:00}:{1:00}:{2:000}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                    //TimeSpan t2 = TimeSpan.FromMilliseconds(double.Parse(data[line, j, 1]) * 1000);
                    //dataGridView2["Column3", j].Value = String.Format("{0:00}:{1:00}:{2:000}", t2.Minutes, t2.Seconds, t2.Milliseconds);
                    dataGridView2["Column4", j + j].Value = data[line, j, 2];
                    dataGridView2["Column4", j + j + 1].Value = data[line, j, 1];
                }
                for (int i = 0; i < lineIndex * 2; i++)
                {
                    string[] array1 = dataGridView2.Rows[i].Cells[0].Value.ToString().Split(':');
                    double number1 = Int32.Parse(array1[0]) * 60 + Int32.Parse(array1[1]) + Int32.Parse(array1[2]) * 0.001;
                    data[line, i, 0] = number1.ToString();
                    //string[] array2 = dataGridView2.Rows[i].Cells[1].Value.ToString().Split(':');
                    //double number2 = Int32.Parse(array2[0]) * 60 + Int32.Parse(array2[1]) + Int32.Parse(array2[2]) * 0.001;
                    //data[line, i, 1] = number2.ToString();
                    data[line, i, 2] = (string)dataGridView2.Rows[i * 2].Cells[1].Value;
                    data[line, i, 1] = (string)dataGridView2.Rows[i * 2 + 1].Cells[1].Value;


                }

                WriteTextEvent(data, lineIndex, line);
            }
            //}
            //    catch { }
        }

        // Covert 버튼 클릭시 받아오는 값
        public void parserReceive2(string[,,] data, int line, int sum, bool sign)
        {
            try
            {
                if (data != null)
            {
                lineIndex = Int32.Parse(data[line, 0, 3]);
                for (int j = 0; j < Int32.Parse(data[line, 0, 3]); j++)
                {
                    dataGridView2.Rows.Add();

                    if (sign) { 
                        TimeSpan t1 = TimeSpan.FromMilliseconds((double.Parse(data[line, j, 0]) * 1000) + sum);
                        dataGridView2["Column2", j].Value = String.Format("{0:00}:{1:00}:{2:000}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                        //TimeSpan t2 = TimeSpan.FromMilliseconds(double.Parse(data[line, j, 1]) * 1000);
                        //dataGridView2["Column3", j].Value = String.Format("{0:00}:{1:00}:{2:000}", t2.Minutes, t2.Seconds, t2.Milliseconds);
                        dataGridView2["Column4", j].Value = data[line, j, 2];
                    }
                    else
                    {
                        TimeSpan t1 = TimeSpan.FromMilliseconds((double.Parse(data[line, j, 0]) * 1000) - sum);
                        dataGridView2["Column2", j].Value = String.Format("{0:00}:{1:00}:{2:000}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                        //TimeSpan t2 = TimeSpan.FromMilliseconds(double.Parse(data[line, j, 1]) * 1000);
                        //dataGridView2["Column3", j].Value = String.Format("{0:00}:{1:00}:{2:000}", t2.Minutes, t2.Seconds, t2.Milliseconds);
                        dataGridView2["Column4", j].Value = data[line, j, 2];
                    }
                    
                }
                for (int i = 0; i < lineIndex; i++)
                {
                    string[] array1 = dataGridView2.Rows[i].Cells[0].Value.ToString().Split(':');
                    double number1 = Int32.Parse(array1[0]) * 60 + Int32.Parse(array1[1]) + Int32.Parse(array1[2]) * 0.001;
                    data[line, i, 0] = number1.ToString();
                    //string[] array2 = dataGridView2.Rows[i].Cells[1].Value.ToString().Split(':');
                    //double number2 = Int32.Parse(array2[0]) * 60 + Int32.Parse(array2[1]) + Int32.Parse(array2[2]) * 0.001;
                    //data[line, i, 1] = number2.ToString();
                    data[line, i, 2] = (string)dataGridView2.Rows[i].Cells[1].Value;


                }

                WriteTextEvent(data, lineIndex, line);
            }
            }
            catch { }
        }
        // save 클릭
        public void save_button_Click(object sender, EventArgs e)
        {
            if (index == 0)
            {
                for (int i = 0; i < lineIndex; i++)
                {
                    string[] array1 = dataGridView2.Rows[i].Cells[1].Value.ToString().Split(':');
                    double number1 = Int32.Parse(array1[0]) * 60 + Int32.Parse(array1[1]) + Int32.Parse(array1[2]) * 0.001;
                    data[this.idx, i, 0] = number1.ToString();
                    //string[] array2 = dataGridView2.Rows[i].Cells[1].Value.ToString().Split(':');
                    //double number2 = Int32.Parse(array2[0]) * 60 + Int32.Parse(array2[1])  + Int32.Parse(array2[2]) * 0.001;
                    //data[this.idx, i, 1] = number2.ToString();
                    data[this.idx, i, 2] = (string)dataGridView2.Rows[i].Cells[2].Value;
                    data[this.idx, i, 1] = (string)dataGridView2.Rows[i].Cells[2].Value;
                }
                WriteTextEvent(data, lineIndex, this.idx);
            }
            else
            {
                for (int i = 0; i < index; i++)
                {
                    string[] array1 = dataGridView2.Rows[i].Cells[0].Value.ToString().Split(':');
                    double number1 = Int32.Parse(array1[0]) * 60 + Int32.Parse(array1[1]) + Int32.Parse(array1[2]) * 0.001;
                    data[this.idx, i, 0] = number1.ToString();
                    //string[] array2 = dataGridView2.Rows[i].Cells[1].Value.ToString().Split(':');
                    //double number2 = Int32.Parse(array2[0]) * 60 + Int32.Parse(array2[1]) + Int32.Parse(array2[2]) * 0.001;
                    //data[this.idx, i, 1] = number2.ToString();
                    data[this.idx, i, 2] = (string)dataGridView2.Rows[2 * i].Cells[1].Value;
                    data[this.idx, i, 1] = (string)dataGridView2.Rows[2 * i + 1].Cells[1].Value;
                }
                WriteTextEvent(data, index, this.idx);
            }
            MessageBox.Show("Save Complete");

        }

        // Form1 에서 값을 전달받는 메소드
        public void received1(int startDetailNum, int endDetailNum, int lineIndex)
        {
            startNum = startDetailNum;
            endNum = endDetailNum;
            total = endNum - startNum;
            this.idx = lineIndex;
            leng = mp3Player.GetLength();
            TimeSpan t = TimeSpan.FromMilliseconds(endDetailNum);
            TimeSpan t2 = TimeSpan.FromMilliseconds(startDetailNum);
            label8.Text = String.Format("{0:00}:{1:00}:{2:000}", t.Minutes, t.Seconds, t.Milliseconds);
            label7.Text = String.Format("{0:00}:{1:00}:{2:000}", t2.Minutes, t2.Seconds, t2.Milliseconds);
        }

        // Form1 에서 값을 전달받는 메소드
        public void received2(string str, string startDetail, string endDetail, int detailTotal)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(detailTotal);
            label1.Text = string.Format("{0:D2}:{1:D2}:{2:D3}", t.Minutes, t.Seconds, t.Milliseconds);

            label6.Text = str;
            label2.Invalidate();
        }

        // 재생 버튼
        private void button5_Click(object sender, EventArgs e)
        {
            DetailTimer.Enabled = true;
            if (pause)
            {
                mp3Player.Play();
                pause = false;
            }
            else
            {
                mp3Player.Seek(startNum);
                mp3Player.Play();
            }
        }

        // Start 버튼
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Add();
            dataGridView2.Rows.Add();
            dataGridView2["Column2", index + index].Value = label7.Text;
            dataGridView2["Column2", index + index + 1].Value = label7.Text;
            //dataGridView2["Column1", index + index].Value = index + 1;
            //dataGridView2["Column1", index + index + 1].Value = index + 1;
            index++;
            Console.WriteLine("index : " + index);
            isIndex = true;
        }

        // End 버튼
        private void button8_Click(object sender, EventArgs e)
        {
            //dataGridView2["Column3", index].Value = label7.Text;
            //index++;
            //isIndex = true;
        }

        // Revert 버튼
        private void button4_Click(object sender, EventArgs e)
        {
            //try
            //{
                if (index > 0)
                {
                    //dataGridView2["Column2", index - 1].Value = "";
                    dataGridView2["Column2", index + index - 1].Value = "";
                    dataGridView2["Column2", index + index - 2].Value = "";
                    //dataGridView2["Column1", index + index - 1].Value = "";
                    //dataGridView2["Column1", index + index - 2].Value = "";
                    //dataGridView2.Rows.Remove(dataGridView2.Rows[index - 1]);
                    dataGridView2.Rows.Remove(dataGridView2.Rows[index + index - 1]);
                    dataGridView2.Rows.Remove(dataGridView2.Rows[index + index - 2]);
                    data[this.idx, index - 1, 0] = "";
                    index--;
                    Console.WriteLine("index : " + index);

                    if (lineIndex > 0) { lineIndex--; }

                    Console.WriteLine("lineIndex : " + lineIndex);
                }
                //if (isIndex)
                //{
                //    if (index > 0)
                //    {
                //        dataGridView2["Column3", index - 1].Value = "";
                //        isIndex = false;
                //        index--;
                //        data[this.idx, index, 1] = "";

                //    }
                //}
                //else
                //{

                //    dataGridView2["Column2", index].Value = "";
                //    dataGridView2.Rows.Remove(dataGridView2.Rows[index]);
                //    isIndex = true;
                //    data[this.idx, index, 0] = "";

                //    if (lineCount > 0) { lineCount--; }
                //}
            //}
            //catch { }
        }

        // 일시정지 버튼
        private void button6_Click(object sender, EventArgs e)
        {
            mp3Player.Pause();
            pause = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            mp3Player.Seek(startNum);
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
            mp3Player.Seek(endNum);
            mp3Player.Pause();
        }

        // Detail Timer 
        private void DetailTimer_Tick(object sender, EventArgs e)
        {
            DetailTracBar.Maximum = total;
            if (!mp3Player.isOpened)
                return;


            if (isScrolled == false)

                try
                {
                    DetailTracBar.Value = mp3Player.GetPosition() - startNum;
                }
                catch { }


            if (!mp3Player.loop && mp3Player.GetPosition() > endNum)
                //mp3Player.Seek(startNum);
                mp3Player.Seek(endNum);

            SetMusicTimer();

        }

        //음악 타이머 메소드
        private void SetMusicTimer()
        {
            if (mp3Player.isOpened)
            {
                TimeSpan t = TimeSpan.FromMilliseconds(mp3Player.GetPosition() - startNum);
                TimeSpan t2 = TimeSpan.FromMilliseconds(mp3Player.GetPosition());
                label4.Text = string.Format("{0:D2}:{1:D2}:{2:D3}", t.Minutes, t.Seconds, t.Milliseconds);
                label7.Text = string.Format("{0:D2}:{1:D2}:{2:D3}", t2.Minutes, t2.Seconds, t2.Milliseconds);
                //if (mp3Player.isEnd)
                //{
                //    label4.Text = "00:00:000";

                //}
            }
        }


        // TrackBar MouseDown Event
        private void MP3TrackBar_MouseDown(object sender, MouseEventArgs e)
        {
            isScrolled = true;
            trackBarMouseX = e.X - trackBarBlankSize;     // 마우스 클릭 좌표
            SetPositionByMouse(trackBarMouseX);
        }

        // 0.5배속
        private void button2_Click(object sender, EventArgs e)
        {
            mp3Player.Speedx05();
        }

        // 1배속
        private void button1_Click(object sender, EventArgs e)
        {
            mp3Player.Speedx1();
        }

        // 2배속
        private void X2Speed_Click(object sender, EventArgs e)
        {
            mp3Player.Speedx2();
        }

        private void dataGridView2_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.KeyPreview = false;
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.KeyPreview = true;
        }

        // plus
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                cell += Convert.ToInt32(this.combo);
                TimeSpan t1 = TimeSpan.FromMilliseconds(cell);
                dataGridView2.Rows[rowIndex].Cells[columnIndex].Value = string.Format("{0:D2}:{1:D2}:{2:D3}", t1.Minutes, t1.Seconds, t1.Milliseconds);
            }
            catch { }
        }

        // minus
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                cell -= Convert.ToInt32(this.combo);
                TimeSpan t1 = TimeSpan.FromMilliseconds(cell);
                dataGridView2.Rows[rowIndex].Cells[columnIndex].Value = string.Format("{0:D2}:{1:D2}:{2:D3}", t1.Minutes, t1.Seconds, t1.Milliseconds);
            }
            catch { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                this.combo = comboBox1.SelectedItem as string;
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string[] array = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Split(':');
                cell = Int32.Parse(array[0]) * 60000 + Int32.Parse(array[1]) * 1000 + Int32.Parse(array[2]);
                rowIndex = e.RowIndex;
                columnIndex = e.ColumnIndex;

                cell = Int32.Parse(array[0]) * 60000 + Int32.Parse(array[1]) * 1000 + Int32.Parse(array[2]);
            }
            catch { }
        }




        // TrackBar MouseMove Event
        private void MP3TrackBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (isScrolled)
            {
                trackBarMouseX = e.X - trackBarBlankSize; // 마우스 클릭 좌표
                SetPositionByMouse(trackBarMouseX);
            }
        }

        // TrackBar MouseUp Event
        private void MP3TrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (mp3Player.isOpened)
            {
                string status = mp3Player.GetStatus();

                mp3Player.Seek(DetailTracBar.Value);

                if (status == "playing")
                    mp3Player.Play();
                else
                    mp3Player.Pause();
            }

            isScrolled = false;
        }

        // TrackBar ▼ 이렇게 생긴애(클릭해서 끌어당길 수 있는 애) 트랙바 클릭시 마우스 따라가게 하는 메소드
        private void SetPositionByMouse(int position)
        {
            if (position < 0 || position > trackBarLength)
                return;

            float rate = (float)position / (float)trackBarLength;
            DetailTracBar.Value = (int)(rate * (float)(DetailTracBar.Maximum - DetailTracBar.Minimum));
        }

        // 1초 앞으로 가는 버튼
        private void pre_button_Click(object sender, EventArgs e)
        {
            mp3Player.Seek(mp3Player.GetPosition() - 1000);
            mp3Player.Play();
        }

        //1초 뒤로 가는 버튼
        private void after_button_Click(object sender, EventArgs e)
        {
            mp3Player.Seek(mp3Player.GetPosition() + 1000);
            mp3Player.Play();
        }

        //키보드 
        private void Form2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ESC_KEY = (char)Keys.Escape;

            if (e.KeyChar == ESC_KEY)
            {
                Close_button_Click(sender, e);
            }
            switch (e.KeyChar.ToString())
            {
                case "a":
                case "A":
                case "ㅁ":
                    button3_Click(sender, e);
                    break;
                case "d":
                case "D":
                case "ㅇ":
                    button8_Click(sender, e);
                    break;
                case "s":
                case "S":
                case "ㄴ":
                    button6_Click(sender, e);
                    break;
                case "w":
                case "W":
                case "ㅈ":
                    button5_Click(sender, e);
                    break;
                case "q":
                case "Q":
                case "ㅂ":
                    pre_button_Click(sender, e);
                    break;
                case "e":
                case "E":
                case "ㄷ":
                    after_button_Click(sender, e);
                    break;
                case "z":
                case "Z":
                case "ㅋ":
                    button2_Click(sender, e);
                    break;
                case "x":
                case "X":
                case "ㅌ":
                    button1_Click(sender, e);
                    break;
                case "f":
                case "F":
                case "ㄹ":
                    button4_Click(sender, e);
                    break;
                case "r":
                case "R":
                case "ㄱ":
                    button7_Click(sender, e);
                    break;
                case "1":
                    button9_Click(sender, e);
                    break;
                case "2":
                    button10_Click(sender, e);
                    break;
                default:
                    break;

            }
        }
    }
}

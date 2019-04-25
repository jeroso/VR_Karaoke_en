using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Timers;
using System.IO;
using System.Net.Json;


namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        MP3Player mp3Player;

        //Track Bar 
        bool isScrolled = false;
        int trackBarBlankSize = 14;    // TrackBar 양옆 빈공간
        int trackBarLength = 0;        // TrackBar의 실제 길이
        int trackBarMouseX = 0;        // TrackBar에서 마우스 클릭 위치

        public Form1()
        {
            InitializeComponent();
            mp3Player = new MP3Player();

            this.KeyPreview = true;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.PowderBlue;
            trackBarLength = MP3TrackBar.Size.Width - (trackBarBlankSize * 2); // TrackBar의 실제 길이
        }

        public static Boolean Exists = false;
        public static string totalTime;
        public int j = 0;
        public int index = 0;
        public bool isIndex = true;
        public static string fileName;
        public static string lyricsFileName;
        public static string jsonFileName;
        public int leng;
        public int min, sec, milliSec;
        public int lineCount = 0;
        public static string[] lines;
        public static string jsonLines;
        public static string[,,] result = new string[500, 20, 4];
        public static double[,] baseResult = new double[500, 2];
        public int idx;
        public int lineIndex = 0;
        public static string[] lineText;
        double[,] time;
        string data1, data2, data3;
        public static Boolean is05x = false;
        double res;
        int verse = 0;
        int c = 0;
        int cell;
        int rowIndex;
        int columnIndex;
        string combo1,combo2;
        bool isCell = false;
        bool sign = true;
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] comboBox01 = { "1000", "100", "50", "10"};
            string[] comboBox02 = { "+", "-" };
            comboBox1.Items.AddRange(comboBox01);
            comboBox1.SelectedIndex = 3;
            comboBox2.Items.AddRange(comboBox02);
            comboBox2.SelectedIndex = 0;

        }

        // MP3 타이머
        private void MP3Timer_Tick(object sender, EventArgs e)
        {
            if (!mp3Player.isOpened)
                return;
            if (isScrolled == false)

                MP3TrackBar.Value = mp3Player.GetPosition();

            if (!mp3Player.loop && mp3Player.GetPosition() == mp3Player.GetLength())
                mp3Player.Stop();

            SetMusicTimer();
        }

        //노래 불러오기
        private void MusicOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "Mp3 File|*.mp3";
                open.InitialDirectory = Environment.CurrentDirectory;

                if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Form2 form2 = new Form2();
                    fileName = open.FileName;

                    mp3Player.Open(fileName);
                    MP3TrackBar.Maximum = mp3Player.GetLength();

                    MP3Timer.Enabled = true;
                    label7.Text = Path.GetFileNameWithoutExtension(fileName);
                    leng = mp3Player.GetLength();
                    min = leng / 60000;
                    sec = (leng % 60000) / 1000;
                    milliSec = (leng % 60000) % 1000;
                    // 전체 재생시간
                    label1.Text = String.Format("{0:00}:{1:00}:{2:000}", min, sec, milliSec);
                    totalTime = leng.ToString();
                    Console.Write("mp3Player Length() : " + MP3TrackBar.Maximum);
                    Console.WriteLine("trackBarLength : " + trackBarLength);
                }
            }
        }
        // 한글 가사 파일 불러오기
        private void LyricsOpen_Click(object sender, EventArgs e)
        {
            Lyrics lyrics1 = new Lyrics();

            //try
            //{
                using (OpenFileDialog open = new OpenFileDialog())
                {
                    open.Filter = "(*.txt) |*.txt|모든 파일(*.*)|*.*";
                    if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        lyricsFileName = open.FileName;
                        label8.Text = Path.GetFileNameWithoutExtension(lyricsFileName);
                        lines = File.ReadAllLines(lyricsFileName, Encoding.Default);

                        if (dataGridView1.RowCount != 0)
                        {
                            dataGridView1.Rows.Clear();
                            c = 0;
                        }

                        for (int i = 0; i < lines.Length /2 + 1; i++)
                        {
                            if (lines[i + i] == "")
                            {
                                verse = i;
                                c++;
                                Console.WriteLine("verse : " + verse);
                            }
                            else
                            {
                                if(c == 0) { 
                                dataGridView1.Rows.Add();
                                dataGridView1["Column4", i - c].Value = lines[i + i] + Environment.NewLine + lines[i + i + 1];
                                dataGridView1["Column6", i - c].Value = "Verse" + (c + 1);
                                dataGridView1["Column1", i - c].Value = i + 1 - c;
                                dataGridView1.Rows[i - c].Cells[5] = new DataGridViewButtonCell();
                                dataGridView1.Rows[i - c].Cells[5].Value = "Confirm";
                            }
                            else
                            {
                                dataGridView1.Rows.Add();
                                dataGridView1["Column4", i - c].Value = lines[i + i - c] + Environment.NewLine + lines[i + i + 1 - c];
                                dataGridView1["Column6", i - c].Value = "Verse" + (c + 1);
                                dataGridView1["Column1", i - c].Value = i + 1 - c;
                                dataGridView1.Rows[i - c].Cells[5] = new DataGridViewButtonCell();
                                dataGridView1.Rows[i - c].Cells[5].Value = "Confirm";
                            }
                            //if (c == 0)
                            //{
                            //    dataGridView1.Rows.Add();
                            //    dataGridView1.Rows.Add();
                            //    dataGridView1["Column4", (2 * i) - c].Value = lines[i] + Environment.NewLine + lines[i + 1];
                            //    dataGridView1["Column6", (2 * i) - c].Value = "Verse" + (c + 1);
                            //    dataGridView1["Column1", (2 * i) - c].Value = i + 1 - c;
                            //    dataGridView1.Rows[(2 * i) - c].Cells[5] = new DataGridViewButtonCell();
                            //    dataGridView1.Rows[(2 * i) - c].Cells[5].Value = "Confirm";
                            //}
                            //else
                            //{
                            //    dataGridView1.Rows.Add();
                            //    dataGridView1.Rows.Add();
                            //    dataGridView1["Column4", (2 * i) - (c + 1)].Value = lines[i];
                            //    dataGridView1["Column6", (2 * i) - (c + 1)].Value = "Verse" + (c + 1);
                            //    dataGridView1["Column1", (2 * i) - (c + 1)].Value = i + 1 - c;
                            //    dataGridView1.Rows[(2 * i) - (c + 1)].Cells[5] = new DataGridViewButtonCell();
                            //    dataGridView1.Rows[(2 * i) - (c + 1)].Cells[5].Value = "Confirm";
                            //}
                        }
                        }
                        //c = 0;
                        
                    }
                }
                string jsonFile = @"C:\VR_Karaoke\" +
                    Path.GetFileNameWithoutExtension(Path.GetFileName(Form1.lyricsFileName)) +
                    ".json";
                if (!System.IO.File.Exists(jsonFile))
                {
                    lyrics1.lyrics1(lines.Length, lines, baseResult, result, verse);

                }
                string payload = lyrics1.collection1.ToString();
            //}
            //catch { }
           
        }

        // 영어 가사 파일 불러 오기
        private void lyricsENFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lyrics lyrics1 = new Lyrics();

            //try
            //{
                using (OpenFileDialog open = new OpenFileDialog())
                {
                    open.Filter = "(*.txt) |*.txt|모든 파일(*.*)|*.*";
                    if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        lyricsFileName = open.FileName;
                        lines = File.ReadAllLines(lyricsFileName, Encoding.Default);

                        //if (dataGridView1.RowCount != 0)
                        //{
                        //    dataGridView1.Rows.Clear();
                        //    c = 0;
                        //}

                        for (int i = 0; i < lines.Length ; i++)
                        {
                            if (lines[i] == "")
                            {
                                verse = i;
                                c++;
                                Console.WriteLine("verse : " + verse);
                            }
                            else
                            {
                            if (c == 0)
                            {
                                    dataGridView1["Column4", (2 * i + 1) - c].Value = lines[i];
                                    dataGridView1["Column6", (2 * i + 1) - c].Value = "Verse" + (c + 1);
                                    dataGridView1["Column1", (2 * i + 1) - c].Value = i + 1 - c;
                                    dataGridView1.Rows[(2 * i + 1) - c].Cells[5] = new DataGridViewButtonCell();
                                    dataGridView1.Rows[(2 * i + 1) - c].Cells[5].Value = "Confirm";
                            }
                            else
                            {
                                    dataGridView1["Column4", (2 * i + 1) - (c + 1)].Value = lines[i];
                                    dataGridView1["Column6", (2 * i + 1) - (c + 1)].Value = "Verse" + (c + 1);
                                    dataGridView1["Column1", (2 * i + 1) - (c + 1)].Value = i + 1 - c;
                                    dataGridView1.Rows[(2 * i + 1) - (c + 1)].Cells[5] = new DataGridViewButtonCell();
                                    dataGridView1.Rows[(2 * i + 1) - (c + 1)].Cells[5].Value = "Confirm";
                            }
                        }
                        }

                    }
                }
                string jsonFile = @"C:\VR_Karaoke\" +
                    Path.GetFileNameWithoutExtension(Path.GetFileName(Form1.lyricsFileName)) +
                    ".json";
                if (!System.IO.File.Exists(jsonFile))
                {
                    lyrics1.lyrics1(lines.Length, lines, baseResult, result, verse);

                }
                string payload = lyrics1.collection1.ToString();
            //}
            //catch { }
        }


        // Json 파일 로드 클릭
        private void JsonLoad_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 form2 = new Form2();
                if(verse != 0) { 
                for (int i = 0; i < lines.Length-1; i++)
                {
                    dataGridView1["Column2", i].Value = "";
                    dataGridView1["Column3", i].Value = "";
                }
            }
            else
            { 
                for (int i = 0; i < lines.Length; i++)
                {
                    dataGridView1["Column2", i].Value = "";
                    dataGridView1["Column3", i].Value = "";
                }
            }
            index = 0;
                JsonParser parser = new JsonParser();
                using (OpenFileDialog open = new OpenFileDialog())
                {

                    open.Filter = "(*.json) |*.json|모든 파일(*.*)|*.*";
                    if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        jsonFileName = open.FileName;


                        jsonLines = System.IO.File.ReadAllText(jsonFileName);
                        time = parser.JsonTextParser(jsonLines, verse);
                        form2.WriteTextEvent += new Form2.TextEventHandler(frm2_WriteTextEvent);
                        for (int i = 0; i < time.GetLength(0); i++)
                        {
                            if (time[i, 0] == 0)
                            {
                                continue;
                            }

                            else
                            {
                                TimeSpan t1 = TimeSpan.FromMilliseconds(time[i, 0]);
                                dataGridView1["Column2", i].Value = string.Format("{0:D2}:{1:D2}:{2:D3}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                                //dataGridView1["Column2", i].Value = time[i, 0];
                                TimeSpan t2 = TimeSpan.FromMilliseconds(time[i, 1]);
                                dataGridView1["Column3", i].Value = string.Format("{0:D2}:{1:D2}:{2:D3}", t2.Minutes, t2.Seconds, t2.Milliseconds);
                                //dataGridView1["Column3", i].Value = time[i, 1];
                                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                                sw.Start();
                                form2.parserReceive(JsonParser.detailtime, i);
                                sw.Stop();
                                index++;
                            }
                        }
                    }
                    baseResultSave();
                }
            }
            catch { }
        }


        //Convert 버튼
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 form2 = new Form2();
            if (this.combo2 == "-")
            {
                sign = false;
            }
            else
            {
                sign = true;
            }
            int sum = 0;
            index = 0;
            sum = Int32.Parse(textBox1.Text) * 60000 + Int32.Parse(textBox2.Text) * 1000 + Int32.Parse(textBox3.Text);
                JsonParser parser = new JsonParser();
                //jsonLines = System.IO.File.ReadAllText(jsonFileName);
                //time = parser.JsonTextParser(jsonLines, verse);
                //form2.WriteTextEvent += new Form2.TextEventHandler(frm2_WriteTextEvent);
                for (int i = 0; i < time.GetLength(0); i++)
                {
                    if (sign)
                    {
                        jsonLines = System.IO.File.ReadAllText(jsonFileName);
                        time = parser.JsonTextParser(jsonLines, verse);
                        form2.WriteTextEvent += new Form2.TextEventHandler(frm2_WriteTextEvent);
                        TimeSpan t1 = TimeSpan.FromMilliseconds(time[i, 0] + sum);
                        TimeSpan t2 = TimeSpan.FromMilliseconds(time[i, 1] + sum);
                        dataGridView1["Column2", i].Value = string.Format("{0:D2}:{1:D2}:{2:D3}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                        dataGridView1["Column3", i].Value = string.Format("{0:D2}:{1:D2}:{2:D3}", t2.Minutes, t2.Seconds, t2.Milliseconds);
                        form2.parserReceive2(JsonParser.detailtime, i, sum, sign);
                }
                    else
                    {
                        jsonLines = System.IO.File.ReadAllText(jsonFileName);
                        time = parser.JsonTextParser(jsonLines, verse);
                        form2.WriteTextEvent += new Form2.TextEventHandler(frm2_WriteTextEvent);
                        TimeSpan t1 = TimeSpan.FromMilliseconds(time[i, 0] - sum);
                        TimeSpan t2 = TimeSpan.FromMilliseconds(time[i, 1] - sum);
                        dataGridView1["Column2", i].Value = string.Format("{0:D2}:{1:D2}:{2:D3}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                        dataGridView1["Column3", i].Value = string.Format("{0:D2}:{1:D2}:{2:D3}", t2.Minutes, t2.Seconds, t2.Milliseconds);
                        form2.parserReceive2(JsonParser.detailtime, i, sum, sign);
                }

                }
           
            textBox1.Text = "00";
            textBox2.Text = "00";
            textBox3.Text = "000";
        }
            catch { }
        }


        //DataGridView1 의 데이터를 baseResult배열에 저장
        public void baseResultSave()
        {
            try
            {
                if (verse != 0)
                {
                    for (int i = 0; i < lines.Length - 1; i++)
                    {
                        if (dataGridView1["Column2", i].Value == null)
                        {
                            continue;
                        }
                        string[] array1 = dataGridView1["Column2", i].Value.ToString().Split(':');
                        double number1 = Int32.Parse(array1[0]) * 60 + Int32.Parse(array1[1]) + Int32.Parse(array1[2]) * 0.001;
                        baseResult[i, 0] = number1;
                        string[] array2 = dataGridView1["Column3", i].Value.ToString().Split(':');
                        double number2 = Int32.Parse(array2[0]) * 60 + Int32.Parse(array2[1]) + Int32.Parse(array2[2]) * 0.001;
                        baseResult[i, 1] = number2;
                    }
                }
                else
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (dataGridView1["Column2", i].Value == null)
                        {
                            continue;
                        }
                        string[] array1 = dataGridView1["Column2", i].Value.ToString().Split(':');
                        double number1 = Int32.Parse(array1[0]) * 60 + Int32.Parse(array1[1]) + Int32.Parse(array1[2]) * 0.001;
                        baseResult[i, 0] = number1;
                        string[] array2 = dataGridView1["Column3", i].Value.ToString().Split(':');
                        double number2 = Int32.Parse(array2[0]) * 60 + Int32.Parse(array2[1]) + Int32.Parse(array2[2]) * 0.001;
                        baseResult[i, 1] = number2;
                    }
                }

            }
            catch { }
        }
        //DataGridView2 의 데이터를 result 배열에 저장
        public void DetailResultSave()
        {

        }
        //Json 파일 save
        private void save_button_Click(object sender, EventArgs e)
        {
            try
            {
                Lyrics lyrics1 = new Lyrics();

                baseResultSave();
                lyrics1.lyrics1(lines.Length, lines, baseResult, result, verse);
                MessageBox.Show("Save completed!");

            }
            catch { }

        }
        // Form2로 부터 데이터 받아오는 메소드
        void frm2_WriteTextEvent(string[,,] data, int index, int line)
        {
            try
            {
                Form2 form2 = new Form2();
                Lyrics lyrics1 = new Lyrics();
                for (int i = 0; i < index; i++)
                {
                    result[line, i, 0] = data[line, i, 0];
                    //result[line, i, 1] = data[line, i, 1];
                    result[line, i, 2] = data[line, i, 2];
                    result[line, i, 1] = data[line, i, 1];
                }
                baseResultSave();
                //DataGridView2 의 행의 개수 0 : start , 1: End  2: text 3: 행수
                result[line, 0, 3] = index.ToString();
                lyrics1.lyrics1(lines.Length, lines, baseResult, result, verse);
            }
            catch { }
        }


        // confirm 버튼 클릭시 
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Form2 form2 = new Form2();
                lineIndex = dataGridView1.SelectedCells[0].RowIndex;
                form2.WriteTextEvent += new Form2.TextEventHandler(frm2_WriteTextEvent);

                if (this.dataGridView1.CurrentCell.ColumnIndex == 5)        //confirm 버튼을 누를시에만 동작
                {
                    //confirm 수정
                    if (result[lineIndex, 0, 3] != null)
                    {
                        int idx = Int32.Parse(result[lineIndex, 0, 3]);
                        form2.dataReceive(result, idx, lineIndex);
                    }
                    string startDetail = dataGridView1.Rows[lineIndex].Cells[1].Value.ToString();
                    string[] arr1 = startDetail.Split(':');
                    int startDetailNum = Int32.Parse(arr1[0]) * 60000 + Int32.Parse(arr1[1]) * 1000 + Int32.Parse(arr1[2]);

                    string endDetail = dataGridView1.Rows[lineIndex].Cells[2].Value.ToString();
                    string[] arr2 = endDetail.Split(':');
                    int endDetailNum = Int32.Parse(arr2[0]) * 60000 + Int32.Parse(arr2[1]) * 1000 + Int32.Parse(arr2[2]); ;

                    int detailTotal = endDetailNum - startDetailNum;

                    form2.received1(startDetailNum, endDetailNum, lineIndex);
                    form2.received2((string)dataGridView1.Rows[lineIndex].Cells[3].Value, startDetail, endDetail, detailTotal);

                    form2.Show();
                }
            }
            catch { MessageBox.Show("Please open the music file & Time Check!"); }
        }


        // Modify 버튼 클릭
        private void Modify_button_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = false;
        }

        // Complete 버튼 클릭
        private void Complete_button_Click(object sender, EventArgs e)
        {
            Lyrics lyrics1 = new Lyrics();
            try
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] array1 = dataGridView1["Column2", i].Value.ToString().Split(':');
                    double number1 = Int32.Parse(array1[0]) * 60 + Int32.Parse(array1[1]) + Int32.Parse(array1[2]) * 0.001;
                    baseResult[i, 0] = number1;
                    string[] array2 = dataGridView1["Column2", i].Value.ToString().Split(':');
                    double number2 = Int32.Parse(array2[0]) * 60 + Int32.Parse(array2[1]) + Int32.Parse(array2[2]) * 0.001;
                    baseResult[i, 1] = number2;
                }
            }
            catch { }
            dataGridView1.ReadOnly = true;
            MessageBox.Show("Modifications completed!");
        }



        //Start 버튼
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Lyrics lyrics1 = new Lyrics();

                dataGridView1["Column2", index].Value = label4.Text;

                string[] array = dataGridView1["Column2", index].Value.ToString().Split(':');
                double number = Int32.Parse(array[0]) * 60 + Int32.Parse(array[1]) + Int32.Parse(array[2]) * 0.001;

                baseResult[index, 0] = number;

                isIndex = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] array1 = dataGridView1["Column2", index].Value.ToString().Split(':');
                    string[] array2 = dataGridView1["Column3", index].Value.ToString().Split(':');
                    double number1 = Int32.Parse(array[0]) * 60 + Int32.Parse(array[1]) + Int32.Parse(array[2]) * 0.001;
                    double number2 = Int32.Parse(array[0]) * 60 + Int32.Parse(array[1]) + Int32.Parse(array[2]) * 0.001;

                    baseResult[index, 0] = number;
                    baseResult[index, 1] = number;
                }
                lyrics1.lyrics1(lines.Length, lines, baseResult, result, verse);

                string payload = lyrics1.collection1.ToString();
            }
            catch { }
        }

        //End 버튼
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                Lyrics lyrics1 = new Lyrics();
                lineCount++;

                dataGridView1["Column3", index].Value = label4.Text;

                string[] array = dataGridView1["Column3", index].Value.ToString().Split(':');
                double number = Int32.Parse(array[0]) * 60 + Int32.Parse(array[1]) + Int32.Parse(array[2]) * 0.001;
                baseResult[index, 1] = number;
                index++;
                isIndex = true;
                Console.WriteLine(index);
                lyrics1.lyrics1(lines.Length, lines, baseResult, result, verse);
                string payload = lyrics1.collection1.ToString();
            }
            catch { }
        }

        //Revert 버튼
        private void button4_Click(object sender, EventArgs e)
        {
            Lyrics lyrics1 = new Lyrics();
            if (isIndex)
            {
                if (index > 0)
                {
                    dataGridView1["Column3", index - 1].Value = "";
                    isIndex = false;
                    index--;
                    baseResult[index, 1] = 0;
                    lyrics1.lyrics1(lines.Length, lines, baseResult, result, verse);
                }
            }
            else
            {
                dataGridView1["Column2", index].Value = "";
                isIndex = true;
                baseResult[index, 0] = 0;
                lyrics1.lyrics1(lines.Length, lines, baseResult, result, verse);
                if (lineCount > 0) { lineCount--; }
            }
        }


        // 음악 재생 버튼
        private void button5_Click(object sender, EventArgs e)
        {

            mp3Player.Play();

        }

        //음악 일시 정지 버튼
        private void button6_Click(object sender, EventArgs e)
        {
            mp3Player.Pause();
        }

        // 음악 정지 버튼
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                mp3Player.Stop();
                index = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1["Column2", i].Value = "";
                    dataGridView1["Column3", i].Value = "";
                }
            }
            catch { }
        }


        // TrackBar MouseDown Event
        private void MP3TrackBar_MouseDown(object sender, MouseEventArgs e)
        {
            isScrolled = true;
            trackBarMouseX = e.X - trackBarBlankSize;     // 마우스 클릭 좌표
            SetPositionByMouse(trackBarMouseX);
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

                mp3Player.Seek(MP3TrackBar.Value);

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
            MP3TrackBar.Value = (int)(rate * (float)(MP3TrackBar.Maximum - MP3TrackBar.Minimum));
        }

        // 2배속도
        private void X2Speed_Click(object sender, EventArgs e)
        {
            mp3Player.Speedx2();
        }

        // 1배속도
        private void button1_Click(object sender, EventArgs e)
        {
            is05x = false;
            mp3Player.Speedx1();
        }

        // 05배속
        private void button2_Click(object sender, EventArgs e)
        {
            is05x = true;
            mp3Player.Speedx05();
        }





        // 1초 앞으로 가는 버튼
        public void pre_button_Click(object sender, EventArgs e)
        {
            mp3Player.Seek(mp3Player.GetPosition() - 1000);
            mp3Player.Play();
        }

        //1초 뒤로 가는 버튼
        public void after_button_Click(object sender, EventArgs e)
        {
            mp3Player.Seek(mp3Player.GetPosition() + 1000);
            mp3Player.Play();
        }

        //plus
        private void button9_Click(object sender, EventArgs e)
        {
            //try
            //{
                if (isCell) { 
                cell += Convert.ToInt32(this.combo1);
                TimeSpan t1 = TimeSpan.FromMilliseconds(cell);
                dataGridView1.Rows[rowIndex].Cells[columnIndex].Value = string.Format("{0:D2}:{1:D2}:{2:D3}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                }
            //}
            //catch { }
        }

        //minus
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (isCell) { 
                cell -= Convert.ToInt32(this.combo1);
                TimeSpan t1 = TimeSpan.FromMilliseconds(cell);
                dataGridView1.Rows[rowIndex].Cells[columnIndex].Value = string.Format("{0:D2}:{1:D2}:{2:D3}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                }
            }
            catch { }
        }


        // close 버튼
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 음악 label 클릭 시 음악파일 불러오기
        private void label7_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "Mp3 File|*.mp3";
                open.InitialDirectory = Environment.CurrentDirectory;

                if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Form2 form2 = new Form2();
                    fileName = open.FileName;

                    mp3Player.Open(fileName);
                    MP3TrackBar.Maximum = mp3Player.GetLength();

                    MP3Timer.Enabled = true;
                    label7.Text = Path.GetFileNameWithoutExtension(fileName);
                    leng = mp3Player.GetLength();
                    min = leng / 60000;
                    sec = (leng % 60000) / 1000;
                    milliSec = (leng % 60000) % 1000;
                    // 전체 재생시간
                    label1.Text = String.Format("{0:00}:{1:00}:{2:000}", min, sec, milliSec);
                    totalTime = leng.ToString();
                    Console.Write("mp3Player Length() : " + MP3TrackBar.Maximum);
                    Console.WriteLine("trackBarLength : " + trackBarLength);
                }
            }
        }

       
        // 가사 label 클릭 시 파일불러오기
        private void label8_Click(object sender, EventArgs e)
        {
            Lyrics lyrics1 = new Lyrics();

            try
            {
                using (OpenFileDialog open = new OpenFileDialog())
                {
                    open.Filter = "(*.txt) |*.txt|모든 파일(*.*)|*.*";
                    if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        lyricsFileName = open.FileName;
                        label8.Text = Path.GetFileNameWithoutExtension(lyricsFileName);
                        lines = File.ReadAllLines(lyricsFileName, Encoding.Default);

                        if (dataGridView1.RowCount != 0)
                        {
                            dataGridView1.Rows.Clear();
                            c = 0;
                        }

                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i] == "")
                            {
                                verse = i;
                                c++;
                                Console.WriteLine("verse : " + verse);
                            }
                            else
                            {
                                dataGridView1.Rows.Add();
                                dataGridView1["Column4", i - c].Value = lines[i];
                                dataGridView1["Column6", i - c].Value = "Verse" + (c + 1);
                                dataGridView1["Column1", i - c].Value = i + 1 - c;
                                dataGridView1.Rows[i - c].Cells[5] = new DataGridViewButtonCell();
                                dataGridView1.Rows[i - c].Cells[5].Value = "Confirm";
                            }
                        }

                    }
                }
                string jsonFile = @"C:\VR_Karaoke\" +
                    Path.GetFileNameWithoutExtension(Path.GetFileName(Form1.lyricsFileName)) +
                    ".json";
                if (!System.IO.File.Exists(jsonFile))
                {
                    lyrics1.lyrics1(lines.Length, lines, baseResult, result, verse);

                }
                string payload = lyrics1.collection1.ToString();
            }
            catch { }
        }



        // dataGridView1   선택
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
                if(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && e.ColumnIndex < 3) {
                isCell = true;
                string[] array = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Split(':');
                cell = Int32.Parse(array[0]) * 60000 + Int32.Parse(array[1]) * 1000 + Int32.Parse(array[2]);
                rowIndex = e.RowIndex;
                columnIndex = e.ColumnIndex;

                cell = Int32.Parse(array[0]) * 60000 + Int32.Parse(array[1]) * 1000 + Int32.Parse(array[2]);
                }
                else
                {
                isCell = false;
                }
            }
            catch { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                this.combo1 = comboBox1.SelectedItem as string;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex >= 0)
            {
                this.combo2 = comboBox2.SelectedItem as string;
            }
        }


        //키보드 
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
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

        //음악 타이머 메소드
        private void SetMusicTimer()
        {
            if (mp3Player.isOpened)
            {
                if (is05x)
                {
                    //TimeSpan t1 = TimeSpan.FromMilliseconds(mp3Player.GetPosition() * 1.11111);
                    TimeSpan t1 = TimeSpan.FromMilliseconds(mp3Player.GetPosition() * 1.00001 + 100);
                    label4.Text = string.Format("{0:D2}:{1:D2}:{2:D3}", t1.Minutes, t1.Seconds, t1.Milliseconds);
                }
                else
                {
                    TimeSpan t2 = TimeSpan.FromMilliseconds(mp3Player.GetPosition());
                    label4.Text = string.Format("{0:D2}:{1:D2}:{2:D3}", t2.Minutes, t2.Seconds, t2.Milliseconds);
                }
            }
        }
    }
}

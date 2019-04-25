using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Json;
using System.IO;
using System.Windows.Forms;
namespace WindowsFormsApp2
{
    public class JsonParser : Lyrics
    {
        public double[,] time;
        public static string[,,] detailtime;
        public int i;
        public JsonArrayCollection col2 = new JsonArrayCollection();
        public JsonObjectCollection item = new JsonObjectCollection();
        public JsonArrayCollection col3 = new JsonArrayCollection();
        public double[,] JsonTextParser(string strData, int verse)
        {


            try
            {
                if (verse != 0)
                {
                    JsonTextParser parser = new JsonTextParser();
                    JsonObjectCollection obj1 = (JsonObjectCollection)parser.Parse(strData);
                    JsonArrayCollection col1 = new JsonArrayCollection();
                    JsonArrayCollection col2 = new JsonArrayCollection();
                    JsonArrayCollection col3 = new JsonArrayCollection();
                    JsonArrayCollection col4 = new JsonArrayCollection();
                    JsonArrayCollection col5 = new JsonArrayCollection();

                    col1 = (JsonArrayCollection)obj1["lyricsVerses"];

                    JsonObjectCollection element1 = (JsonObjectCollection)col1[0];

                    col2 = (JsonArrayCollection)element1["lines"];

                    JsonObjectCollection element2 = (JsonObjectCollection)col1[1];
                    col4 = (JsonArrayCollection)element2["lines"];

                    time = new double[col2.Count + col4.Count, 2];

                    // detailtime [ col2 : lines(가사 한줄) , col3 : Form2에 저장하는 한글자 라인 ,  0 : charStart  1: charEnd  2: text 3: col3.Count
                    detailtime = new string[col2.Count + col4.Count, 20, 4];
                    for (i = 0; i < col2.Count; i++)
                    {
                        JsonObjectCollection items = (JsonObjectCollection)col2[i];

                        time[i, 0] = double.Parse((string)items["lineStart"].GetValue().ToString()) * 1000;
                        time[i, 1] = double.Parse((string)items["lineEnd"].GetValue().ToString()) * 1000;
                        Console.WriteLine(time[i, 0] + " , " + time[i, 1]);
                        col3 = (JsonArrayCollection)items["charEvents"];
                        Console.WriteLine(col3.Count);

                        for (int j = 0; j < col3.Count; j++)
                        {
                            JsonObjectCollection item = (JsonObjectCollection)col3[j];
                            detailtime[i, j, 0] = item["charStart"].GetValue().ToString();
                            //detailtime[i, j, 1] = item["charEnd"].GetValue().ToString();
                            detailtime[i, j, 1] = ((JsonStringValue)item["textEn"]).Value;
                            detailtime[i, j, 2] = ((JsonStringValue)item["text"]).Value;

                        }
                        detailtime[i, 0, 3] = col3.Count.ToString();

                    }




                    for (i = 0; i < col4.Count; i++)
                    {
                        JsonObjectCollection items = (JsonObjectCollection)col4[i];

                        time[i + verse, 0] = double.Parse((string)items["lineStart"].GetValue().ToString()) * 1000;
                        time[i + verse, 1] = double.Parse((string)items["lineEnd"].GetValue().ToString()) * 1000;
                        //Console.WriteLine(time[i, 0] + " , " + time[i, 1]);
                        col3 = (JsonArrayCollection)items["charEvents"];
                        Console.WriteLine(col3.Count);

                        for (int j = 0; j < col3.Count; j++)
                        {
                            JsonObjectCollection item = (JsonObjectCollection)col3[j];
                            detailtime[i + verse, j, 0] = item["charStart"].GetValue().ToString();
                            //detailtime[i + verse, j, 1] = item["charEnd"].GetValue().ToString();
                            detailtime[i, j, 1] = ((JsonStringValue)item["textEn"]).Value;
                            detailtime[i + verse, j, 2] = ((JsonStringValue)item["text"]).Value;
                        }
                        detailtime[i + verse, 0, 3] = col3.Count.ToString();
                    }
                }
                else
                {


                    JsonTextParser parser = new JsonTextParser();
                    JsonObjectCollection obj1 = (JsonObjectCollection)parser.Parse(strData);
                    JsonArrayCollection col1 = new JsonArrayCollection();
                    JsonArrayCollection col2 = new JsonArrayCollection();
                    JsonArrayCollection col3 = new JsonArrayCollection();

                    col1 = (JsonArrayCollection)obj1["lyricsVerses"];

                    JsonObjectCollection element = (JsonObjectCollection)col1[0];

                    col2 = (JsonArrayCollection)element["lines"];
                    time = new double[col2.Count, 2];

                    Console.WriteLine(time.GetLength(0));

                    detailtime = new string[col2.Count, 20, 4];
                    // detailtime [ col2 : lines(가사 한줄) , col3 : Form2에 저장하는 한글자 라인 ,  0 : charStart  1: charEnd  2: text 3: col3.Count
                    for (i = 0; i < col2.Count; i++)
                    {
                        JsonObjectCollection items = (JsonObjectCollection)col2[i];

                        Console.WriteLine("col2.Count : " + col1.Count);
                        time[i, 0] = double.Parse((string)items["lineStart"].GetValue().ToString()) * 1000;
                        time[i, 1] = double.Parse((string)items["lineEnd"].GetValue().ToString()) * 1000;
                        Console.WriteLine(time[i, 0] + " , " + time[i, 1]);
                        col3 = (JsonArrayCollection)items["charEvents"];
                        Console.WriteLine(col3.Count);

                        for (int j = 0; j < col3.Count; j++)
                        {
                            JsonObjectCollection item = (JsonObjectCollection)col3[j];
                            detailtime[i, j, 0] = item["charStart"].GetValue().ToString();
                            //detailtime[i, j, 1] = item["charEnd"].GetValue().ToString();
                            detailtime[i, j, 1] = ((JsonStringValue)item["textEn"]).Value;
                            detailtime[i, j, 2] = ((JsonStringValue)item["text"]).Value;
                        }
                        detailtime[i, 0, 3] = col3.Count.ToString();



                    }

                }
            }
            catch { }
            return time;
        }

    }
}

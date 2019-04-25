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
    public class Lyrics
    {



        public JsonObjectCollection collection1 = new JsonObjectCollection();
        public JsonObjectCollection collection2 = new JsonObjectCollection();
        public JsonObjectCollection collection3 = new JsonObjectCollection();
        public JsonObjectCollection collection4 = new JsonObjectCollection();
        public JsonObjectCollection collection5 = new JsonObjectCollection();
        public JsonArrayCollection arr = new JsonArrayCollection();
        public JsonArrayCollection arr2 = new JsonArrayCollection();
        public JsonArrayCollection arr3 = new JsonArrayCollection();

        public void lyrics1(int lines, string[] lineText, double[,] baseResult, string[,,] result, int verse)
        {


            if (verse != 0)
            {
                collection1.Add(new JsonStringValue("songNum", "120398"));
                collection1.Add(new JsonStringValue("title", ""));
                collection1.Add(new JsonStringValue("singer", ""));
                collection1.Add(new JsonStringValue("songWriter", ""));
                collection1.Add(new JsonStringValue("composer", ""));
                collection1.Add(new JsonStringValue("artist", ""));


                arr = new JsonArrayCollection("lyricsVerses");

                collection2 = new JsonObjectCollection();
                collection2.Add(new JsonNumericValue("verseStart", baseResult[0, 0]));
                collection2.Add(new JsonNumericValue("verseEnd", baseResult[verse - 1, 1]));



                arr2 = new JsonArrayCollection("lines");

                for (int i = 0; i < verse / 2; i++)
                {
                    collection3 = new JsonObjectCollection();
                    collection3.Add(new JsonStringValue("lineText", lineText[i + i]));
                    collection3.Add(new JsonStringValue("lineTextEn", lineText[i + i + 1]));
                    collection3.Add(new JsonNumericValue("lineStart", baseResult[i, 0]));
                    collection3.Add(new JsonNumericValue("lineEnd", baseResult[i, 1]));
                    collection3.Add(new JsonStringValue("gender", ""));

                    arr3 = new JsonArrayCollection("charEvents");
                    if (Form1.result[i, 0, 3] != null)
                    {
                        for (int k = 0; k < Int32.Parse(Form1.result[i, 0, 3]); k++)
                        {

                            collection5 = new JsonObjectCollection();
                            collection5.Add(new JsonNumericValue("charStart", double.Parse(result[i, k, 0])));
                            //collection5.Add(new JsonNumericValue("charEnd", double.Parse(result[i, k, 1])));
                            collection5.Add(new JsonStringValue("text", result[i, k, 2]));
                            collection5.Add(new JsonStringValue("textEn" , result[i, k, 1]));
                            arr3.Add(collection5);
                        }
                    }
                    collection3.Add(arr3);
                    arr2.Add(collection3);

                }
                collection2.Add(arr2);
                arr.Add(collection2);


                collection2 = new JsonObjectCollection();
                collection2.Add(new JsonNumericValue("verseStart", baseResult[verse, 0]));
                collection2.Add(new JsonNumericValue("verseEnd", baseResult[lines - 2, 1]));



                arr2 = new JsonArrayCollection("lines");
                //for (int i = verse + 1; i < lines / 2; i++)
                for (int i = verse + 1; i < lines / 2 + 1; i++)
                {
                    collection4 = new JsonObjectCollection();
                    collection4.Add(new JsonStringValue("lineText", lineText[i + i - 1]));
                    collection4.Add(new JsonStringValue("lineTextEn", lineText[i + i]));
                    collection4.Add(new JsonNumericValue("lineStart", baseResult[i - 1, 0]));
                    collection4.Add(new JsonNumericValue("lineEnd", baseResult[i - 1, 1]));
                    collection4.Add(new JsonStringValue("gender", ""));

                    arr3 = new JsonArrayCollection("charEvents");
                    if (Form1.result[i - 1, 0, 3] != null)
                    {
                        for (int k = 0; k < Int32.Parse(Form1.result[i - 1, 0, 3]); k++)
                        {
                            collection5 = new JsonObjectCollection();
                            collection5.Add(new JsonNumericValue("charStart", double.Parse(result[i - 1, k, 0])));
                            //collection5.Add(new JsonNumericValue("charEnd", double.Parse(result[i - 1, k, 1])));
                            collection5.Add(new JsonStringValue("text", result[i - 1, k, 2]));
                            collection5.Add(new JsonStringValue("textEn", result[i -1, k, 1]));
                            arr3.Add(collection5);
                        }
                    }
                    collection4.Add(arr3);
                    arr2.Add(collection4);
                }
                collection2.Add(arr2);
                arr.Add(collection2);
                collection1.Add(arr);
            }
            else
            {


                collection1.Add(new JsonStringValue("songNum", "120398"));
                collection1.Add(new JsonStringValue("title", ""));
                collection1.Add(new JsonStringValue("singer", ""));
                collection1.Add(new JsonStringValue("songWriter", ""));
                collection1.Add(new JsonStringValue("composer", ""));
                collection1.Add(new JsonStringValue("artist", ""));


                arr = new JsonArrayCollection("lyricsVerses");

                collection2 = new JsonObjectCollection();
                collection2.Add(new JsonNumericValue("verseStart", baseResult[0, 0]));
                collection2.Add(new JsonNumericValue("verseEnd", baseResult[lines - 1, 1]));


                arr2 = new JsonArrayCollection("lines");


                for (int i = 0; i < lines; i++)
                {
                    collection3 = new JsonObjectCollection();
                    collection3.Add(new JsonStringValue("lineText", lineText[i]));
                    collection3.Add(new JsonNumericValue("lineStart", baseResult[i, 0]));
                    collection3.Add(new JsonNumericValue("lineEnd", baseResult[i, 1]));
                    collection3.Add(new JsonStringValue("gender", ""));

                    arr3 = new JsonArrayCollection("charEvents");
                    if (Form1.result[i, 0, 3] != null)
                    {
                        for (int k = 0; k < Int32.Parse(Form1.result[i, 0, 3]); k++)
                        {

                            collection5 = new JsonObjectCollection();
                            collection5.Add(new JsonNumericValue("charStart", double.Parse(result[i, k, 0])));
                            //collection5.Add(new JsonNumericValue("charEnd", double.Parse(result[i, k, 1])));
                            collection5.Add(new JsonStringValue("text", result[i, k, 2]));
                            collection5.Add(new JsonStringValue("textEn", result[i, k, 1]));
                            arr3.Add(collection5);
                        }
                    }
                    collection3.Add(arr3);
                    arr2.Add(collection3);

                }
                collection2.Add(arr2);
                arr.Add(collection2);
                collection1.Add(arr);

            }


            string payload = collection1.ToString();
            string path = @"C:\VR_Karaoke";
            string jsonFile = @"C:\VR_Karaoke\" +
                Path.GetFileNameWithoutExtension(Path.GetFileName(Form1.lyricsFileName)) +
                ".json";


            try
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
                di.Create();
            }
            catch { }
            StreamWriter textWrite = File.CreateText(jsonFile);
            textWrite.Write(payload);
            textWrite.Dispose();


        }
    }
}

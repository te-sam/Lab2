using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace чб
{
    public partial class Form1 : Form
    {
        string File_name = "";
        int bright = 0;

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "JPG files (*.JPG)|*.jpg|" + "BMP files (*.BMP)|*.bmp|" + "PNG files (*.PNG)|*.png";
            saveFileDialog1.Filter = "JPG files (*.JPG)|*.jpg|" + "BMP files (*.BMP)|*.bmp|" + "PNG files (*.PNG)|*.png";
            panel2.Visible = false;
            panel3.Visible = false;
            radioButton1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            if (pictureBox1.Image != null) // если изображение в pictureBox1 имеется
            {
                Bitmap input = new Bitmap(pictureBox1.Image);//получение изображения
                Bitmap ouput = new Bitmap(input.Width, input.Height);// вывод изображения
                                                                        // перебираем в циклах все пиксели исходного изображения
                for (int j = 0; j < input.Height; j++)
                    for (int i = 0; i < input.Width; i++)    // получаем (i, j) пиксель
                    {

                        UInt32 pixel = (UInt32)(input.GetPixel(i, j).ToArgb());
                        // получаем компоненты цветов пикселя путем побитового умножения значения пикселя на битовую маску для соответствующего цвета с последующим сдвигом вправо
                        float R = (float)((pixel & 0x00FF0000) >> 16); // красный 
                        float G = (float)((pixel & 0x0000FF00) >> 8); // зеленый
                        float B = (float)(pixel & 0x000000FF); // синий
                                                                // делаем цвет черно-белым (оттенки серого) - находим среднее арифметическое
                        R = G = B = (R + G + B) / 3.0f; // собираем новый пиксель по частям (по каналам) с использованием операций сдвига и последующим логическим сложением всех компонент
                        UInt32 newPixel = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);
                        ouput.SetPixel(i, j, Color.FromArgb((int)newPixel));
                        progressBar1.Value += (progressBar1.Value != progressBar1.Maximum) ? 1 : 0;
                    }
                pictureBox2.Image = ouput;
            }
            else
            {
                MessageBox.Show("Загрузите изображение!");
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            File_name = openFileDialog1.FileName;
            pictureBox1.Image = new Bitmap(File_name);
        }


        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            File_name = saveFileDialog1.FileName;
            pictureBox2.Image.Save(File_name);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void оттенокСерогоToolStripMenuItem_Click(object sender, EventArgs e)
        {
           panel2.Visible = true;
        }

        private void яркостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }

        private void обАвтореToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программу создал студент группы ЭИСБ-24 Самохвалов А.Ю");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)  // если изображение в pictureBox1 существует
            {
                progressBar1.Value = 0;
                bright = trackBar1.Value;
                Bitmap input = new Bitmap(pictureBox1.Image);//получение картинки
                Bitmap ouput = new Bitmap(input.Width, input.Height);//Конечная картинка

                Color c;

                if (radioButton1.Checked)
                {
                    bright = (bright < -255) ? -255 : (bright > 255) ? 255 : bright; //проверка выхода за пределы и присваивание значение track bar1
                    for (int y = 0; y < input.Height; y++)
                        for (int x = 0; x < input.Width; x++)
                        {
                            c = input.GetPixel(x, y); //цвет текущего пикселя
                                                      //увеличиваем/уменьшаем значение каждой компоненты RGB
                            int cR = c.R + bright;
                            int cG = c.G + bright;
                            int cB = c.B + bright;

                            //то, что ниже, нужно, чтобы избежать значений больше 255 или меньше 0
                            cR = (cR < 0) ? 1 : (cR > 255) ? 255 : cR;
                            cG = (cG < 0) ? 1 : (cG > 255) ? 255 : cG;
                            cB = (cB < 0) ? 1 : (cB > 255) ? 255 : cB;
                            ouput.SetPixel(x, y, Color.FromArgb((byte)cR, (byte)cG, (byte)cB));
                            progressBar1.Value += (progressBar1.Value != progressBar1.Maximum) ? 1 : 0;
                        }
                    pictureBox2.Image = ouput;
                }

                if (radioButton2.Checked)
                {
                    progressBar1.Value = 0;
                    {
                        double contrastLevel = Math.Pow((100.0 + trackBar1.Value) / 100.0, 2);
                        double cR = 0, cG = 0, cB = 0;

                        for (int y = 0; y < input.Height; y++)
                        {
                            for (int x = 0; x < input.Width; x++)
                            {
                                c = input.GetPixel(x, y);

                                cR = ((((c.R / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;
                                cG = ((((c.G / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;
                                cB = ((((c.B / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;

                                cR = (cR < 0) ? 1 : (cR > 255) ? 255 : cR;
                                cG = (cG < 0) ? 1 : (cG > 255) ? 255 : cG;
                                cB = (cB < 0) ? 1 : (cB > 255) ? 255 : cB;

                                ouput.SetPixel(x, y, Color.FromArgb((byte)cR, (byte)cG, (byte)cB));
                                progressBar1.Value += (progressBar1.Value != progressBar1.Maximum) ? 1 : 0;
                            }
                        }
                        pictureBox2.Image = ouput;
                    }
                }
            }
            else
            {
                MessageBox.Show("Зазгрузите изображение!");
            }
        }

        private void алгоритмПреобразованияВЧбToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Каждый пиксель изображения формируется при помощи сочетания четырех каналов: ARGB (Alpha, Red, Green, Blue), альфа-канала, красного, зеленого и синего." +
                " Альфа-канал отвечает за прозрачность пикселя (100% – пиксель полностью непрозрачный, 0% – полностью прозрачный)." +
                " Сочетание значений RGB каналов определяет цвет пикселя." + " Для того, чтобы преобразовать цветное изображение в черно - белое, нужно найти среднее арифметическое значение R, G и B каналов пикселя и затем это значение присвоить RGB каналам этого же пикселя(то есть оно будет одинаковое).Альфа - канал оставляем без изменений" +
"Такую операцию нужно проделать с каждым пикселем изображения");
        }

        private void алгоритмИзмененияЯркостиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Для каждого цветового канала рассчитывается новое значение прибавлением к текущему значению значения коэффициента."+
              "Контролируем переполнение переменных: ЕСЛИ I < 0, ТО I = 0, ЕСЛИ I > 255, ТО I = 255, где I – соответственно R, G, B пикселя.");
        }
    }
}

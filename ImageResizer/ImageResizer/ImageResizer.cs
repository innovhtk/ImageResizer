using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageResizer
{
    public partial class ImageResizer : Form
    {
        public string Dir { get; set; }
        public string ModDir { get; set; }
        public int Indice { get; set; }
        public ImageResizer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label1.Text = folderBrowserDialog1.SelectedPath;
                Dir = folderBrowserDialog1.SelectedPath;
                string[] parts = Dir.Split('\\');
                ModDir = "";
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    ModDir += parts[i] + "\\";
                }
                ModDir += parts[parts.Length - 1] + "_Reducido";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int indice = 2;
            try
            {
                indice = Convert.ToInt32(textBox1.Text);
            }
            catch (Exception)
            {
                Console.WriteLine("Escriba un número en el ídice de reducción");
                return;
            }
            Indice = indice;
            if (!Directory.Exists(ModDir))
            {
                Directory.CreateDirectory(ModDir);
            }
            Cursor.Current = Cursors.WaitCursor;
            CheckDirectory(Dir, ModDir);
            Cursor.Current = Cursors.Default;
            Console.WriteLine("Se creó una copia de la carpeta con imágenes reducidas en " + ModDir);
        }

        private void CheckDirectory(string Dir, string ModDir)
        {
            List<string> directories = new List<string>();
            directories = Directory.EnumerateDirectories(Dir).ToList<string>();
            if (directories.Count > 0)
            {
                foreach (string dir in directories)
                {
                    string ModSubdir = dir.Replace(Dir, ModDir);
                    if (!Directory.Exists(ModSubdir))
                    {
                        Directory.CreateDirectory(ModSubdir);
                    }
                    CheckDirectory(dir, ModSubdir);
                }
            }
            List<string> files = new List<string>();
            files = Directory.EnumerateFiles(Dir).ToList<string>();
            if (files.Count > 0)
            {
                foreach (string image in files)
                {
                    ReduceImage(image, ModDir);
                }
            }
        }

        private void ReduceImage(string image, string modDir)
        {
            try
            {
                using (Bitmap bitmap = (Bitmap)Image.FromFile(image))
                {
                    using (Bitmap newBitmap = new Bitmap(bitmap, new Size(bitmap.Width / Indice, bitmap.Height / Indice)))
                    {
                        string name = image.Split('\\')[image.Split('\\').Length - 1];
                        newBitmap.Save(modDir + "\\" + name, ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

    }
}

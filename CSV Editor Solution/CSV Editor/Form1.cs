/*
 ***************************************************************************
 *                                                                         *
 *                              Leonel Jerez                               *
 *                                9/5/2018                                 *
 *                              CSV Editor v1                              *
 *                                                                         *
 ***************************************************************************
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSV_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            string fileLocation = null;

            //Dialog Box to open file
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Title = "Open a CSV File";
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "csv files (*.csv)|*.csv";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Multiselect = false;

                //Check to make sure file is good.
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileLocation = openFileDialog1.FileName;
                }
            }

            //Check to see if its empty
            if (fileLocation != null)
            {
                textBox1.Text = fileLocation;            
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            string fileLocation = null;

            //Dialog Box to open file
            using (SaveFileDialog openFileDialog1 = new SaveFileDialog())
            {
                openFileDialog1.Title = "Open a CSV File";
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "csv files (*.csv)|*.csv";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                //Check to make sure file is good.
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileLocation = openFileDialog1.FileName;
                }
            }

            //Check to see if its empty
            if (fileLocation != null)
            {
                textBox2.Text = fileLocation;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            //Gets input and output locations
            string openLocation = textBox1.Text;
            string saveLocation = textBox2.Text;
            string[] data;


            using (StreamReader sr = new StreamReader(openLocation))
            {

                /*
                 ***************************************************************************
                 *                                                                         *
                 *                  Check for half the size radio button                   *
                 *                                                                         *
                 ***************************************************************************
                 */
                if (radioHalf.Checked)
                {
                    int rows = 0;

                    //Count number of rows in csv file
                    using (var reader = File.OpenText(openLocation))
                    {
                        while (reader.ReadLine() != null)
                        {
                            rows++;
                        }
                    }

                    //divide by two for half
                    rows = rows / 2;
                    //creates new array using the amount of rows
                    data = new string[rows];


                    //Populate array with CSV rows
                    int arrayInjection = 0;
                    using (var reader = File.OpenText(openLocation))
                    {
                        while (reader.ReadLine() != null && arrayInjection != rows)
                        {
                            data[arrayInjection] = reader.ReadLine();
                            arrayInjection++;
                        }
                    }

                    if (!File.Exists(saveLocation))
                    {
                        File.Create(saveLocation).Close();
                    }

                    //Starts process for saving the file into a new document
                    StringBuilder sb = new StringBuilder();
                    for (int row = 0; row < rows; row++)
                    {
                        sb.Append(data[row]);
                        sb.AppendLine();
                    }

                    File.WriteAllText(saveLocation, sb.ToString());
                    textBox3.Text = "Completed";
                }

                /*
                 ***************************************************************************
                 *                                                                         *
                 *             Check for smaller than 100MB  radio button                  *
                 *                                                                         *
                 ***************************************************************************
                 */
                else if (radioSmaller.Checked)
                {
                    int rows = 0;

                    //Count number of rows in csv file
                    using (var reader = File.OpenText(openLocation))
                    {
                        while (reader.ReadLine() != null)
                        {
                            rows++;
                        }
                    }

                    //creates new array using the amount of rows
                    data = new string[rows];

                    //Gets megabyte of file
                    FileInfo csvFile = new FileInfo(openLocation);
                    long megabyte = csvFile.Length /(1024*1024);
                    //declares 100mb in bytes as max size
                    long maxbytes = 100000000;

                    //Populate array with CSV rows
                    int arrayInjection = 0;
                    using (var reader = File.OpenText(openLocation))
                    {
                        while (reader.ReadLine() != null && arrayInjection != rows)
                        {
                            data[arrayInjection] = reader.ReadLine();
                            arrayInjection++;
                        }
                    }


                    if (!File.Exists(saveLocation))
                    {
                         File.Create(saveLocation).Close();
                    }

                    //Starts process for saving the file into a new document
                    StringBuilder sb = new StringBuilder();
                    for (int row = 0; row < rows; row++)
                    {
                        if (sb.Length > maxbytes)
                        {
                            break;
                        }
                        sb.Append(data[row]);
                        sb.AppendLine();
                    }

                    File.WriteAllText(saveLocation, sb.ToString());
                    textBox3.Text = "Completed";
                }
            }
        }
    }
}

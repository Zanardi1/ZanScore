﻿using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace ZanScore
{
    public partial class NewsLibrary : Form
    {
        private static int absoluteindex=0;

        static public List<string> NewsSourcesList = new List<string>() { };
        public List<string> NewsCategoriesList = new List<string>() { };
        static public List<string> NewsSourcesRSSList = new List<string>() { };

        static public int AbsoluteIndex //calculeaza numarul de ordine al unei stiri din lista de stiri citita si stocata in cele trei liste declarate mai sus
        {
            get
            {
                return absoluteindex;
            }
            set
            {
                if (value >= 0)
                    absoluteindex = value;
                else
                    absoluteindex = 0;
            }
        }

        //todo de adaugat surse de stiri cautate de catre mine

        public NewsLibrary()
        {
            InitializeComponent();
            ClearLists();
            OpenLibraryFile();
        }

        private void ClearLists()
        {
            NewsSourcesList.Clear();
            NewsCategoriesList.Clear();
            NewsSourcesRSSList.Clear();
        }

        private bool CheckIfFileExists()
        //Verifica daca exista fisierul ce contine biblioteca 
        {
            if (!File.Exists("Library.txt"))
                return false;
            else
                return true;
        }

        private void ReadFromLibrary()
        //Procedura citeste din biblioteca si umple cele trei liste
        {
            string[] ReadBuffer = new string[] { };
            ReadBuffer = File.ReadAllLines("Library.txt");
            int i = 0, found = 0, j = 0;
            for (i = 0; i < ReadBuffer.Length; i += 3)
            {
                NewsSourcesList.Add(ReadBuffer[i]);
                found = ReadBuffer[i].IndexOf(":");
                NewsSourcesList[j] = NewsSourcesList[j].Substring(found + 1);

                NewsCategoriesList.Add(ReadBuffer[i + 1]);
                found = ReadBuffer[i + 1].IndexOf(":");
                NewsCategoriesList[j] = NewsCategoriesList[j].Substring(found + 1);

                NewsSourcesRSSList.Add(ReadBuffer[i + 2]);
                found = ReadBuffer[i + 2].IndexOf(":");
                NewsSourcesRSSList[j] = NewsSourcesRSSList[j].Substring(found + 1);
                j++;
            }
        }

        private void FillWindowDatagrid(int Category)
        /*Procedura ia datele din cele trei liste si umple datagrid-ul in functie de categoria selectata. Valorile pentru Category sunt:
         0 - National & World News;
         1 - Sports;
         2 - Gaming;
         3 - Lifestyle;
         4 - Music;
         5 - Science;
         6 - Technology;
         7 - Politics;
         8 - Entertainment;
         9 - Business;*/
        {
            int LowerBound = 0, UpperBound = 0, j = 0;
            NewsLibrarySourcesView.Rows.Clear();
            switch (Category)
            {
                case 0:
                    {
                        LowerBound = 0;
                        UpperBound = 26;
                        break;
                    }
                case 1:
                    {
                        LowerBound = 27;
                        UpperBound = 43;
                        break;
                    }
                case 2:
                    {
                        LowerBound = 44;
                        UpperBound = 52;
                        break;
                    }
                case 3:
                    {
                        LowerBound = 53;
                        UpperBound = 94;
                        break;
                    }
                case 4:
                    {
                        LowerBound = 95;
                        UpperBound = 102;
                        break;
                    }
                case 5:
                    {
                        LowerBound = 103;
                        UpperBound = 117;
                        break;
                    }
                case 6:
                    {
                        LowerBound = 118;
                        UpperBound = 129;
                        break;
                    }
                case 7:
                    {
                        LowerBound = 130;
                        UpperBound = 136;
                        break;
                    }
                case 8:
                    {
                        LowerBound = 137;
                        UpperBound = 143;
                        break;
                    }
                case 9:
                    {
                        LowerBound = 144;
                        UpperBound = 150;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            for (int i = LowerBound; i <= UpperBound; i++)
            {
                NewsLibrarySourcesView.Rows.Add();
                NewsLibrarySourcesView.Rows[j].Cells[0].Value = NewsSourcesList[i];
                j++;
            }
            NewsLibrarySourcesView.Rows[0].Selected = true;
        }

        private void OpenLibraryFile()
        {
            if (!CheckIfFileExists())
            {
                MessageBox.Show("Library file missing. This window will now close.");
                Close();
            }
            else
            {
                ReadFromLibrary();
                CategoryListBox.SelectedIndex = 0;
                FillWindowDatagrid(0);
            }
        }

        private void ChangeNewsSourcesCategory(object sender, System.EventArgs e)
        //Procedura afiseaza sursele in functie de categoria selectata
        {
            FillWindowDatagrid(CategoryListBox.SelectedIndex);
        }

        private int ReturnFirstSelected(DataGridView Grid)
        {
            int SelectedPosition = 0;
            for (int i = 0; i < Grid.RowCount; i++)
                if (Grid.Rows[i].Selected)
                    SelectedPosition = i;
            return SelectedPosition;
        }

        private int ComputeAbsoluteIndex()
        {
            int Category = 0, Place = 0;
            switch (CategoryListBox.SelectedIndex)
            {
                case 0:
                    {
                        Category = 0;
                        break;
                    }
                case 1:
                    {
                        Category = 27;
                        break;
                    }
                case 2:
                    {
                        Category = 44;
                        break;
                    }
                case 3:
                    {
                        Category = 53;
                        break;
                    }
                case 4:
                    {
                        Category = 95;
                        break;
                    }
                case 5:
                    {
                        Category = 103;
                        break;
                    }
                case 6:
                    {
                        Category = 118;
                        break;
                    }
                case 7:
                    {
                        Category = 130;
                        break;
                    }
                case 8:
                    {
                        Category = 137;
                        break;
                    }
                case 9:
                    {
                        Category = 144;
                        break;
                    }
            }
            Place = ReturnFirstSelected(NewsLibrarySourcesView);
            return Category + Place;
        }

        private void VisitTheSelectedSource(object sender, System.EventArgs e)
        //Procedura de vizitare a sursei selectate
        {
            AbsoluteIndex = ComputeAbsoluteIndex();
            ((Form1)Owner).DownloadAllNewsProcess(false);
            Close();
        }
    }
}
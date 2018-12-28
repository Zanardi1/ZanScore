﻿using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace ZanScore
{
    public partial class EditSourcesWindow : Form
    {
        int SelectedPositionInGrid = 0;
        public EditSourcesWindow()
        {
            InitializeComponent();
            SourceNameToEdit.Width = AllTheSources.Width / 3;
            SourceURLToEdit.Width = 2 * AllTheSources.Width / 3;
        }

        private void DeleteSelectedNewsSources(object sender, System.EventArgs e)
        {
            DialogResult UserAnswer;
            string text = "Are you sure you want to delete the selected news source? This cannot be undone!";
            string caption = "Confirm deletion";
            MessageBoxButtons Buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon Icon = MessageBoxIcon.Question;
            MessageBoxDefaultButton Default = MessageBoxDefaultButton.Button1;
            UserAnswer = MessageBox.Show(text, caption, Buttons, Icon, Default);
            if (UserAnswer == DialogResult.Yes)
            {
                List<int> Positions = new List<int>();
                int j = 0;
                for (int i = 0; i < AllTheSources.RowCount; i++)
                    if (AllTheSources.Rows[i].Selected)
                    {
                        Positions.Add(i);
                        j++;
                    }
                ((Form1)Owner).NewsSourcesCollection.RemoveSource(Positions);
                ((Form1)Owner).NewsSourcesCollection.SaveSources();
                for (int i = 0; i < AllTheSources.Rows.Count; i++)
                    if (AllTheSources.Rows[i].Selected)
                        AllTheSources.Rows.RemoveAt(i);
            }
        }

        private void EditSelectedSource(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            bool IsNewsSelected = false;
            EnableEditingControls();
            while ((i < AllTheSources.RowCount) && (!IsNewsSelected))
            {
                if (AllTheSources.Rows[i].Selected)
                {
                    NewSourceNameText.Text = AllTheSources.Rows[i].Cells[0].Value.ToString();
                    NewSourceURLText.Text = AllTheSources.Rows[i].Cells[1].Value.ToString();
                    j = i + 1;
                    SelectedPositionInGrid = i;
                    while (j < AllTheSources.RowCount) //Deselecteaza toate stirile de dedesubtul primei stiri alese (care va fi si editata)
                    {
                        AllTheSources.Rows[j].Selected = false;
                        j++;
                    }
                    IsNewsSelected = true;
                }
                i++;
            }
        }

        private void DiscardEditingChanges(object sender, EventArgs e)
        {
            DisableEditingControls();
        }

        private void SaveEditChanges(object sender, EventArgs e)
        {
            ((Form1)Owner).NewsSourcesCollection.EditSource(SelectedPositionInGrid, NewSourceNameText.Text, NewSourceURLText.Text);
            AllTheSources.Rows[SelectedPositionInGrid].Cells[0].Value = NewSourceNameText.Text;
            AllTheSources.Rows[SelectedPositionInGrid].Cells[1].Value = NewSourceURLText.Text;
            DisableEditingControls();
        }

        private void EnableEditingControls()
        {
            NewSourceNameLabel.Enabled = true;
            NewSourceURLLabel.Enabled = true;
            NewSourceNameText.Enabled = true;
            NewSourceURLText.Enabled = true;
            SaveNewsEditButton.Enabled = true;
            DiscardNewsEditButton.Enabled = true;
            ReorderNewsButton.Enabled = false;
            DeleteNewsButton.Enabled = false;
        }

        private void DisableEditingControls()
        {
            NewSourceNameLabel.Enabled = false;
            NewSourceURLLabel.Enabled = false;
            NewSourceNameText.Enabled = false;
            NewSourceURLText.Enabled = false;
            SaveNewsEditButton.Enabled = false;
            DiscardNewsEditButton.Enabled = false;
            ReorderNewsButton.Enabled = true;
            DeleteNewsButton.Enabled = true;
            NewSourceNameText.Text = "";
            NewSourceURLText.Text = "";
        }

        private void DisableReorderingControls()
        {
            MoveUpOnePositionButton.Enabled = false;
            MoveDownOnePositionButton.Enabled = false;
            MoveToFirstPositionButton.Enabled = false;
            MoveToLastPositionButton.Enabled = false;
            EditNewsButton.Enabled = true;
            DeleteNewsButton.Enabled = true;
        }

        private void EnableReorderingControls()
        //Activeaza controalele ce permit reordonarea surselor
        {
            MoveUpOnePositionButton.Enabled = true;
            MoveDownOnePositionButton.Enabled = true;
            MoveToFirstPositionButton.Enabled = true;
            MoveToLastPositionButton.Enabled = true;
            EditNewsButton.Enabled = false;
            DeleteNewsButton.Enabled = false;
        }

        private void ReorderSelectedNewsSources(object sender, EventArgs e)
        {
            EnableReorderingControls();
        }

        private void MoveSelectedSourceUpOnePosition(object sender, EventArgs e)
        //Subrutina muta sursa aleasa cu o pozitie mai sus
        {
            int i = 0;
            string buffer;
            while (AllTheSources.Rows[i].Selected == false)
                i++; //Cauta prima sursa selectata si-i retine pozitia
            ((Form1)this.Owner).NewsSourcesCollection.SortSources(i, 1);
            if (i > 0)
            {
                buffer = AllTheSources.Rows[i - 1].Cells[0].Value.ToString();
                AllTheSources.Rows[i - 1].Cells[0].Value = AllTheSources.Rows[i].Cells[0].Value;
                AllTheSources.Rows[i].Cells[0].Value = buffer;

                buffer = AllTheSources.Rows[i - 1].Cells[1].Value.ToString();
                AllTheSources.Rows[i - 1].Cells[1].Value = AllTheSources.Rows[i].Cells[1].Value;
                AllTheSources.Rows[i].Cells[1].Value = buffer;

                AllTheSources.Rows[i - 1].Selected = true;
                AllTheSources.Rows[i].Selected = false;
            }
        }

        private void MoveSelectedSourceDownOnePosition(object sender, EventArgs e)
        //Subrutina muta sursa aleasa cu o pozitie mai jos
        {
            int i = 0;
            string buffer;
            while (AllTheSources.Rows[i].Selected == false)
                i++; //Cauta prima sursa selectata si-i retine pozitia
            ((Form1)this.Owner).NewsSourcesCollection.SortSources(i, 2);
            if (i < AllTheSources.RowCount)
            {
                buffer = AllTheSources.Rows[i + 1].Cells[0].Value.ToString();
                AllTheSources.Rows[i + 1].Cells[0].Value = AllTheSources.Rows[i].Cells[0].Value;
                AllTheSources.Rows[i].Cells[0].Value = buffer;

                buffer = AllTheSources.Rows[i + 1].Cells[1].Value.ToString();
                AllTheSources.Rows[i + 1].Cells[1].Value = AllTheSources.Rows[i].Cells[1].Value;
                AllTheSources.Rows[i].Cells[1].Value = buffer;

                AllTheSources.Rows[i + 1].Selected = true;
                AllTheSources.Rows[i].Selected = false;
            }
        }

        private void MoveSelectedSourceToFirstPosition(object sender, EventArgs e)
        //Subrutina muta sursa aleasa pe prima pozitie
        {
            int i = 0;
            string buffer;
            while (AllTheSources.Rows[i].Selected == false)
                i++; //Cauta prima sursa selectata si-i retine pozitia
            ((Form1)this.Owner).NewsSourcesCollection.SortSources(i, 3);
            if (i > 0) //todo Eroare de depasire la aceasta secventa
            {
                buffer = AllTheSources.Rows[0].Cells[0].Value.ToString();
                for (int j = 0; j < AllTheSources.RowCount; j++)
                    AllTheSources.Rows[j].Cells[0].Value = AllTheSources.Rows[j + 1].Cells[0].Value;
                AllTheSources.Rows[AllTheSources.RowCount - 1].Cells[0].Value = buffer;

                buffer = AllTheSources.Rows[0].Cells[1].Value.ToString();
                for (int j = 0; j < AllTheSources.RowCount; j++)
                    AllTheSources.Rows[j].Cells[1].Value = AllTheSources.Rows[j + 1].Cells[1].Value;
                AllTheSources.Rows[AllTheSources.RowCount - 1].Cells[1].Value = buffer;
            }
        }
    }
}

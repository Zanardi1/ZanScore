﻿using System.Windows.Forms;
using System.Xml;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.IO;

namespace ZanScore
/*
O biblioteca ce contine toate functiile necesare prelucrarii unui fisier RSS:
1. Deschiderea unui fisier RSS;
2. Validarea acestuia;
3. Citirea si interpretarea lui;
4. Extragerea informatiilor din el si punerea lor in program;
5. 

 */

{
    /// <summary>
    /// Class that handles the data read from an RSS file. Details about this file format are at: https://www.w3schools.com/xml/xml_rss.asp .
    /// </summary>
    public class RSSSourceData
    {
        /// <summary>
        /// List which stores the titles of the news channels
        /// </summary>
        public List<string> NewsChannelTitle = new List<string>(); 
        /// <summary>
        /// List which stores the nwes' titles
        /// </summary>
        public List<string> NewsTitle = new List<string>(); 
        /// <summary>
        /// List that stores the news' URLs
        /// </summary>
        public List<string> NewsLink = new List<string>(); 
        /// <summary>
        /// List that stores the news' descriptions
        /// </summary>
        public List<string> NewsDescription = new List<string>(); 

        /// <summary>
        /// Class constructor
        /// </summary>
        public RSSSourceData()
        {

        }

        /// <summary>
        /// It loads a specified RSS file, in XML format, parses it and fills the class structures from the program with the required data from the file.
        /// </summary>
        /// <param name="FileToLoad">The RSS file that must be read</param>
        /// <returns>true if no exception was thrown. Else it returns false</returns>
        public bool FillRSSData(string FileToLoad)
        //Ideea si metoda am luat-o de la: https://stackoverflow.com/questions/10399400/best-way-to-read-rss-feed-in-net-using-c-sharp
        //todo de rescris astfel incat sa faca aceleasi lucruri si sa testeze aparitia exceptiilor pentru crearea variabilei reader
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show("No internet connection detected!", "Error!", buttons, icon);
                return false;
            }

            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                IgnoreComments = true,
                IgnoreWhitespace = true
            };
            XmlReader reader = XmlReader.Create(FileToLoad, settings);

            SyndicationFeed feed = new SyndicationFeed();
            try
            {
                feed = SyndicationFeed.Load(reader);
            }

            catch (XmlException e)
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(e.Message + " Program will go to the next news source", "Error loading news source file", buttons, icon);
                return false;
            }

            catch (FileNotFoundException F) //In cazul in care nu gaseste fisierul XML. Nu cred ca va aparea vreodata exceptia asta, avand in vedere ca lucreaza online, dar sa fiu sigur
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(F.Message + " Program will go to the next news source", "Error loading news source file", buttons, icon);
                return false;
            }

            catch (System.IO.IOException I)
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(I.Message + " Program will go to the next news source", "Error loading news source file", buttons, icon);
                return false;
            }

            catch (System.Net.WebException W)
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(W.Message + " Program will go to the next news source", "Error loading news source file", buttons, icon);
                return false;
            }

            finally
            {
                reader.Close();
            }

            foreach (SyndicationItem item in feed.Items)
            {
                NewsChannelTitle.Add(feed.Title == null ? "" : feed.Title.Text.ToString());
                NewsTitle.Add(item.Title == null ? "" : item.Title.Text);
                NewsLink.Add(item.Links[0].Uri.ToString() == null ? "" : item.Links[0].Uri.ToString());
                NewsDescription.Add(item.Summary == null ? "" : item.Summary.Text);
            }

            return true;
        }

        /// <summary>
        /// Clears the four lists which are class members
        /// </summary>
        public void EmptyFields()
        {
            NewsChannelTitle.Clear();
            NewsDescription.Clear();
            NewsLink.Clear();
            NewsTitle.Clear();
        }
    }
}
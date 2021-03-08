using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace GMAD_GUI
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        //Use this to keep track of wether we are extracting or creating an addon
        private enum GMADState
        {
            Create,
            Extract
        }
        private GMADState gmadState = GMADState.Extract;
        public Form1()
        {
            InitializeComponent();
            //Set the callback from the console output
            

        }

        private void consoleControl1_Load(object sender, EventArgs e)
        {
            consoleControl1.OnConsoleOutput += ConsoleControl1_OnConsoleOutput;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
        private string gmaLocation;
        private string outputLocation;
        private string addonLocation;
        //Top browse button, sets .gma to extract or folder to compile
        private void metroTile4_Click(object sender, EventArgs e)
        {
            if (gmadState == GMADState.Extract)
            {
                var fileBrowser = new OpenFileDialog();
                fileBrowser.Multiselect = false;
                fileBrowser.Filter = "GMOD Addons|*.gma|All Files|*.*";
                fileBrowser.ShowDialog();
                metroTextBox1.Text = fileBrowser.FileName;
                gmaLocation = fileBrowser.FileName;
            } else
            {
                var fileBrowser = new CommonOpenFileDialog();
                fileBrowser.IsFolderPicker = true;
                CommonFileDialogResult result = fileBrowser.ShowDialog();
                metroTextBox1.Text = fileBrowser.FileName;
                addonLocation = fileBrowser.FileName;
            }
        }
        //Bottom browse button, sets output location
        private void metroTile5_Click(object sender, EventArgs e)
        {
            if (gmadState == GMADState.Extract)
            {
                var fileBrowser = new CommonOpenFileDialog();
                fileBrowser.IsFolderPicker = true;
                CommonFileDialogResult result = fileBrowser.ShowDialog();
                metroTextBox2.Text = fileBrowser.FileName;
                outputLocation = fileBrowser.FileName;
            } else
            {
                var fileBrowser = new SaveFileDialog();
                fileBrowser.Filter = "GMOD Addons|*.gma|All Files|*.*";
                fileBrowser.DefaultExt = ".gma";
                fileBrowser.ShowDialog();
                metroTextBox2.Text = fileBrowser.FileName;
                gmaLocation = fileBrowser.FileName;
            }
            
        }
        // The extract/create button
        private void metroTile1_Click(object sender, EventArgs e)
        {
            
            if (gmadState == GMADState.Extract)
            {

                var startInfo = new ProcessStartInfo();
                startInfo.FileName = @"C:\Program Files (x86)\Steam\steamapps\common\GarrysMod\bin\gmad.exe";
                startInfo.Arguments = "extract -file " + gmaLocation + " -out " + outputLocation;
                consoleControl1.Visible = true;
                consoleControl1.StartProcess(startInfo.FileName, startInfo.Arguments);

            } else {

                var startInfo = new ProcessStartInfo();
                startInfo.FileName = @"C:\Program Files (x86)\Steam\steamapps\common\GarrysMod\bin\gmad.exe";
                startInfo.Arguments = "create -folder " + addonLocation + " -out " + gmaLocation;
                consoleControl1.Visible = true;
                consoleControl1.StartProcess(startInfo.FileName, startInfo.Arguments);

            }  
        }
        
        //This function is called whenever the console receives an output
        private void ConsoleControl1_OnConsoleOutput(object sender, ConsoleControl.ConsoleEventArgs args)
        {
            
           if (consoleControl1.InternalRichTextBox.Text.Contains("Done!"))
            {

                
                var result = System.Windows.Forms.MessageBox.Show("Success!", "Finished Extracting");
                
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    consoleControl1.ClearOutput();
                    consoleControl1.InternalRichTextBox.Text = "";
                    consoleControl1.Visible = false;
                    //Reset arguments
                    gmaLocation = "";
                    outputLocation = "";
                    addonLocation = "";
                }
            } 
            if (consoleControl1.InternalRichTextBox.Text.Contains("Successfully saved"))
            {
                
                var result = System.Windows.Forms.MessageBox.Show("Success!", "Addon created");
               
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    consoleControl1.ClearOutput();
                    consoleControl1.InternalRichTextBox.Text = "";
                    consoleControl1.Visible = false;
                    //Reset arguments
                    gmaLocation = "";
                    outputLocation = "";
                    addonLocation = "";
                }
            } 

        }
        // Switch to the create window
        private void metroTile2_Click(object sender, EventArgs e)
        {
            gmadState = GMADState.Create;
            metroTile1.Text = "Create!";
            metroTextBox1.Text = "Select folder with addon files";
            //Reset arguments
            gmaLocation = "";
            outputLocation = "";
            addonLocation = "";
        }
        //Switch to the extract window
        private void metroTile3_Click(object sender, EventArgs e)
        {
            gmadState = GMADState.Extract;
            metroTile1.Text = "Extract!";
            metroTextBox1.Text = "Select .gma to extract";
            //Reset arguments
            gmaLocation = "";
            outputLocation = "";
            addonLocation = "";
        }
    }
}

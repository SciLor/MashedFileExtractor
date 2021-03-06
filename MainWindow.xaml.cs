﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using SciLors_Mashed_File_Extractor.FileFormats.PIZ;

namespace SciLors_Mashed_File_Extractor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private List<PIZFile> files = new List<PIZFile>();
        public MainWindow() {
            InitializeComponent();
        }

        private void mniOpen_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "PIZ files (*.piz)|*.piz";//|RWS files (*.RWS)|*.rws|";
            if (openFileDialog.ShowDialog() == true) {
                tvwContent.Items.Clear();
                foreach (string filePath in openFileDialog.FileNames) {
                    PIZFile file = new PIZFile(filePath);
                    TreeViewItem baseItem = new TreeViewItem();
                    baseItem.Tag = file;
                    baseItem.Header = file.fileName + " ["
                        + String.Format("Appendix2=0x{0:X2}", (byte)file.fileSignature.appendix2)
                        + "]";

                    TreeViewItem baseInfoItem = new TreeViewItem();
                    baseInfoItem.Header = "Files [" + file.files.Count + "]";
                    foreach (FileHeader fileHeader in file.files) {
                        TreeViewItem fileInfoItem = new TreeViewItem();
                        fileInfoItem.Tag = fileHeader;
                        fileInfoItem.Header = fileHeader.fileName;
                        fileInfoItem.ItemsSource = new string[] {
                            String.Format("Offset=0x{0:X8}", fileHeader.fileOffset),
                            "Size=" + fileHeader.fileSize,
                            String.Format("Unknown=0x{0:X8}", fileHeader.fileUnknown),
                        };
                        ContextMenu ctxFile = new ContextMenu();
                        MenuItem extractMenu = new MenuItem();
                        extractMenu.Header = "Extract File";
                        extractMenu.Click += new RoutedEventHandler(tvwContext_ExtractFile);

                        ctxFile.Items.Add(extractMenu);
                        fileInfoItem.ContextMenu = ctxFile;
                        baseInfoItem.Items.Add(fileInfoItem);
                    }

                    baseItem.Items.Add(baseInfoItem);
                    tvwContent.Items.Add(baseItem);
                }
            }
                
        }

        private void tvwContext_ExtractFile(object sender, RoutedEventArgs e) {
            TreeViewItem parent = (TreeViewItem)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget;
            PIZFile pizFile = (PIZFile)((TreeViewItem)((TreeViewItem)parent.Parent).Parent).Tag;
            FileHeader fileHeader = (FileHeader)parent.Tag;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = fileHeader.fileName;
            saveFileDialog.ShowDialog();

            pizFile.extractFile(fileHeader, saveFileDialog.FileName);
            
        }
    }
}

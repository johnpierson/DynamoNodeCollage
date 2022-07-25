using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dynamo.Core;
using Dynamo.UI.Commands;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace DynamoNodeCollage.CreateCollageCommand
{
    public class CreateCollageViewModel : ViewModelBase
    {
        public CreateCollageModel Model { get; set; }
        public DelegateCommand OpenImage { get; set; }
        public DelegateCommand ReadColors { get; set; }
        public DelegateCommand PlaceNodes { get; set; }

        private DSCore.Color[][] _colors;
        public DSCore.Color[][] Colors
        {
            get => _colors;
            set { _colors = value; RaisePropertyChanged(() => Colors); }
        }
        private int _density;
        public int Density
        {
            get => _density;
            set { _density = value; RaisePropertyChanged(() => Density); }
        }
        private int _nodeCount;
        public int NodeCount
        {
            get => _nodeCount;
            set { _nodeCount = value; RaisePropertyChanged(() => NodeCount); }
        }
        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set { _imagePath = value; RaisePropertyChanged(() => ImagePath); }
        }
        public CreateCollageViewModel(CreateCollageModel model)
        {
            Model = model;

            //commands
            OpenImage = new DelegateCommand(OnOpenImage);
            ReadColors = new DelegateCommand(OnReadColors);
            PlaceNodes = new DelegateCommand(OnPlaceNodes);
        }
        public void OnOpenImage(object o)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;"
            };

            if (ofd.ShowDialog().HasValue)
            {
                ImagePath = ofd.FileName;
            }
        }
        public void OnReadColors(object o)
        {
            Colors = Model.GetPixels(ImagePath,Density);

            NodeCount = Colors.Length * Colors.Rank;
        }

        public void OnPlaceNodes(object o)
        {
            double rowNum = 0;
            double colNum = 0;
            foreach (var row in Colors)
            {
                foreach (var c in row)
                {
                    Model.PlaceNodeWithGroup(c, rowNum, colNum);
                    rowNum += 200;
                }
                colNum += 200;
                rowNum = 0;
            }
        }
    }
}

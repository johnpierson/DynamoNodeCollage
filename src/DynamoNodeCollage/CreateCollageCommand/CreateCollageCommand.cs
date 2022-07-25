using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.Models;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using Microsoft.Win32;

namespace DynamoNodeCollage.CreateCollageCommand
{
    public  class CreateCollageCommand
    {
        public static DynamoViewModel DynamoViewModel { get; set; }
        public static void AddMenuItem(ViewLoadedParams p)
        {
            DynamoViewModel = p.DynamoWindow.DataContext as DynamoViewModel;
            var m = new CreateCollageModel(p);
            var vm = new CreateCollageViewModel(m);

            var menuItem = new MenuItem { Header = "Dynamo Collage" };

            menuItem.Click += (sender, args) =>
            {
                var window = new CreateCollageView
                {
                    // Set the data context for the main grid in the window.
                    MainGrid = { DataContext = vm },
                    // Set the owner of the window to the Dynamo window.
                    Owner = p.DynamoWindow
                };

                window.Show();
            };

            p.AddExtensionMenuItem(menuItem);
        }
        public static void PlaceNodeWithGroup(DSCore.Color c, double row, double column)
        {
            //create code block
            var codeBlockGuid = Guid.NewGuid();
            CodeBlockNodeModel newCodeBlock = new CodeBlockNodeModel("\"neat dynamo logo\"", codeBlockGuid, 0, 0,
                DynamoViewModel.Model.LibraryServices, DynamoViewModel.Model.CurrentWorkspace.ElementResolver);
            DynamoViewModel.Model.ExecuteCommand(
                new DynamoModel.CreateNodeCommand(newCodeBlock, row, column, false, false));
            var codeBlock = DynamoViewModel.CurrentSpaceViewModel.Nodes.First(node => node.NodeModel.GUID.Equals(codeBlockGuid));

            //create group and color it
            DynamoModel.CreateAnnotationCommand annotationCommand = new DynamoModel.CreateAnnotationCommand(Guid.NewGuid(), " ", " ", codeBlock.X, codeBlock.Y, false);
            DynamoViewModel.Model.ExecuteCommand(annotationCommand);
            DynamoViewModel.CurrentSpaceViewModel.Annotations.Last().Background = System.Windows.Media.Color.FromRgb(c.Red, c.Green, c.Blue);
        }

        public static string OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;"
            };

            if (ofd.ShowDialog().HasValue)
            {
                return ofd.FileName;
            }

            return string.Empty;
        }
        public static DSCore.Color[][] GetPixels(string imagePath, int density = 10)
        {
            Bitmap bitmap = new Bitmap(imagePath);

            int horizontalTiles = (int)bitmap.Width / density;
            int verticalTiles = (int)bitmap.Height / density;

            return DSCore.IO.Image.Pixels(bitmap, horizontalTiles, verticalTiles);
        }
    }
}

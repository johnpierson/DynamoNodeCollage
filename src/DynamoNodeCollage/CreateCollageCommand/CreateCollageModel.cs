using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.Models;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;

namespace DynamoNodeCollage.CreateCollageCommand
{
    public class CreateCollageModel
    {
        public DynamoView dynamoView { get; }
        public ViewLoadedParams LoadedParams { get; }
        public DynamoViewModel DynamoViewModel { get; }
        public CreateCollageModel(ViewLoadedParams p)
        {
            dynamoView = p.DynamoWindow as DynamoView;
            LoadedParams = p;
            DynamoViewModel = p.DynamoWindow.DataContext as DynamoViewModel;
        }

        public void PlaceNodeWithGroup(DSCore.Color c, double row, double column)
        {
            //create code block
            var codeBlockGuid = Guid.NewGuid();
            CodeBlockNodeModel newCodeBlock = new CodeBlockNodeModel("\"neat dynamo logo\"", codeBlockGuid, 0, 0,
                DynamoViewModel.Model.LibraryServices, DynamoViewModel.Model.CurrentWorkspace.ElementResolver);
            DynamoViewModel.Model.ExecuteCommand(
                new DynamoModel.CreateNodeCommand(newCodeBlock, row, column, false, false));
            var codeBlock = DynamoViewModel.CurrentSpaceViewModel.Nodes.First(node => node.NodeModel.GUID.Equals(codeBlockGuid));

            //create group and color it
            DynamoModel.CreateAnnotationCommand annotationCommand = new DynamoModel.CreateAnnotationCommand(Guid.NewGuid()," "," ",codeBlock.X,codeBlock.Y,false);
            DynamoViewModel.Model.ExecuteCommand(annotationCommand);
            DynamoViewModel.CurrentSpaceViewModel.Annotations.Last().Background = System.Windows.Media.Color.FromRgb(c.Red,c.Green,c.Blue);
        }

        public DSCore.Color[][] GetPixels(string imagePath, int density = 10)
        {
            Bitmap bitmap = new Bitmap(imagePath);

            int horizontalTiles = (int)bitmap.Width / density;
            int verticalTiles = (int)bitmap.Height / density;


            return DSCore.IO.Image.Pixels(bitmap, horizontalTiles,verticalTiles);
        }
    }
}

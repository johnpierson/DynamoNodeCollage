using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Wpf.Extensions;

namespace DynamoNodeCollage
{
    public class DynamoNodeCollageViewExtension : IViewExtension
    {
        public void Dispose()
        {
        }

        public void Startup(ViewStartupParams viewStartupParams)
        {
        }

        public void Loaded(ViewLoadedParams viewLoadedParams)
        {
           CreateCollageCommand.CreateCollageCommand.AddMenuItem(viewLoadedParams);
        }

        public void Shutdown()
        {
            
        }

        public string UniqueId => "5748EE34-E31E-4D9A-96D6-345FEE83B00A";
        public string Name => "Dynamo Node Collage View Extension";
    }
}

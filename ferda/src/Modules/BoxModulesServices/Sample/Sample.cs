using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.Sample
{
    public class Service : Ferda.Modules.FerdaServiceI
    {
        protected override void registerBoxes()
        {
            this.registerBox("BodyMassIndexSampleFactoryCreator", new Sample.BodyMassIndex.BodyMassIndexBoxInfo());
            // if more boxes should be provided by this service, register them here ...
        }
    }
}

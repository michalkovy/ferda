using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.Sample
{
    /// <summary>
    /// Sample IceBox service with BodyMassIndex box 
    /// </summary>
    public class Service : Ferda.Modules.FerdaServiceI
    {
        /// <summary>
        /// Registers box to ice object adapter
        /// </summary>
        protected override void registerBoxes()
        {
            this.registerBox("BodyMassIndexSampleFactoryCreator", new Sample.BodyMassIndex.BodyMassIndexBoxInfo());
            // if more boxes should be provided by this service, register them here ...
        }
    }
}

namespace SampleBoxModules
{
    public class Service : Ferda.Modules.FerdaServiceI
	{
        protected override void registerBoxes()
        {
            this.registerBox("SampleBoxModuleSampleBoxModules", new SampleBoxes.SampleBoxModule.SampleBoxModuleBoxInfo());
        }
    }
}

using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.Plugins;

namespace SpecFlowPlugin
{
    public class SpecflowThreadSafeHooks : IRuntimePlugin
    {
        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters)
        {
            runtimePluginEvents.CustomizeGlobalDependencies += RuntimePluginEvents_CustomizeGlobalDependencies;
        }

        private void RuntimePluginEvents_CustomizeGlobalDependencies(object sender,
            CustomizeGlobalDependenciesEventArgs e)
        {
            e.ObjectContainer.RegisterTypeAs<FeatureBindingInvoker, IBindingInvoker>();
        }
    }
}
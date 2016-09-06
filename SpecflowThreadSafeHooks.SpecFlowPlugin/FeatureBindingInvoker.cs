using System;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.Configuration;
using TechTalk.SpecFlow.ErrorHandling;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Tracing;

namespace SpecFlowPlugin
{
    public class FeatureBindingInvoker : BindingInvoker
    {
        public FeatureBindingInvoker(RuntimeConfiguration runtimeConfiguration, IErrorProvider errorProvider)
            : base(runtimeConfiguration, errorProvider)
        {
        }

        public static event Action<FeatureContext, HookType> FeatureStarted;

        public override object InvokeBinding(IBinding binding, IContextManager contextManager, object[] arguments,
            ITestTracer testTracer,
            out TimeSpan duration)
        {
            var featureHooks = new[] {HookType.BeforeFeature, HookType.AfterFeature};
            var hookType = (binding as HookBinding)?.HookType;

            if (hookType.HasValue && featureHooks.Contains(hookType.Value))
            {
                var hasOnlyOneParameter = binding.Method.Parameters.Count() == 1;

                if (hasOnlyOneParameter)
                {
                    var parameterIsFeatureContext = binding.Method.Parameters.First().Type.FullName ==
                                                    typeof(FeatureContext).FullName;

                    if (parameterIsFeatureContext)
                    {
                        arguments = new object[] {contextManager.FeatureContext};
                        FeatureStarted?.Invoke(contextManager.FeatureContext, hookType.Value);
                    }
                }
            }

            return base.InvokeBinding(binding, contextManager, arguments, testTracer, out duration);
        }
    }
}
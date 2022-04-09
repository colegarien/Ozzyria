using Ozzyria.Game.ECS;
using Ozzyria.Test.ECS.Stub;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Ozzyria.Test
{
    public class PerformanceTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public PerformanceTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void newVsActivator()
        {
            var stopWatch = new Stopwatch();
            var trials = 1000000;

            var compNew = new List<IComponent>();
            var compAct = new List<IComponent>();

            stopWatch.Restart();
            for(int i = 0; i < trials; i++)
                compNew.Add(createWithNew<ComponentA>());
            stopWatch.Stop();

            var newTime = stopWatch.ElapsedMilliseconds;

            stopWatch.Restart();
            for (int i = 0; i < trials; i++)
                compAct.Add(createWithActivator(typeof(ComponentA)));
            stopWatch.Stop();

            var activateTime = stopWatch.ElapsedMilliseconds;

            var compString = $"new {newTime}ms vs activator {activateTime}ms";
            _testOutputHelper.WriteLine(compString);
            Assert.True(newTime < activateTime, compString);
        }

        public IComponent createWithNew<T>() where T : new()
        {
            return (IComponent)new T();
        }

        public IComponent createWithActivator(Type type)
        {
            return (IComponent)Activator.CreateInstance(type);
        }
    }
}

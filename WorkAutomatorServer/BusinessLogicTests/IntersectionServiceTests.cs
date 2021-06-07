using System;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using WorkAutomatorLogic.ServiceInterfaces;

namespace BusinessLogicTests
{
    public class IntersectionServiceTests
    {
        [Test]
        public void IntersectionTestIn()
        {
            Type serviceType = Assembly.GetAssembly(typeof(IAccountService)).DefinedTypes.First(t => t.Name == "IntersectionService");
            MethodInfo checkInsideMethod = serviceType.GetMethod("CheckInside");

            object service = Activator.CreateInstance(serviceType);
            bool isInside = (bool)checkInsideMethod.Invoke(
                service,
                new object[]
                {
                    (0.333571428571429, 0.636363636363636),
                    new (double, double)[]
                    {
                        (0.2514, 0.5307),
                        (0.2514, 0.6167),
                        (0.2514, 0.6904),
                        (0.2507, 0.9189),
                        (0.3793, 0.5332),
                        (0.3793, 0.9066)
                    }
                }
            );

            Assert.IsTrue(isInside);
        }

        [Test]
        public void IntersectionTestOut()
        {
            Type serviceType = Assembly.GetAssembly(typeof(IAccountService)).DefinedTypes.First(t => t.Name == "IntersectionService");
            MethodInfo checkInsideMethod = serviceType.GetMethod("CheckInside");

            object service = Activator.CreateInstance(serviceType);
            bool isInside = (bool)checkInsideMethod.Invoke(
                service,
                new object[]
                {
                    (0.333571428571429, 0.636363636363636),
                    new (double, double)[]
                    {
                        (0.1229, 0.9263),
                        (0.12, 0.6953),
                        (0.1243, 0.5356),
                        (0.1986, 0.5307),
                        (0.2386, 0.5307),
                        (0.2514, 0.5307),
                        (0.2514, 0.6167),
                        (0.2514, 0.6904),
                        (0.2507, 0.9189)
                    }
                }
            );

            Assert.IsFalse(isInside);
        }
    }
}

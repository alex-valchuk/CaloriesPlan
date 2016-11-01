using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CaloriesPlan.API.Filters;

namespace CaloriesPlan.API.Tests
{
    [TestClass]
    public class OwnerOrIsInOneOfRolesAttrbuteTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            //arrange
            var attr = new OwnerOrIsInOneOfRoles("User");
            var actionContext = new HttpActionContext()

        }
    }
}

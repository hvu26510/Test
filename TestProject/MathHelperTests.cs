using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;


namespace TestProject
{
    [TestFixture]
    internal class MathHelperTests
    {
        private MathHelper _m;
        [SetUp] public void Setup() => _m = new();

        [Test] public void N_5_Returns_9() => Assert.AreEqual(9, _m.SumOddNumbers(5));
        [Test] public void N_1_Returns_1() => Assert.AreEqual(1, _m.SumOddNumbers(1));
        [Test] public void N_0_Returns_0() => Assert.AreEqual(0, _m.SumOddNumbers(0));
        [Test] public void N_Negative_Returns_0() => Assert.AreEqual(0, _m.SumOddNumbers(-7));
        [Test] public void N_Even6_Returns_9() => Assert.AreEqual(9, _m.SumOddNumbers(6));

    }
}

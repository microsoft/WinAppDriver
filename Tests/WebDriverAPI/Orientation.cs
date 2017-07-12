//******************************************************************************
//
// Copyright (c) 2016 Microsoft Corporation. All rights reserved.
//
// This code is licensed under the MIT License (MIT).
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//******************************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WebDriverAPI
{
    [TestClass]
    public class Orientation : CalculatorBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void GetOrientation()
        {
            var orientation = session.Orientation;
            Assert.IsNotNull(orientation);
            Assert.IsTrue(orientation == OpenQA.Selenium.ScreenOrientation.Landscape || orientation == OpenQA.Selenium.ScreenOrientation.Portrait);
        }

        [TestMethod]
        public void GetOrientationError_NoSuchWindow()
        {
            try
            {
                var orientation = Utility.GetOrphanedSession().Orientation;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }
    }
}

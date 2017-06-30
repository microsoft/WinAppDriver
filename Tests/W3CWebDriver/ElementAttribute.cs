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

namespace W3CWebDriver
{
    [TestClass]
    public class ElementAttribute : AlarmClockBase
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
        public void ErrorGetElementAttributeNoSuchWindow()
        {
            try
            {
                var attribute = Utility.GetOrphanedElement().GetAttribute("Attribute");
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void ErrorGetElementAttributeStaleElement()
        {
            try
            {
                var attribute = GetStaleElement().GetAttribute("Attribute");
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }

        [TestMethod]
        public void GetValidElementAttribute()
        {
            // NOTE: The attributes below are only a subset of supported attributes.
            //       Use inspect.exe to identify all available attributes of an element
            var element = alarmTabElement;

            // Fixed value string attributes
            Assert.AreEqual(element.GetAttribute("Name"), "Alarm");
            Assert.AreEqual(element.GetAttribute("AutomationId"), "AlarmPivotItem");
            Assert.AreEqual(element.GetAttribute("FrameworkId"), "XAML");
            Assert.AreEqual(element.GetAttribute("ClassName"), "PivotItem");
            Assert.AreEqual(element.GetAttribute("LegacyName"), "Alarm"); // Shows as Legacy|Accessible.Name in inspect.exe

            // Fixed value boolean attributes
            Assert.AreEqual(element.GetAttribute("IsEnabled"), "True");
            Assert.AreEqual(element.GetAttribute("IsKeyboardFocusable"), "True");
            Assert.AreEqual(element.GetAttribute("IsControlElement"), "True");

            // Arbitrary value attributes
            Assert.IsTrue(System.Convert.ToInt32(element.GetAttribute("ProcessId")) > 0);
            Assert.IsFalse(string.IsNullOrEmpty(element.GetAttribute("RuntimeId")));

            // Arbitrary value array attributes
            Assert.IsFalse(string.IsNullOrEmpty(element.GetAttribute("ClickablePoint")));
            var boundingRectangle = element.GetAttribute("BoundingRectangle");
            Assert.IsTrue(boundingRectangle.Contains("Top"));
            Assert.IsTrue(boundingRectangle.Contains("Left"));
            Assert.IsTrue(boundingRectangle.Contains("Width"));
            Assert.IsTrue(boundingRectangle.Contains("Height"));

            // Pattern specific attribute that may be used along with element.Selected property etc.
            Assert.AreEqual(element.GetAttribute("SelectionItem.IsSelected"), element.Selected.ToString());
            Assert.AreEqual(element.GetAttribute("IsSelectionItemPatternAvailable"), "True");
            Assert.AreEqual(element.GetAttribute("IsSelectionPatternAvailable"), "False");
        }

        [TestMethod]
        public void GetInvalidElementAttribute()
        {
            // Getting the value of an invalid attribute should return null
            string invalidAttribute = alarmTabElement.GetAttribute("InvalidAttribute");
            Assert.AreEqual(invalidAttribute, null);
        }
    }
}

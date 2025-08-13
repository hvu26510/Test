using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using SeleniumExtras.WaitHelpers;


namespace TestProject
{
    [TestFixture]
    public class VitimexTests
    {
        private IWebDriver driver = default!;
        private WebDriverWait wait = default!;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            // options.AddArgument("--headless=new"); // bật nếu muốn chạy ẩn
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            // ngắn hạn implicit wait (explicit wait vẫn dùng)
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(12));
        }


        // Helper: chờ và trả về phần tử đầu tìm được (CSS/Id/Name...)
        private IWebElement WaitAndFind(By by, int timeoutSec = 12)
        {
            var w = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSec));
            return w.Until(d =>
            {
                var elems = d.FindElements(by);
                var e = elems.FirstOrDefault(x => x.Displayed);
                return e;
            });
        }

        // Helper: chọn option trong <select> bằng visible text, trả về true nếu ok
        private bool TrySelectByText(By selectBy, string text)
        {
            try
            {
                var selEl = WaitAndFind(selectBy, 6);
                var select = new SelectElement(selEl);
                select.SelectByText(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Test 1: Tìm cửa hàng Hà Nội + nhập tên cửa hàng
        [Test]
        public void TimCuaHang_HaNoi()
        {
            driver.Navigate().GoToUrl("https://vitimex.com.vn/");

            // Click link "Hệ thống cửa hàng" bằng LinkText (trên trang có link này). Nếu LinkText khác, dùng PartialLinkText.
            WaitAndFind(By.LinkText("Hệ thống cửa hàng")).Click();

            // --- Chọn tỉnh ---
            // thử một loạt selector phổ biến cho <select> tỉnh
            var provinceSelectors = new By[] {
                By.CssSelector("select#province"),
                By.CssSelector("select[name='province']"),
                By.CssSelector("select[id*='province']"),
                By.CssSelector("select") // fallback (cẩn trọng: có thể nhiều select trên trang)
            };

            bool chose = false;
            foreach (var s in provinceSelectors)
            {
                if (TrySelectByText(s, "Hà Nội"))
                {
                    chose = true;
                    break;
                }
            }

            // Nếu không phải <select> mà là custom dropdown (div/li): thao tác click mở rồi chọn option chứa "Hà Nội"
            if (!chose)
            {
                // tìm vùng dropdown phổ biến (tùy site)
                var ddCandidates = driver.FindElements(By.CssSelector("div.dropdown, .nice-select, .custom-select, .selectric"));
                if (ddCandidates.Any())
                {
                    var dd = ddCandidates.First();
                    dd.Click();
                    // sau khi mở, chờ option có text 'Hà Nội'
                    var opt = wait.Until(d =>
                        d.FindElements(By.CssSelector("li, div, .option"))
                         .FirstOrDefault(x => x.Displayed && x.Text.Contains("Hà Nội")));
                    opt?.Click();
                }
            }
            // --- Nhập tên cửa hàng (nếu input có trên trang) ---
            var storeNameSelectors = new By[] {
                By.CssSelector("input#txtShopName"),
                By.CssSelector("input[name='shopName']"),
                By.CssSelector("input[placeholder*='cửa hàng'], input[placeholder*='Tên']")
            };
            foreach (var sel in storeNameSelectors)
            {
                var elems = driver.FindElements(sel);
                if (elems.Count > 0)
                {
                    var input = elems.First();
                    input.Clear();
                    input.SendKeys("Tràng Tiền");
                    break;
                }
            }

            // --- Click nút Tìm / Search ---
            var searchSelectors = new By[] {
                By.CssSelector("button#btnSearch"),
                By.CssSelector("button.btn-search"),
                By.CssSelector("button[type='submit']"),
                By.CssSelector("input[type='submit']")
            };
            foreach (var s in searchSelectors)
            {
                var el = driver.FindElements(s).FirstOrDefault(x => x.Displayed && x.Enabled);
                if (el != null)
                {
                    el.Click();
                    break;
                }
            }

            // --- Chờ kết quả hiển thị (các class phổ biến của list cửa hàng) ---
            var results = wait.Until(d =>
            {
                var r = d.FindElements(By.CssSelector(".store-list, .shop-list, .result, .store-item"));
                return r.Count > 0 ? r : null;
            });

            Assert.IsTrue(results.Count > 0, "Không tìm thấy cửa hàng (kết quả rỗng).");
        }

        








        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                try
                {
                    driver.Quit();    // Đóng trình duyệt
                }
                finally
                {
                    driver.Dispose(); // Giải phóng tài nguyên
                }
            }
        }
    }
}

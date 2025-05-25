using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using log4net;

namespace AlzaCareerApiTests
{
    [TestFixture]
    public class AlzaCareerTests
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AlzaCareerTests));

        public static ILog Log => log;

        private IWebDriver GetVisibleBrowserDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");
            return new ChromeDriver(options);
        }

        private JObject GetParsedJsonFromBrowser(string url)
        {
            using (var driver = GetVisibleBrowserDriver())
            {
                driver.Navigate().GoToUrl(url);
                var body = driver.FindElement(By.TagName("pre")).Text;
                return JObject.Parse(body);
            }
        }

        [Test]
        public void PositionDescription_ShouldNotBeRequired_OnlyForStudentsField()
        {
            log.Info("[TEST] Validating 'forStudents' field type — should be Boolean");
            var content = GetParsedJsonFromBrowser("https://webapi.alza.cz/api/career/v2/positions/java-developer-");
            log.Info("forStudents value: " + content["forStudents"]?.ToString());
            Assert.That(content["forStudents"]?.Type == JTokenType.Boolean, "forStudents should be a boolean");
        }

        [Test]
        public void PlaceOfEmployment_ShouldMatchExpectedAddress()
        {
            log.Info("[TEST] Verifying 'placeOfEmployment' — all address fields must match expected values");
            var content = GetParsedJsonFromBrowser("https://webapi.alza.cz/api/career/v2/positions/java-developer-");
            var place = content["placeOfEmployment"];
            log.Info("Place of Employment:");
            log.Info("Name: " + place?["name"]?.ToString());
            log.Info("State: " + place?["state"]?.ToString());
            log.Info("City: " + place?["city"]?.ToString());
            log.Info("Street: " + place?["streetName"]?.ToString());
            log.Info("Postal Code: " + place?["postalCode"]?.ToString());

            

            Assert.Multiple(() =>
            {
                Assert.That(place?["name"]?.ToString(), Is.EqualTo("Hall office park"));
                Assert.That(place?["state"]?.ToString(), Is.EqualTo("Česká republika"));
                Assert.That(place?["city"]?.ToString(), Is.EqualTo("Praha"));
                Assert.That(place?["streetName"]?.ToString(), Is.EqualTo("U Pergamenky 2"));
                Assert.That(place?["postalCode"]?.ToString(), Is.EqualTo("17000"));
            });
        }
        [Test]
        public void ExecutiveUser_DetailsShouldBeCorrect()
        {
            log.Info("[TEST] Validating executive user details — name, description, and photo");
            var content = GetParsedJsonFromBrowser("https://webapi.alza.cz/api/career/v2/positions/java-developer-");
            var executiveMetaHref = content["executiveUser"]?["meta"]?["href"]?.ToString();

            if (string.IsNullOrEmpty(executiveMetaHref))
            {
                Assert.Fail("executiveUser.meta.href отсутствует");
                return;
            }

            var executiveDetails = GetParsedJsonFromBrowser(executiveMetaHref);

            var name = executiveDetails["name"]?.ToString();
            var description = executiveDetails["description"]?.ToString();
            var image = executiveDetails["image"]?.ToString();

            log.Info("Name: " + name);
            log.Info("Description: " + description);
            log.Info("Foto: " + image);

            Assert.Multiple(() =>
            {
                Assert.That(name, Is.EqualTo("Kozák Michal"), "Name ");
                Assert.That(description, Is.Not.Null.And.Not.Empty, "No description");
                Assert.That(image, Is.Not.Null.And.Not.Empty, "No foto");
            });
        }

    }
}
